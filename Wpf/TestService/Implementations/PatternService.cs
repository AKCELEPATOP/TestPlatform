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
                    foreach(var groupCategory in groupCategories)
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

                    var groupQuestions = model.PatternQuestions.GroupBy(rec=>rec.QuestionId).Select(g=>g.First()).ToList();

                    foreach(var groupQuestion in groupQuestions)
                    {
                        context.PatternQuestions.Add(new PatternQuestion
                        {
                            PatternId = element.Id,
                            QuestionId = groupQuestion.QuestionId
                        });
                    }
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }catch(Exception)
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

        public Task<PatternViewModel> Get(int id)
        {
            Pattern pattern = context.Patterns.FirstOrDefaultAsync(rec=>rec.Id == )
        }

        public Task<List<PatternViewModel>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task Upd(PatternBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
