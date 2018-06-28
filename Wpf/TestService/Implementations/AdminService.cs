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
        private ApplicationDbContext context;

        public AdminService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static AdminService Create(ApplicationDbContext context)
        {
            return new AdminService(context);
        }

        public async Task DelElement(string id)
        {
            User element = await context.Users.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!element.Roles.Select(rec => rec.RoleId).Contains(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Администратором");
            }
            context.Users.Remove(element);
            await context.SaveChangesAsync();
        }

        public async Task<UserViewModel> Get(string id)
        {
            User element = await context.Users.Include(rec => rec.Group).FirstOrDefaultAsync(rec => rec.Id == id);

            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (!element.Roles.Select(rec => rec.RoleId).Contains(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Администратором");
            }
            return new UserViewModel
            {
                Id = element.Id,
                FIO = element.FIO,
                UserName = element.UserName,
                GroupName = element.Group.Name
            };
        }

        public async Task<List<UserViewModel>> GetList()
        {
            List<UserViewModel> result = await context.Users.Where(rec => rec.Roles.Select(r => r.RoleId).Contains(ApplicationRoles.Admin))
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,
                    GroupName = rec.Group.Name,
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
            if (!userOld.Roles.Select(rec => rec.RoleId).Contains(ApplicationRoles.Admin))
            {
                throw new Exception("Элемент не является Пользователем");
            }
            userOld.FIO = model.FIO;
            userOld.UserName = model.UserName;
            userOld.GroupId = model.GroupId;
            await context.SaveChangesAsync();
        }
    }
}
