using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    
                    foreach(var answer in model.Answers)
                    {
                        context.Answers.Add(new Answer
                        {
                            QuestionId = element.Id,
                            Text = answer.Text,
                            True = answer.True
                        });
                    }
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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
                        context.Questions.Remove(element);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
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
            Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Id == id);
            
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    QuestionViewModel result = new QuestionViewModel
                    {
                        Id = element.Id,
                        Text=element.Text,
                        Answers = element.Answers.Select(recQ => new AnswerViewModel
                        {
                            Id = recQ.Id,
                            Text=recQ.Text,
                            True = recQ.True
                        }).ToList(),
                        Complexity=element.Complexity,
                        CategoryName=element.Category.Name,
                        Active = element.Active,
                        CategoryId = element.CategoryId,
                        ComplexityName = element.Complexity.ToString(),
                        Time = element.Time
                    };
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

                    foreach(var answer in answers)
                    {
                        context.Answers.Add(new Answer
                        {
                            QuestionId = model.Id,
                            Text = answer.Text,
                            True = answer.True,
                        });
                        await context.SaveChangesAsync();
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
