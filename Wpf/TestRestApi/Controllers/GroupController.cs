using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestService;
using TestService.BindingModels;
using TestService.Implementations;
using TestService.Interfaces;

namespace TestRestApi.Controllers
{
    //[Authorize(Roles = ApplicationRoles.SuperAdmin + "," + ApplicationRoles.Admin)]
    public class GroupController : ApiController
    {
        #region global
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

        private IGroup _service;

        public IGroup Service
        {
            get
            {
                return _service ?? GroupService.Create(Context);
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

        [HttpPost]
        public async Task AddElement(GroupBindingModel model)
        {
            await Service.AddElement(model);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var element = await Service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public async Task UpdElement(GroupBindingModel model)
        {
            await Service.UpdElement(model);
        }

        [HttpPost]
        public async Task DelElement(int id)
        {
            await Service.DelElement(id);
        }
    }
}
