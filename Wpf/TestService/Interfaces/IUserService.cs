using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> Get(string id);

        Task<List<UserViewModel>> GetList();

        Task AddElement(UserBindingModel model);

        Task UpdElement(UserBindingModel model);

        Task DelElement(string id);
    }
}
