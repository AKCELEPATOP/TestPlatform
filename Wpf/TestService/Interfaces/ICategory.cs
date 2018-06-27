using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface ICategory
    {
        Task<List<CategoryViewModel>> GetList();

        Task AddElement(CategoryBindingModel model);

        Task UpdElement(CategoryBindingModel model);

        Task DelElement(int id);

       // Task AddQuestions(QuestionViewModel model);

        Task<List<QuestionViewModel>> GetListQuestions(int id);


    }
}
