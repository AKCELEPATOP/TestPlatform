using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.Interfaces;
using TestService.ViewModels;

namespace TestService.Implementations
{
    public class StatService : IStatService
    {
        private ApplicationDbContext context;

        public StatService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static StatService Create(ApplicationDbContext context)
        {
            return new StatService(context);
        }

        public async Task<List<StatViewModel>> GetList(GetListModel model)
        {
            return await context.Stats.Skip(model.Skip).Take(model.Take).Include(rec=>rec.Pattern).Select(rec => new StatViewModel
            {
                Total = rec.Total,
                Right = rec.Right,
                PatternName = rec.Pattern.Name
            }).ToListAsync();
        }

        public async Task<StatChartViewModel> GetUserChart(GetListModel model)
        {
            return new StatChartViewModel
            {
                Results = await context.Stats.Where(rec => rec.UserId == model.UserId)
                .Reverse()
                .Take(model.Take)
                .Select(rec => (double)rec.Right / rec.Total)
                .ToListAsync()
            };
        }

        public async Task<List<StatViewModel>> GetUserList(GetListModel model)
        {
            return await context.Stats.Where(rec => rec.UserId == model.UserId).Skip(model.Skip).Take(model.Take).Include(rec=>rec.Pattern).Select(rec => new StatViewModel
            {
                PatternName = rec.Pattern.Name,
                Right = rec.Right,
                Total = rec.Total
            }).ToListAsync();
        }
    }
}
