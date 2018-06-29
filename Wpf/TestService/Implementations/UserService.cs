using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        private static ApplicationDbContext context;

        private UserManager<User> userManager;

        private Lazy<string> roleId = new Lazy<string>(() =>
        {
            var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            return manager.FindByName(ApplicationRoles.User).Id;
        });

        private string RoleId => roleId.Value;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            UserService.context = context;
            this.userManager = userManager;
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
                PasswordHash = model.PasswordHash,
                Email = model.Email
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
            string roleId = RoleId;
            if (!element.Roles.FirstOrDefault().RoleId.Equals(roleId))
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
                string roleId = RoleId;
                if (!element.Roles.FirstOrDefault().RoleId.Equals(roleId))
                {
                    throw new Exception("Элемент не является Пользователем");
                }
                return new UserViewModel
                {
                    Id = element.Id,
                    FIO = element.FIO,
                    UserName = element.UserName,
                    GroupName = element.UserGroup.Name,
                    Email = element.Email
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
            string roleId = RoleId;
            if (!userOld.Roles.FirstOrDefault().RoleId.Equals(roleId))
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
            string roleId = RoleId;
            return await context.Users.Where(rec => rec.Roles.FirstOrDefault().RoleId.Equals(roleId)).Include(r => r.UserGroup)
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
            string roleId = RoleId;
            if (!user.Roles.FirstOrDefault().RoleId.Equals(roleId))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            user.UserGroupId = groupId;
            await context.SaveChangesAsync();
        }
    }
}
