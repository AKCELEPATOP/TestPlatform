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
    public class GroupService : IGroup
    {
        private readonly ApplicationDbContext context;

        public GroupService(ApplicationDbContext context)
        {
            this.context = context;

        }

        public async Task AddElement(GroupBindingModel model)
        {
            UserGroup element = await context.UserGroups.FirstOrDefaultAsync(rec => rec.Name == model.Name);
            if (element != null)
            {
                throw new Exception("Группа с таким названием уже существует");
            }

            context.UserGroups.Add(new UserGroup
            {
                Name = model.Name
            });

            await context.SaveChangesAsync();
        }


        public async Task DelElement(int id)
        {
            UserGroup element = await context.UserGroups.FirstOrDefaultAsync(rec => rec.Id == id);
            if (element != null)
            {
                context.UserGroups.Remove(element);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public async Task<GroupViewModel> GetElement(int id)
        {
            UserGroup result = await context.UserGroups.FirstOrDefaultAsync(rec => rec.Id == id);
            {
                if (result == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    GroupViewModel element = new GroupViewModel
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Users = result.Users.Select(recQ => new UserViewModel
                        {
                            Id = recQ.Id,
                            FIO=recQ.FIO
                        }).ToList()
                    };
                    return element;
                }


            }
        }

        public async Task<List<GroupViewModel>> GetList()
        {
            List<GroupViewModel> result = await context.UserGroups.Select(rec => new GroupViewModel
            {
                Id = rec.Id,
                Name = rec.Name
            }).ToListAsync();
            return result;
        }

        public async Task UpdElement(GroupBindingModel model)
        {
            UserGroup element = await context.UserGroups.FirstOrDefaultAsync(rec =>
                                   rec.Name == model.Name && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть категория с таким названием");
            }
            element = context.UserGroups.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Name = model.Name;
            context.SaveChanges();
        }
    }
}
