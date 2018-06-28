using Microsoft.AspNet.Identity;
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

        private UserManager<User> userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
        }

        public static UserService Create(ApplicationDbContext context, UserManager<User> userManager)
        {
            return new UserService(context, userManager);
        }

        public async Task AddElement(UserBindingModel model, UserManager<User> manager)
        {
            User user = new User
            {
                FIO = model.FIO,
                UserGroupId = model.GroupId,
                UserName = model.UserName,
                PasswordHash = model.PasswordHash
            };
            await manager.CreateAsync(user, user.PasswordHash);
        }

        public async Task DelElement(string id)
        {
            User element = await context.Users.FirstOrDefaultAsync(rec => rec.Id == id);
            if(element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!userManager.GetRoles(element.Id).FirstOrDefault().Equals(ApplicationRoles.User))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            context.Users.Remove(element);
            await context.SaveChangesAsync();
        }

        public async Task<UserViewModel> Get(string id)
        {
            User element = await context.Users.Include(rec=>rec.UserGroup).FirstOrDefaultAsync(rec => rec.Id == id);

            if (element != null)
            {
                return new UserViewModel
                {
                    Id = element.Id,
                    FIO = element.FIO,
                    UserName = element.UserName,
                    GroupName = element.UserGroup.Name
                };
            }
            throw new Exception("Элемент не найден");
        }

        public async Task UpdElement(UserBindingModel model)
        {
            var userOld = await context.Users.FirstOrDefaultAsync(rec => rec.Id == model.Id);
            if (userOld == null)
            {
                throw new Exception("Нет данных");
            }
            if (!userManager.GetRoles(userOld.Id).FirstOrDefault().Equals(ApplicationRoles.User))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            userOld.FIO = model.FIO;
            userOld.UserName = model.UserName;
            userOld.UserGroupId = model.GroupId;
            await context.SaveChangesAsync();
        }

        public async Task<List<UserViewModel>> GetList()
        {
            return await context.Users.Where(rec => userManager.GetRoles(rec.Id).FirstOrDefault().Equals(ApplicationRoles.User)).Include(r => r.UserGroup)
                .Select(rec => new UserViewModel
            {
                Id = rec.Id,
                FIO = rec.FIO,
                GroupName = rec.UserGroup.Name,
                UserName = rec.UserName
            }).ToListAsync();
        }

        public async Task SetGroup(string userId, int groupId)
        {
            User user = await context.Users.FirstOrDefaultAsync(rec => rec.Id == userId);
            if(user == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!userManager.GetRoles(user.Id).FirstOrDefault().Equals(ApplicationRoles.User))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            user.UserGroupId = groupId;
            await context.SaveChangesAsync();
        }
    }
}
