using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModels;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> Get(string id);

        Task AddElement(UserBindingModel model);

        Task UpdElement(UserBindingModel model);

        Task DelElement(string id);

        Task<List<UserViewModel>> GetList();

        Task SetGroup(string userId, int groupId);

    }
}
