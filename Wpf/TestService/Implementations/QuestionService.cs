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

        public async Task AddElement(QuestionBindingModel model)
        {
            Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Text == model.Text);
            if (element != null)
            {
                throw new Exception("Категория с таким названием уже существует");
            }

            context.Questions.Add(new Question
            {
                Text=model.Text,
                Complexity=model.Complexity,
                Active=model.Active,
                Answers=model.Answers
            });

            await context.SaveChangesAsync();
        }

        public async Task DelElement(int id)
        {
            Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                context.Questions.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<QuestionViewModel> GetElement(int id)
        {
            Question element = await context.Questions.FirstOrDefaultAsync(rec => rec.Id == id);
            {
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
                            Text=recQ.Text
                            
                        }).ToList(),
                        Complexity=element.Complexity.ToString(),
                        CategoryName=element.Category.Name


                    };
                    return result;
                }

            }
        }

        public async Task<List<QuestionViewModel>> GetList()
        {
            List<QuestionViewModel> result = await context.Questions.Select(rec => new QuestionViewModel
            {
                Id = rec.Id,
                Text=rec.Text
            }).ToListAsync();
            return result;
        }


        public async Task UpdElement(QuestionBindingModel model)
        {
            Question element = await context.Questions.FirstOrDefaultAsync(rec =>
                                   rec.Text == model.Text && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть категория с таким названием");
            }
            element = context.Questions.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Text = model.Text;
            element.Complexity = model.Complexity;
            element.Active = model.Active;
            element.Answers = model.Answers;
            context.SaveChanges();
        }
    }
}
