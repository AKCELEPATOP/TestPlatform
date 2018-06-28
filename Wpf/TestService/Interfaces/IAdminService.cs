using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetList();

        Task<UserViewModel> Get(string id);

        Task UpdElement(UserBindingModel model);

        Task DelElement(string id);
    }
}
