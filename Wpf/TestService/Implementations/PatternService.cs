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
                rec.Complexity == QuestionComplexity.Difficult).OrderBy(a => Guid.NewGuid())
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
                rec.Complexity == QuestionComplexity.Middle).OrderBy(a => Guid.NewGuid())
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
                rec.Complexity == QuestionComplexity.Easy).OrderBy(a => Guid.NewGuid())
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


            var questionCount = context.Patterns.FirstOrDefault(rec => rec.Id == model.PatternId)
                .PatternCategories.Select(rec => rec.Count).DefaultIfEmpty(0).Sum();
            var pattern = await context.Patterns.FirstOrDefaultAsync(rec => rec.Id == model.PatternId);
            foreach (var category in pattern.PatternCategories)
            {
                result.StatCategories.Add(new StatCategoryViewModel
                {
                    CategoryId = category.CategoryId
                });
            }
            if (model.QuestionResponses.Count != questionCount)
            {
                throw new Exception("Не совпадает количество вопросов");
            }
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
                    }).ToList()
                }).FirstOrDefaultAsync();

                var list = result.StatCategories.Find(rec => rec.CategoryId == question.CategoryId);
                if (list == null)
                {
                    throw new Exception("В шаблоне отсутствует данная категория");
                }
                list.Questions.Add(new StatQuestionViewModel
                {
                    Text = question.Text,
                    Right = CheakAnswers(question.Answers, questionResponce.Answers)
                });
            }

            result.Total = questionCount;
            result.Right = result.StatCategories.SelectMany(rec => rec.Questions).Where(rec => rec.Right).Count();
            context.Stats.Add(new Stat
            {
                PatternId = model.PatternId,
                DateCreate = DateTime.Now,
                UserId = model.UserId,Right = result.Right,
                Total = result.Total
            });
            await context.SaveChangesAsync();
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

        public async Task<List<PatternViewModel>> GetUserList(string id)
        {
            int groupId = (await context.Users.FirstOrDefaultAsync(rec => rec.Id == id)).UserGroupId ?? -1;
            return await GetGroupList(groupId);
        }

        public async Task<List<PatternViewModel>> GetGroupList(int id)
        {
            if (id != -1)
            {
                return await context.Patterns.Where(rec => rec.UserGroupId == id).Select(rec => new PatternViewModel
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
                return await context.Patterns.Where(rec => rec.UserGroup == null).Select(rec => new PatternViewModel
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
