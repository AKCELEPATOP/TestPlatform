using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestModels;
using TestService.BindingModels;
using TestService.Interfaces;
using TestService.ViewModels;

namespace TestService.Implementations
{
    public class QuestionService : IQuestion
    {

        private readonly ApplicationDbContext context;

        public QuestionService(ApplicationDbContext context)
        {
            this.context = context;

        }

        public static QuestionService Create(ApplicationDbContext context)
        {
            return new QuestionService(context);
        }

        public async Task AddElement(QuestionBindingModel model)
        {
            Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Text == model.Text);
            if (element != null)
            {
                throw new Exception("Такой вопрос уже существует");
            }
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    element = new Question
                    {
                        Text = model.Text,
                        Complexity = model.Complexity,
                        Active = model.Active,
                        CategoryId = model.CategoryId,
                        Time = model.Time
                    };
                    context.Questions.Add(element);
                    await context.SaveChangesAsync();

                    foreach (var answer in model.Answers)
                    {
                        context.Answers.Add(new Answer
                        {
                            QuestionId = element.Id,
                            Text = answer.Text,
                            True = answer.True,
                        });
                    }
                    await context.SaveChangesAsync();
                    if (model.Attachments != null)
                    {
                        foreach (var attachment in model.Attachments)
                        {
                            var buffer = Convert.FromBase64String(attachment.Image);

                            HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(buffer);

                            try
                            {
                                if (objFile != null && objFile.ContentLength > 0)
                                {
                                    string path = model.ImagesPath + $@"{DateTime.Now.Ticks}.png"; ;

                                    objFile.SaveAs(path);

                                    context.Attachments.Add(new Attachment
                                    {
                                        Path = path,
                                        QuestionId = element.Id
                                    });
                                    await context.SaveChangesAsync();
                                }
                            }
                            catch(Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.Answers.RemoveRange(context.Answers.Where(rec => rec.QuestionId == element.Id));

                        var list = await context.Attachments.Where(rec => rec.QuestionId == element.Id).ToListAsync();

                        foreach (var el in list)
                        {
                            System.IO.File.Delete(el.Path);
                        }
                        context.Attachments.RemoveRange(list);

                        context.Questions.Remove(element);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Ошибка удаления");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<QuestionViewModel> GetElement(int id)
        {
            var result = await context.Questions.Where(rec => rec.Id == id).Select(rec=>new QuestionViewModel
            {
                Id = rec.Id,
                Text = rec.Text,
                Answers = rec.Answers.Select(recQ => new AnswerViewModel
                {
                    Id = recQ.Id,
                    Text = recQ.Text,
                    True = recQ.True
                }).ToList(),
                Complexity = rec.Complexity,
                CategoryName = rec.Category.Name,
                Active = rec.Active,
                CategoryId = rec.CategoryId,
                ComplexityName = rec.Complexity.ToString(),
                Time = rec.Time
            }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("Элемент не найден");
            }
            else
            {
                List<AttachmentViewModel> attachments = new List<AttachmentViewModel>();

                var list = await context.Attachments.Where(rec => rec.QuestionId == result.Id).ToListAsync();

                foreach (var el in list)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(el.Path);

                    attachments.Add(new AttachmentViewModel
                    {
                        Image = Convert.ToBase64String(bytes),
                        Id = el.Id
                    });
                }
                if (attachments.Count > 0)
                {
                    result.Images = attachments;
                }
                return result;
            }


        }

        public async Task UpdElement(QuestionBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Question element = await context.Questions.FirstOrDefaultAsync(rec =>
                                  rec.Text == model.Text && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть такой вопрос");
                    }
                    element = context.Questions.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.Text = model.Text;
                    element.Complexity = model.Complexity;
                    element.Active = model.Active;
                    element.Time = model.Time;
                    await context.SaveChangesAsync();

                    var answersId = model.Answers.Select(rec => rec.Id).Distinct();

                    //обновление вопросов
                    var updateAnswers = context.Answers.Where(rec => rec.QuestionId == model.Id && answersId.Contains(rec.Id));

                    foreach (var updateAnswer in updateAnswers)
                    {
                        updateAnswer.Text = model.Answers.FirstOrDefault(rec => rec.Id == updateAnswer.Id).Text;
                        updateAnswer.True = model.Answers.FirstOrDefault(rec => rec.Id == updateAnswer.Id).True;
                    }
                    await context.SaveChangesAsync();
                    //удаление вопросов
                    context.Answers.RemoveRange(context.Answers.Where(rec => rec.QuestionId == model.Id && !answersId.Contains(rec.Id)));
                    await context.SaveChangesAsync();

                    var answers = model.Answers
                        .Where(rec => rec.Id == 0);

                    foreach (var answer in answers)
                    {
                        context.Answers.Add(new Answer
                        {
                            QuestionId = model.Id,
                            Text = answer.Text,
                            True = answer.True,
                        });
                        await context.SaveChangesAsync();
                    }

                    //обновление приложений
                    if (model.Attachments != null)
                    {
                        var attachIds = model.Attachments.Select(rec => rec.Id).Distinct();

                        var updateAttachments = context.Attachments.Where(rec => rec.QuestionId == model.Id && answersId.Contains(rec.Id));

                        foreach (var updateAttach in updateAttachments)
                        {
                            var buffer = Convert.FromBase64String(model.Attachments.FirstOrDefault(rec => rec.Id == updateAttach.Id).Image);

                            HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(buffer);

                            try
                            {
                                if (objFile != null && objFile.ContentLength > 0)
                                {
                                    string path = updateAttach.Path;//возможна ошибка

                                    objFile.SaveAs(path);

                                    await context.SaveChangesAsync();
                                }
                            }
                            catch
                            {
                                throw new Exception("Не удалось обновить изображение");
                            }
                        }

                        context.Attachments.RemoveRange(context.Attachments.Where(rec => rec.QuestionId == model.Id && !attachIds.Contains(rec.Id)));

                        await context.SaveChangesAsync();

                        var attachments = model.Attachments
                            .Where(rec => rec.Id == 0);

                        foreach (var attach in attachments)
                        {

                            var buffer = Convert.FromBase64String(attach.Image);

                            HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(buffer);

                            try
                            {
                                if (objFile != null && objFile.ContentLength > 0)
                                {
                                    string path = model.ImagesPath + ((string.IsNullOrEmpty(objFile.FileName)) ? string.Format("{0}.{1}.png", element.Id, 1) : objFile.FileName);

                                    objFile.SaveAs(path);

                                    context.Attachments.Add(new Attachment
                                    {
                                        Path = path,
                                        QuestionId = element.Id
                                    });
                                    await context.SaveChangesAsync();
                                }
                            }
                            catch
                            {
                                throw new Exception("Не удалось добавить изображение");
                            }
                            await context.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
