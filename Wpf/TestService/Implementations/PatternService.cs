using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModels;
using TestService.BindingModels;
using TestService.Interfaces;
using TestService.ViewModels;
using System.Data.Entity;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.IO;

namespace TestService.Implementations
{
    public class PatternService : IPatternService
    {
        private ApplicationDbContext context;

        private int min = 60;

        public PatternService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static PatternService Create(ApplicationDbContext context)
        {
            return new PatternService(context);
        }

        public async Task Add(PatternBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var element = context.Patterns.FirstOrDefault(rec => rec.Name == model.Name);
                    if (element != null)
                    {
                        throw new Exception("Есть шаблон с таким названием");
                    }
                    element = new Pattern
                    {
                        Name = model.Name,
                        UserGroupId = model.UserGroupId
                    };
                    context.Patterns.Add(element);
                    await context.SaveChangesAsync();

                    var groupCategories = model.PatternCategories.GroupBy(rec => rec.CategoryId).Select(rec => new PatternCategoriesBindingModel
                    {
                        CategoryId = rec.Key,
                        Easy = rec.Select(r => r.Easy).FirstOrDefault(),
                        Copmlex = rec.Select(r => r.Copmlex).FirstOrDefault(),
                        Middle = rec.Select(r => r.Middle).FirstOrDefault()
                    });
                    foreach (var groupCategory in groupCategories)
                    {
                        context.PatternCategories.Add(new PatternCategory
                        {
                            PatternId = element.Id,
                            Easy = groupCategory.Easy,
                            Complex = groupCategory.Copmlex,
                            Middle = groupCategory.Middle,
                            CategoryId = groupCategory.CategoryId
                        });
                    }

                    await context.SaveChangesAsync();

                    if (model.PatternQuestions != null)
                    {
                        var groupQuestions = model.PatternQuestions.GroupBy(rec => rec.QuestionId).Select(g => g.First()).ToList();

                        foreach (var groupQuestion in groupQuestions)
                        {
                            context.PatternQuestions.Add(new PatternQuestion
                            {
                                PatternId = element.Id,
                                QuestionId = groupQuestion.QuestionId
                            });
                        }
                        await context.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task Del(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Pattern element = context.Patterns.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.PatternCategories.RemoveRange(
                                            context.PatternCategories.Where(rec => rec.PatternId == id));
                        context.PatternQuestions.RemoveRange(
                                            context.PatternQuestions.Where(rec => rec.PatternId == id));
                        context.Patterns.Remove(element);
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

        public async Task<PatternViewModel> Get(int id)
        {
            Pattern element = await context.Patterns.FirstOrDefaultAsync(rec => rec.Id == id);
            List<PatternCategoryViewModel> patternCategories = await context.PatternCategories.Where(rec => rec.PatternId == element.Id).GroupJoin(context.PatternQuestions.Where(rec => rec.PatternId == element.Id).Include(rec => rec.Question),
                category => category.CategoryId,
                question => question.Question.CategoryId,
                (category, questions) => new PatternCategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.Category.Name,
                    Complex = category.Complex,
                    Middle = category.Middle,
                    Easy = category.Easy,
                    Id = category.Id,
                    PatternId = category.PatternId,
                    Count = category.Complex + category.Easy + category.Middle,
                    PatternQuestions = questions.Select(rec => new PatternQuestionViewModel
                    {
                        Id = rec.Id,
                        Complexity = rec.Question.Complexity.ToString(),
                        PatternId = element.Id,
                        QuestionId = rec.QuestionId,
                        QuestionText = rec.Question.Text
                    }).ToList()
                }).ToListAsync();
            if (element != null)
            {
                return new PatternViewModel
                {
                    Id = element.Id,
                    Name = element.Name,
                    UserGroupId = element.UserGroupId ?? -1,
                    UserGroupName = element.UserGroupId.HasValue ? context.UserGroups.FirstOrDefault(rec => rec.Id == element.UserGroupId.Value).Name : "",
                    PatternCategories = patternCategories
                };
            }
            throw new Exception("Элемент не найден");
        }

        public async Task<List<PatternViewModel>> GetList()
        {
            return await context.Patterns.Include(rec => rec.UserGroup).Select(rec => new PatternViewModel
            {
                Id = rec.Id,
                Name = rec.Name,
                UserGroupId = rec.UserGroupId ?? -1,
                UserGroupName = rec.UserGroup.Name
            }).ToListAsync();
        }

        public async Task Upd(PatternBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Pattern pattern = context.Patterns.FirstOrDefault(rec => rec.Name == model.Name && rec.Id != model.Id);
                    if (pattern != null)
                    {
                        throw new Exception("Уже есть шаблон с таким названием");
                    }
                    pattern = context.Patterns.FirstOrDefault(rec => rec.Id == model.Id);
                    if (pattern == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    pattern.Name = model.Name;
                    pattern.UserGroupId = model.UserGroupId;
                    await context.SaveChangesAsync();

                    //обновление категорий
                    var categoriesIds = model.PatternCategories.Select(rec => rec.CategoryId).Distinct();

                    var updateCategories = context.PatternCategories.Where(rec => rec.PatternId == model.Id && categoriesIds.Contains(rec.CategoryId));

                    foreach (var updateCategory in updateCategories)
                    {
                        updateCategory.Complex = model.PatternCategories.FirstOrDefault(rec => rec.CategoryId == updateCategory.CategoryId).Copmlex;
                        updateCategory.Middle = model.PatternCategories.FirstOrDefault(rec => rec.CategoryId == updateCategory.CategoryId).Middle;
                        updateCategory.Easy = model.PatternCategories.FirstOrDefault(rec => rec.CategoryId == updateCategory.CategoryId).Easy;
                        await context.SaveChangesAsync();
                    }


                    context.PatternCategories.RemoveRange(
                        context.PatternCategories.Where(rec => rec.PatternId == model.Id && !categoriesIds.Contains(rec.CategoryId)));

                    await context.SaveChangesAsync();

                    var groupCategories = model.PatternCategories
                        .Where(rec => rec.Id == 0)
                        .GroupBy(rec => rec.CategoryId)
                        .Select(rec => new PatternCategoryViewModel
                        {
                            CategoryId = rec.Key,
                            Easy = rec.Select(r => r.Easy).FirstOrDefault(),
                            Complex = rec.Select(r => r.Copmlex).FirstOrDefault(),
                            Middle = rec.Select(r => r.Middle).FirstOrDefault()
                        });

                    foreach (var groupCategory in groupCategories)
                    {
                        PatternCategory element = await context.PatternCategories.FirstOrDefaultAsync(rec => rec.PatternId == model.Id &&
                                                                rec.CategoryId == groupCategory.CategoryId);
                        if (element != null)
                        {
                            element.Easy += groupCategory.Easy;
                            element.Complex = groupCategory.Complex;
                            element.Middle = groupCategory.Middle;
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            context.PatternCategories.Add(new PatternCategory
                            {
                                PatternId = model.Id,
                                Easy = groupCategory.Easy,
                                Complex = groupCategory.Complex,
                                Middle = groupCategory.Middle,
                                CategoryId = groupCategory.CategoryId,
                            });
                            await context.SaveChangesAsync();
                        }
                    }
                    //обновление вопросов
                    var questionsIds = model.PatternQuestions.Select(rec => rec.QuestionId).Distinct();

                    var updateQuestions = context.PatternQuestions.Where(rec => rec.PatternId == model.Id && questionsIds.Contains(rec.QuestionId));

                    context.PatternQuestions.RemoveRange(
                        context.PatternQuestions.Where(rec => rec.PatternId == model.Id && !questionsIds.Contains(rec.QuestionId)));

                    await context.SaveChangesAsync();

                    var groupQuestions = model.PatternQuestions
                        .Where(rec => rec.Id == 0)
                        .GroupBy(rec => rec.QuestionId)
                        .Select(rec => new PatternQuestionViewModel
                        {
                            QuestionId = rec.Key
                        });

                    foreach (var groupQuestion in groupQuestions)
                    {
                        PatternQuestion patternQuestion = await context.PatternQuestions.FirstOrDefaultAsync(rec => rec.PatternId == model.Id &&
                                                                rec.QuestionId == groupQuestion.QuestionId);

                        if (patternQuestion == null)
                        {
                            context.PatternQuestions.Add(new PatternQuestion
                            {
                                PatternId = model.Id,
                                QuestionId = groupQuestion.QuestionId
                            });
                            await context.SaveChangesAsync();
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

        public async Task<TestViewModel> CreateTest(int patternId)
        {
            PatternUsingModel model = await context.Patterns.Where(rec => rec.Id == patternId).GroupJoin(
                context.PatternQuestions.Include(rec => rec.Question).Include(rec => rec.Question.Answers)
                , p => p.Id, pq => pq.PatternId, (p, pq) => new PatternUsingModel
                {
                    Name = p.Name,
                    PatternCategories = p.PatternCategories.Select(rec => new PatternCategoryViewModel
                    {
                        CategoryId = rec.CategoryId,
                        Complex = rec.Complex,
                        Count = rec.Easy + rec.Complex + rec.Middle,
                        Middle = rec.Middle,
                        Easy = rec.Easy,
                        PatternId = rec.PatternId,

                    }).ToList(),
                    Questions = pq.Select(rec => new QuestionViewModel
                    {
                        Text = rec.Question.Text,
                        CategoryId = rec.Question.CategoryId,
                        Complexity = rec.Question.Complexity,
                        Time = rec.Question.Time,
                        Answers = rec.Question.Answers.Select(r => new AnswerViewModel
                        {
                            Id = r.Id,
                            Text = r.Text
                        }).ToList(),
                        Multi = (rec.Question.Answers.Where(r => r.True).Count() > 1),
                        CategoryName = rec.Question.Category.Name,
                        Images = rec.Question.Attachments.Select(r => new AttachmentViewModel
                        {
                            Image = r.Path

                        }).ToList(),
                        Id = rec.QuestionId
                    }).ToList()
                }).FirstOrDefaultAsync();
            TestViewModel result = new TestViewModel
            {
                Name = model.Name,
                PatternId = patternId,
                Questions = new List<QuestionViewModel>()
            };
            if (model.Questions != null && model.Questions.Count > 0)
            {
                result.Questions.AddRange(model.Questions);
            }
            var questionIds = result.Questions.Select(r => r.Id);
            foreach (var patternCategory in model.PatternCategories)
            {
                //добавление сложных
                int countComplex = patternCategory.Complex - model.Questions.Where(rec => rec.Complexity.Equals(QuestionComplexity.Difficult)).Count();
                if (countComplex > 0)
                {
                    result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                    rec.Complexity == QuestionComplexity.Difficult && rec.Active && !questionIds.Contains(rec.Id)).OrderBy(a => Guid.NewGuid())
                        .Take(countComplex).Select(rec => new QuestionViewModel
                        {
                            Id = rec.Id,
                            Time = rec.Time,
                            Text = rec.Text,
                            Answers = rec.Answers.Select(r => new AnswerViewModel
                            {
                                Id = r.Id,
                                Text = r.Text
                            }).ToList(),
                            Multi = (rec.Answers.Where(r => r.True).Count() > 1),
                            CategoryName = rec.Category.Name,
                            Images = rec.Attachments.Select(r => new AttachmentViewModel
                            {
                                Image = r.Path

                            }).ToList()
                        }));
                }
                //добавление средних
                int countMiddle = patternCategory.Middle - model.Questions.Where(rec => rec.Complexity.Equals(QuestionComplexity.Middle)).Count();
                if (countMiddle > 0)
                {
                    result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                    rec.Complexity == QuestionComplexity.Middle && rec.Active && !questionIds.Contains(rec.Id)).OrderBy(a => Guid.NewGuid())
                        .Take(countMiddle).Select(rec => new QuestionViewModel
                        {
                            Id = rec.Id,
                            Time = rec.Time,
                            Text = rec.Text,
                            Answers = rec.Answers.Select(r => new AnswerViewModel
                            {
                                Id = r.Id,
                                Text = r.Text
                            }).ToList(),
                            Multi = (rec.Answers.Where(r => r.True).Count() > 1),
                            CategoryName = rec.Category.Name,
                            Images = rec.Attachments.Select(r => new AttachmentViewModel
                            {
                                Image = r.Path

                            }).ToList()
                        }));
                }
                //добавление легких
                int countEasy = patternCategory.Easy - model.Questions.Where(rec => rec.Complexity.Equals(QuestionComplexity.Easy)).Count();
                if (countEasy > 0)
                {
                    result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                    rec.Complexity == QuestionComplexity.Easy && rec.Active && !questionIds.Contains(rec.Id)).OrderBy(a => Guid.NewGuid())
                        .Take(countEasy).Select(rec => new QuestionViewModel
                        {
                            Id = rec.Id,
                            Time = rec.Time,
                            Text = rec.Text,
                            Answers = rec.Answers.Select(r => new AnswerViewModel
                            {
                                Id = r.Id,
                                Text = r.Text
                            }).ToList(),
                            Multi = (rec.Answers.Where(r => r.True).Count() > 1),
                            CategoryName = rec.Category.Name,
                            Images = rec.Attachments.Select(r => new AttachmentViewModel
                            {
                                Image = r.Path

                            }).ToList()
                        }));
                }
            }
            //result.Questions = result.Questions.GroupBy(p => p.Id).Select(p => p.First()).ToList();
            foreach (var question in result.Questions)
            {
                for (int i = 0; i < question.Images.Count; i++)
                {
                    try
                    {
                        question.Images[i].Image = Convert.ToBase64String(File.ReadAllBytes(question.Images[i].Image));
                    }
                    catch (Exception ex)
                    {
                        question.Images[i].Image = ex.Message;
                    }
                }
            }

            result.Time = result.Questions.Select(rec => rec.Time).DefaultIfEmpty(0).Sum();
            return result;
        }

        public async Task<StatViewModel> CheakTest(TestResponseModel model)
        {
            var user = context.Users.Where(rec => rec.Id.Equals(model.UserId)).Select(rec => new UserViewModel
            {
                FIO = rec.FIO,
                Email = rec.Email
            }).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Данный пользователь не существует");
            }
            StatViewModel result = new StatViewModel
            {
                UserName = user.FIO,
                Email = user.Email ?? "",
                StatCategories = new List<StatCategoryViewModel>()
            };


            result.StatCategories.AddRange(context.PatternCategories.Where(rec => rec.PatternId == model.PatternId).Include(rec => rec.Category)
                .Select(rec => new StatCategoryViewModel
                {
                    CategoryId = rec.CategoryId,
                    CategoryName = rec.Category.Name
                }));

            foreach (var questionResponce in model.QuestionResponses)
            {
                var question = await context.Questions.Where(rec => rec.Id == questionResponce.QuestionId).Include(rec => rec.Answers).Select(rec => new QuestionViewModel
                {
                    Id = rec.Id,
                    Text = rec.Text,
                    CategoryId = rec.CategoryId,
                    Answers = rec.Answers.Select(r => new AnswerViewModel
                    {
                        Id = r.Id,
                        True = r.True
                    }).ToList(),
                    Complexity = rec.Complexity
                }).FirstOrDefaultAsync();

                var list = result.StatCategories.Find(rec => rec.CategoryId == question.CategoryId);
                if (list == null)
                {
                    throw new Exception("В шаблоне отсутствует данная категория");
                }
                list.Total += (int)question.Complexity;
                bool right = CheakAnswers(question.Answers, questionResponce.Answers);
                if (right)
                {
                    list.Right += (int)question.Complexity;
                }
                list.Questions = new List<StatQuestionViewModel>();
                list.Questions.Add(new StatQuestionViewModel
                {
                    Text = question.Text,
                    Right = right
                });
            }

            result.Total = result.StatCategories.Select(rec => rec.Total).DefaultIfEmpty(0).Sum();
            result.Right = result.StatCategories.Select(rec => rec.Right).DefaultIfEmpty(0).Sum();
            result.Mark = (int)((double)result.Right / result.Total * 100);
            context.Stats.Add(new Stat
            {
                PatternId = model.PatternId,
                DateCreate = DateTime.Now,
                UserId = model.UserId,
                Right = result.Right,
                Total = result.Total
            });
            await context.SaveChangesAsync();

            Task task = Task.Run(async () => await SendMail(result.Email, "Вы прошли тест!", CreateMessage(result)));
            //отправка
            return result;
        }

        public bool CheakAnswers(List<AnswerViewModel> answers, List<int> answer)
        {
            foreach (var a in answers)
            {
                if (a.True)
                {
                    if (!answer.Contains(a.Id)) return false;
                }
                else if (answer.Contains(a.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public async System.Threading.Tasks.Task SendMail(string mailto, string caption, string message, string path = null)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            SmtpClient stmpClient = null;
            try
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                mailMessage.To.Add(new MailAddress(mailto));
                mailMessage.Subject = caption;
                mailMessage.Body = message;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                if (path != null)
                {
                    mailMessage.Attachments.Add(new System.Net.Mail.Attachment(path));
                }
                mailMessage.IsBodyHtml = true;

                stmpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"].Split('@')[0],
                    ConfigurationManager.AppSettings["MailPassword"])
                };
                await stmpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mailMessage.Dispose();
                mailMessage = null;
                stmpClient = null;
            }
        }

        private string CreateMessage(StatViewModel model)
        {
            return string.Format("<div style=\"max-width:640px;margin:0 auto;border-radius:4px;overflow:hidden\"><div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:40px 40px 0px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"color:#000;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:13px;line-height:22px;table-layout:auto\" width=\"100%\" border=\"0\">" +
"<tbody><tr>" +
"<td>" +
"<div style=\"color:#4f545c;font-size:20px;line-height:24px\">" +
"<span style=\"font-weight:bold\">Привет {0},</span>" +
"</div>" +
"<div style=\"font-size:16px;line-height:22px;color:#737f8d;margin-top:12px\">" +
"{1}" +
"</div>" +
"</td>" +
"<td width=\"78\" valign=\"top\" align=\"right\">" +
"<table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;border-spacing:0px\" align=\"center\" border=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"width:58px;height:48px\">" +
"<img src=\"https://cdn2.iconfinder.com/data/icons/social-buttons-2/512/mail-512.png\" width=\"64\" height=\"64\" style=\"border:none;display:block;outline:none;text-decoration:none\" alt=\"\" class=\"CToWUd\">" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody></table></td></tr></tbody></table></div></td></tr></tbody></table></div>" +
"<div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 0px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\"><p style=\"font-size:1px;margin:0px auto;border-top:1px solid #f0f2f4;width:100%\"></p></td></tr></tbody></table></div></td></tr></tbody></table></div>" +
"<div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 10px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><div style=\"color:#7289da;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:16px;line-height:15px;text-align:left\">" +
"<strong>" +
" <span style=\"color:#7289da;font-size:14px\">{3}</span></strong>" +
"</div></td></tr></tbody></table></div></td></tr></tbody></table></div>" +
"<div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:0px 40px 10px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"color:#000;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:13px;line-height:22px;table-layout:auto\" width=\"100%\" border=\"0\">" +
"<tbody><tr>" +
"<!--td width=\"20\">" +
"&nbsp;" +
"</td-->" +
"<td valign=\"top\">" +
"<div style=\"color:#2d3136;font-size:16px;line-height:20px\">{4}</div>" +
"<div style=\"color:#737f8d;font-size:15px;line-height:17px;margin-top:3px\">" +
" <!-- таблица результата-->" +
" <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:13px;width:100%;background:#ffffff\" align=\"center\" border=\"0\">" +
" <tbody>" +
" <tr style=\"color: #0083BE;font-size: 16px; font-weight: bold;\">" +
" <td>" +
" Категория" +
" </td>" +
" <td>" +
" Всего" +
" </td>" +
" <td>" +
" Результат" +
" </td>" +
" </tr>" +
" {5}" +
" </tbody>" +
" </table>" +
"</div>" +
"</td>" +
"</tr>" +
"</tbody></table></td></tr></tbody></table></div></td></tr></tbody></table></div>" +
"<div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 0px\"></td></tr></tbody></table></div>" +
"</div>", model.UserName, "Вы только что прошли тест", "https://cdn2.iconfinder.com/data/icons/social-buttons-2/512/mail-512.png", model.PatternName, GetMarkField(model.Mark), FillStatTable(model.StatCategories));
        }

        private string FillStatTable(List<StatCategoryViewModel> list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var elem in list)
            {
                builder.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", elem.CategoryName, elem.Total, elem.Right);
            }
            return builder.ToString();
        }

        private string GetMarkField(int mark)
        {
            return string.Format("<b style=\"color: {0}; font-size: 20px\">{1}%</b>", (mark >= min) ? "#2FC74B" : "#DB141E", mark);
        }

        public async Task<List<PatternViewModel>> GetUserList(string id)
        {
            int? groupId = (await context.Users.FirstOrDefaultAsync(rec => rec.Id == id)).UserGroupId;
            return await GetGroupList(groupId);
        }

        public async Task<List<PatternViewModel>> GetGroupList(int? id)
        {
            if (id.HasValue)
            {
                return await context.Patterns.Where(rec => rec.UserGroupId == id || rec.UserGroup == null)
                    .Where(rec => !(rec.PatternCategories.Select(r => r.Category).Any(r => !r.Active))).Select(rec => new PatternViewModel
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        UserGroupId = rec.UserGroupId,
                        UserGroupName = rec.UserGroup.Name ?? "Общий"
                    }).ToListAsync();
            }
            else
            {
                //не знаю будет ли работать
                return await context.Patterns.Where(rec => rec.UserGroup == null)
                    .Where(rec => !(rec.PatternCategories.Select(r => r.Category).Any(r => !r.Active))).Select(rec => new PatternViewModel
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        UserGroupName = "Общий"
                    }).ToListAsync();
            }
        }
    }
}
