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
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;

        }

        public static CategoryService Create(ApplicationDbContext context)
        {
            return new CategoryService(context);
        }

        public async Task AddElement(CategoryBindingModel model)
        {
            Category element = await context.Categories.FirstOrDefaultAsync(rec => rec.Name == model.Name);
            if (element != null)
            {
                throw new Exception("Категория с таким названием уже существует");
            }

            context.Categories.Add(new Category
            {
                Name = model.Name
            });

            await context.SaveChangesAsync();
        }

        /*     public async Task AddQuestions(QuestionViewModel model)
             {

             }
             */
        public async Task DelElement(int id)
        {
            Category element = await context.Categories.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                context.Categories.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<CategoryViewModel> GetElement(int id)
        {
            Category result = await context.Categories.FirstOrDefaultAsync(rec => rec.Id == id);
            {
                if (result == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    CategoryViewModel element = new CategoryViewModel
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Questions = result.Questions.Select(recQ => new QuestionViewModel
                        {
                            Id = recQ.Id,
                            Active = recQ.Active,
                            Answers = recQ.Answers.Select(recA => new AnswerViewModel
                            {
                                Text=recA.Text
                            }).ToList(),
                            Complexity = recQ.Complexity,
                            Text = recQ.Text
                        }).ToList()
                    };
                    return element;
                }


            }
        }



        public async Task<List<CategoryViewModel>> GetList()
        {
            List<CategoryViewModel> result = await context.Categories.Select(rec => new CategoryViewModel
            {
                Id = rec.Id,
                Name = rec.Name
            }).ToListAsync();
            return result;
        }

        public async Task<List<QuestionViewModel>> GetListQuestions(int id)
        {
            return await context.Categories.Where(rec => rec.Id == id).SelectMany(rec => rec.Questions).Select(rec => new QuestionViewModel
            {
                Id = rec.Id,
                Text = rec.Text,
                CategoryId = rec.CategoryId,
                Time = rec.Time,
                Active = rec.Active,
                Complexity = rec.Complexity,
                ComplexityName = rec.Complexity.ToString(),
            }).ToListAsync();
        }

        public async Task UpdElement(CategoryBindingModel model)
        {
            Category element = await context.Categories.FirstOrDefaultAsync(rec =>
                                   rec.Name == model.Name && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть категория с таким названием");
            }
            element = context.Categories.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Name = model.Name;
            context.SaveChanges();
        }
    }
}
