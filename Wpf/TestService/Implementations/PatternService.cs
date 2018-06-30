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

namespace TestService.Implementations
{
    public class PatternService : IPatternService
    {
        private ApplicationDbContext context;

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
                    var element = new Pattern
                    {
                        Name = model.Name,
                        UserGroupId = model.UserGroupId
                    };
                    context.Patterns.Add(element);
                    await context.SaveChangesAsync();

                    var groupCategories = model.PatternCategories.GroupBy(rec => rec.CategoryId).Select(rec => new PatternCategoriesBindingModel
                    {
                        CategoryId = rec.Key,
                        Count = rec.Sum(r => r.Count),
                        Copmlex = rec.Select(r => r.Copmlex).FirstOrDefault(),
                        Middle = rec.Select(r => r.Middle).FirstOrDefault()
                    });
                    foreach (var groupCategory in groupCategories)
                    {
                        context.PatternCategories.Add(new PatternCategory
                        {
                            PatternId = element.Id,
                            Count = groupCategory.Count,
                            Complex = groupCategory.Copmlex,
                            Middle = groupCategory.Middle,
                            CategoryId = groupCategory.CategoryId
                        });
                    }

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
                    transaction.Commit();
                }
                catch (Exception)
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
                    Easy = 1 - category.Complex - category.Middle,
                    Id = category.Id,
                    PatternId = category.PatternId,
                    Count = category.Count,
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
                        updateCategory.Complex = model.PatternCategories.FirstOrDefault(rec => rec.Id == updateCategory.Id).Copmlex;
                        updateCategory.Middle = model.PatternCategories.FirstOrDefault(rec => rec.Id == updateCategory.Id).Middle;
                        updateCategory.Count = model.PatternCategories.FirstOrDefault(rec => rec.Id == updateCategory.Id).Count;
                    }
                    await context.SaveChangesAsync();

                    context.PatternCategories.RemoveRange(
                        context.PatternCategories.Where(rec => rec.PatternId == model.Id && !categoriesIds.Contains(rec.CategoryId)));

                    await context.SaveChangesAsync();

                    var groupCategories = model.PatternCategories
                        .Where(rec => rec.Id == 0)
                        .GroupBy(rec => rec.CategoryId)
                        .Select(rec => new PatternCategoryViewModel
                        {
                            CategoryId = rec.Key,
                            Count = rec.Sum(r => r.Count),
                            Complex = rec.Select(r => r.Copmlex).FirstOrDefault(),
                            Middle = rec.Select(r => r.Middle).FirstOrDefault()
                        });

                    foreach (var groupCategory in groupCategories)
                    {
                        PatternCategory element = await context.PatternCategories.FirstOrDefaultAsync(rec => rec.PatternId == model.Id &&
                                                                rec.CategoryId == groupCategory.CategoryId);
                        if (element != null)
                        {
                            element.Count += groupCategory.Count;
                            element.Complex = groupCategory.Complex;
                            element.Middle = groupCategory.Middle;
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            context.PatternCategories.Add(new PatternCategory
                            {
                                PatternId = model.Id,
                                Count = groupCategory.Count,
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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
                        Count = rec.Count,
                        Middle = rec.Middle,
                        Easy = 1 - rec.Complex - rec.Middle,
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
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
            TestViewModel result = new TestViewModel
            {
                Name = model.Name,
                PatternId = patternId
            };
            foreach (var patternCategory in model.PatternCategories)
            {
                List<QuestionViewModel> list = model.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId).ToList();
                if (list != null)
                {
                    result.Questions.AddRange(list);
                }
                //добавление сложных
                int countComplex = (int)(patternCategory.Complex * patternCategory.Count) - list.Where(rec => rec.Complexity.Equals(QuestionComplexity.Difficult)).Count();
                result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                rec.Complexity == QuestionComplexity.Difficult && rec.Active).OrderBy(a => Guid.NewGuid())
                    .Take(countComplex).Select(rec => new QuestionViewModel
                    {
                        Id = rec.Id,
                        Time = rec.Time,
                        Answers = rec.Answers.Select(r => new AnswerViewModel
                        {
                            Id = r.Id,
                            Text = r.Text
                        }).ToList()
                    }));
                //добавление средних
                int countMiddle = (int)(patternCategory.Middle * patternCategory.Count) - list.Where(rec => rec.Complexity.Equals(QuestionComplexity.Middle)).Count();
                result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                rec.Complexity == QuestionComplexity.Middle && rec.Active).OrderBy(a => Guid.NewGuid())
                    .Take(countMiddle).Select(rec => new QuestionViewModel
                    {
                        Id = rec.Id,
                        Time = rec.Time,
                        Answers = rec.Answers.Select(r => new AnswerViewModel
                        {
                            Id = r.Id,
                            Text = r.Text
                        }).ToList()
                    }));

                //добавление легких
                int countEasy = (patternCategory.Count - (int)(patternCategory.Complex * patternCategory.Count) -
                    (int)(patternCategory.Middle * patternCategory.Count)) -
                    list.Where(rec => rec.Complexity.Equals(QuestionComplexity.Easy)).Count();

                result.Questions.AddRange(context.Questions.Where(rec => rec.CategoryId == patternCategory.CategoryId &&
                rec.Complexity == QuestionComplexity.Easy && rec.Active).OrderBy(a => Guid.NewGuid())
                    .Take(countEasy).Select(rec => new QuestionViewModel
                    {
                        Id = rec.Id,
                        Time = rec.Time,
                        Answers = rec.Answers.Select(r => new AnswerViewModel
                        {
                            Id = r.Id,
                            Text = r.Text
                        }).ToList()
                    }));

            }
            result.Time = result.Questions.Select(rec => rec.Time).DefaultIfEmpty(0).Sum();
            return result;
        }

        public async Task<StatViewModel> CheakTest(TestResponseModel model)
        {
            StatViewModel result = new StatViewModel();

            Task getUserData = Task.Run(() => {
                var user = context.Users.Where(rec => rec.Id == model.UserId).Select(rec => new UserViewModel
                {
                    FIO = rec.FIO,
                    Email = rec.Email
                }).FirstOrDefault();
                result.UserName = user.FIO;
                result.Email = user.Email;
            });

            var questionCount = context.Patterns.FirstOrDefault(rec => rec.Id == model.PatternId)
                .PatternCategories.Select(rec => rec.Count).DefaultIfEmpty(0).Sum();
            if (model.QuestionResponses.Count != questionCount)
            {
                throw new Exception("Не совпадает количество вопросов");
            }

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
                    list.Right+= (int)question.Complexity;
                }
                list.Questions.Add(new StatQuestionViewModel
                {
                    Text = question.Text,
                    Right = right
                });
            }

            result.Total = result.StatCategories.Select(rec=>rec.Total).DefaultIfEmpty(0).Sum();
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

            await getUserData;
            Task task = Task.Run(() => SendMail(result.Email, "Вы прошли тест", CreateMessage(result)));
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
                    mailMessage.Attachments.Add(new Attachment(path));
                }

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
            return string.Format("<div style=\"max-width:640px;margin:0 auto;border-radius:4px;overflow:hidden\"><div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:40px 40px 0px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"color:#000;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:13px;line-height:22px;table-layout:auto\" width=\"100%\" border=\"0\">\n" +
        "            <tbody><tr>\n" +
        "              <td>\n" +
        "                <div style=\"color:#4f545c;font-size:20px;line-height:24px\">\n" +
        "                  <span style=\"font-weight:bold\">Привет {0},</span>\n" +
        "                </div>\n" +
        "                <div style=\"font-size:16px;line-height:22px;color:#737f8d;margin-top:12px\">\n" +
        "                  {1}\n" +
        "                </div>\n" +
        "              </td>\n" +
        "              <td width=\"78\" valign=\"top\" align=\"right\">\n" +
        "                <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;border-spacing:0px\" align=\"center\" border=\"0\">\n" +
        "                  <tbody>\n" +
        "                    <tr>\n" +
        "                      <td style=\"width:58px;height:48px\">\n" +
        "                        <img src=\"{2}\" width=\"64\" height=\"64\" style=\"border:none;display:block;outline:none;text-decoration:none\" alt=\"\" class=\"CToWUd\">\n" +
        "                      </td>\n" +
        "                    </tr>\n" +
        "                  </tbody>\n" +
        "                </table>\n" +
        "              </td>\n" +
        "            </tr>\n" +
        "          </tbody></table></td></tr></tbody></table></div></td></tr></tbody></table></div>\n" +
        "      <div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 0px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\"><p style=\"font-size:1px;margin:0px auto;border-top:1px solid #f0f2f4;width:100%\"></p></td></tr></tbody></table></div></td></tr></tbody></table></div>\n" +
        "      <div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 10px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><div style=\"color:#7289da;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:16px;line-height:15px;text-align:left\">\n" +
        "      <strong>\n" +
        "       <span style=\"color:#7289da;font-size:14px\">{3}</span></strong>\n" +
        "    </div></td></tr></tbody></table></div></td></tr></tbody></table></div>\n" +
        "      <div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:0px 40px 10px\"><div aria-labelledby=\"mj-column-per-100\" class=\"m_-5457715721865842861mj-column-per-100 m_-5457715721865842861outlook-group-fix\" style=\"vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"><tbody><tr><td style=\"word-break:break-word;font-size:0px;padding:0px\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"color:#000;font-family:Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif;font-size:13px;line-height:22px;table-layout:auto\" width=\"100%\" border=\"0\">\n" +
        "      <tbody><tr>\n" +
        "        <!--td width=\"20\">\n" +
        "          &nbsp;\n" +
        "        </td-->\n" +
        "        <td valign=\"top\">\n" +
        "          <div style=\"color:#2d3136;font-size:16px;line-height:20px\">{4}</div>\n" +
        "          <div style=\"color:#737f8d;font-size:15px;line-height:17px;margin-top:3px\">\n" +
        "             <!-- таблица результата-->\n" +
        "             <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:13px;width:100%;background:#ffffff\" align=\"center\" border=\"0\">\n" +
        "             \t<tbody>\n" +
        "             \t\t<tr style=\"color: #0083BE;font-size: 16px; font-weight: bold;\">\n" +
        "             \t\t\t<td>\n" +
        "             \t\t\t\tКатегория\n" +
        "             \t\t\t</td>\n" +
        "             \t\t\t<td>\n" +
        "             \t\t\t\tВсего\n" +
        "             \t\t\t</td>\n" +
        "             \t\t\t<td>\n" +
        "             \t\t\t\tРезультат\n" +
        "             \t\t\t</td>\n" +
        "             \t\t</tr>\n" +
        "             \t\t{5}\n" +
        "             \t</tbody>\n" +
        "             </table>\n" +
        "             {5}\n" +
        "          </div>\n" +
        "        </td>\n" +
        "      </tr>\n" +
        "    </tbody></table></td></tr></tbody></table></div></td></tr></tbody></table></div>\n" +
        "      <div style=\"margin:0px auto;max-width:640px;background:#ffffff\"><table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:0px;width:100%;background:#ffffff\" align=\"center\" border=\"0\"><tbody><tr><td style=\"text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 40px 0px\"></td></tr></tbody></table></div>\n" +
        "      </div>", model.UserName, "Вы только что прошли тест", "https://cdn2.iconfinder.com/data/icons/social-buttons-2/512/mail-512.png", model.PatternName, model.Mark,FillStatTable(model.StatCategories));
        }

        private string FillStatTable(List<StatCategoryViewModel> list)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var elem in list)
            {
                builder.AppendFormat("<tr>/n<td>/n{0}/n</td>/n<td>/n{1}/n</td>/n<td>/n{2}/n</td></tr>", elem.CategoryName, elem.Total, elem.Right);
            }
            return builder.ToString();
        }

        public async Task<List<PatternViewModel>> GetUserList(string id)
        {
            int groupId = (await context.Users.FirstOrDefaultAsync(rec => rec.Id == id)).UserGroupId ?? -1;
            return await GetGroupList(groupId);
        }

        public async Task<List<PatternViewModel>> GetGroupList(int id)
        {
            if (id != -1)
            {
                return await context.Patterns.Where(rec => rec.UserGroupId == id)
                    .Where(rec=>!rec.PatternCategories.Select(r=>r.Category).Any(r=>!r.Active)).Select(rec => new PatternViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    UserGroupId = rec.UserGroupId.Value,
                    UserGroupName = rec.UserGroup.Name
                }).ToListAsync();
            }
            else
            {
                //не знаю будет ли работать
                return await context.Patterns.Where(rec => !rec.UserGroupId.HasValue).Select(rec => new PatternViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    UserGroupId = rec.UserGroupId.Value,
                    UserGroupName = rec.UserGroup.Name
                }).ToListAsync();
            }
        }
    }
}
