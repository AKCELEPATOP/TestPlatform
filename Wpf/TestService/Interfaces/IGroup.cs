using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface IGroup
    {
        Task<List<GroupViewModel>> GetList();

        Task<GroupViewModel> GetElement(int id);

        Task AddElement(GroupBindingModel model);

        Task UpdElement(GroupBindingModel model);

        Task DelElement(int id);
        
    }
}
