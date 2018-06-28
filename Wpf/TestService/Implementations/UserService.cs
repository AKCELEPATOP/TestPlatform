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
    public class UserService : IUserService
    {
        private ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static UserService Create(ApplicationDbContext context)
        {
            return new UserService(context);
        }

        public Task AddElement(UserBindingModel model)
        {
            throw new NotImplementedException();
        }

        public async Task DelElement(string id)
        {
            User element = await context.Users.FirstOrDefaultAsync(rec => rec.Id == id);
            if(element != null)
            {
                context.Users.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<UserViewModel> Get(string id)
        {
            User element = await context.Users.Include(rec=>rec.Group).FirstOrDefaultAsync(rec => rec.Id == id);

            if (element != null)
            {
                return new UserViewModel
                {
                    Id = element.Id,
                    FIO = element.FIO,
                    UserName = element.UserName,
                    GroupName = element.Group.Name
                };
            }
            throw new Exception("Элемент не найден");
        }

        public async Task<List<UserViewModel>> GetList()
        {
            List<UserViewModel> result = await context.Users.Include(r => r.Group).Select(rec => new UserViewModel
            {
                Id = rec.Id,
                FIO = rec.FIO,
                GroupName = rec.Group.Name,
                UserName = rec.UserName
            })
                .ToListAsync();
            return result;
        }

        public Task UpdElement(UserBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
