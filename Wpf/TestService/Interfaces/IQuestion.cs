using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public  interface IQuestion
    {
        Task<List<QuestionViewModel>> GetList();

        Task<QuestionViewModel> GetElement(int id);

        Task AddElement(QuestionBindingModel model);

        Task UpdElement(QuestionBindingModel model);

        Task DelElement(int id);

    }
}
