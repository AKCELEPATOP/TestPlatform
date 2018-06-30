using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TestService.Interfaces;
using TestService.BindingModels;
using TestService;
using Microsoft.AspNet.Identity.Owin;
using TestService.Implementations;
using System.Threading.Tasks;

namespace TestRestApi.Controllers
{
    [Authorize(Roles = ApplicationRoles.SuperAdmin + "," + ApplicationRoles.Admin)]
    public class CategoryController : ApiController
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

        private ICategory _service;

        public ICategory Service
        {
            get
            {
                return _service ?? CategoryService.Create(Context);
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
            var list =await Service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
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
        public async Task AddElement(CategoryBindingModel model)
        {
            await Service.AddElement(model);
        }

        [HttpPost]
        public async Task UpdElement(CategoryBindingModel model)
        {
            await Service.UpdElement(model);
        }

        [HttpPost]
        public async Task DelElement(int id)
        {
            await Service.DelElement(id);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetListQuestions(int id)
        {
            var list = await Service.GetListQuestions(id);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetListQuestions(int id)
        {
            var list = await Service.GetListQuestions(id);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}