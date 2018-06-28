using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestService.Interfaces
{
    public interface IPatternService
    {
        Task Add(PatternBindingModel model);

        Task Upd(PatternBindingModel model);

        Task Del(int id);

        Task<PatternViewModel> Get(int id);

        Task<List<PatternViewModel>> GetList();

        Task<TestViewModel> CreateTest(int patternId);

        Task<StatViewModel> CheakTest(TestResponseModel model);
    }
}
