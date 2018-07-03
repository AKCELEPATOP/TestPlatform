using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestModels;
using TestService;
using TestService.BindingModels;
using TestService.Implementations;
using TestService.Interfaces;

namespace TestRestApi.Controllers
{
    [Authorize(Roles = ApplicationRoles.SuperAdmin)]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        #region global

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext _context;

        public ApplicationDbContext Context
        {
            get
            {
                return _context ?? HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _context = value;
            }
        }

        private IAdminService _service;

        public IAdminService Service
        {
            get
            {
                return _service ?? AdminService.Create(Context, UserManager);
            }
            private set
            {
                _service = value;
            }
        }
        #endregion

        [HttpGet]
        public async Task<IHttpActionResult> GetList()
        {
            var list = await Service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            var element = await Service.Get(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public async Task UpdElement(UserBindingModel model)
        {
            await Service.UpdElement(model);
        }

        [HttpPost]
        public async Task DelElement(string id)
        {
            await Service.DelElement(id);
        }

        [HttpPost]
        public async Task SetAdmin(string id)
        {
            if (!await UserManager.IsInRoleAsync(id, ApplicationRoles.User))
            {
                throw new Exception("Пользователь уже является администратором");
            }
            using(var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    await UserManager.RemoveFromRoleAsync(id, ApplicationRoles.User);
                    await UserManager.AddToRoleAsync(id, ApplicationRoles.Admin);
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }       
        }

        [HttpPost]
        public async Task SetUser(string id)
        {
            if (!await UserManager.IsInRoleAsync(id, ApplicationRoles.Admin))
            {
                throw new Exception("Пользователь не является администратором");
            }
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    await UserManager.AddToRoleAsync(id, ApplicationRoles.User);
                    await UserManager.RemoveFromRoleAsync(id, ApplicationRoles.Admin);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }
    }
}
