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
    public class AdminService : IAdminService
    {
        private static ApplicationDbContext context;

        private UserManager<User> userManager;

        private string roleId;

        private string RoleId
        {
            get
            {
                return roleId ?? GetRoleId();
            }
        }

       /* private Lazy<string> RoleId = new Lazy<string>(() =>
        {
            var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            return 
        });*/


        public AdminService(ApplicationDbContext context, UserManager<User> userManager)
        {
            AdminService.context = context;
            this.userManager = userManager;
        }

        public static AdminService Create(ApplicationDbContext context, UserManager<User> userManager)
        {
            return new AdminService(context, userManager);
        }

        private string GetRoleId()
        {
            var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleId = manager.FindByName(ApplicationRoles.Admin).Id;
            return roleId;
        }

        public async Task DelElement(string id)
        {
            User element = await context.Users.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!userManager.GetRoles(element.Id).FirstOrDefault().Equals(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Администратором");
            }
            context.Users.Remove(element);
            await context.SaveChangesAsync();
        }

        public async Task<UserViewModel> Get(string id)
        {
            User element = await context.Users.Include(rec => rec.UserGroup).FirstOrDefaultAsync(rec => rec.Id == id);

            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!userManager.GetRoles(element.Id).FirstOrDefault().Equals(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Администратором");
            }
            return new UserViewModel
            {
                Id = element.Id,
                FIO = element.FIO,
                UserName = element.UserName,
                GroupName = element.UserGroup.Name
            };
        }

        public async Task<List<UserViewModel>> GetList()
        {
            List<UserViewModel> result = await context.Users.Where(rec => rec.Roles.FirstOrDefault().RoleId.Equals(RoleId))
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,
                    GroupName = rec.UserGroup.Name,
                    UserName = rec.UserName
                })
                .ToListAsync();
            return result;
        }

        public async Task UpdElement(UserBindingModel model)
        {
            var userOld = await context.Users.FirstOrDefaultAsync(rec => rec.Id == model.Id);
            if (userOld == null)
            {
                throw new Exception("Нет данных");
            }
            if (!userManager.GetRoles(userOld.Id).FirstOrDefault().Equals(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            userOld.FIO = model.FIO;
            userOld.UserName = model.UserName;
            userOld.UserGroupId = model.GroupId;
            await context.SaveChangesAsync();
        }
    }
}
