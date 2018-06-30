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
    [Authorize(Roles = ApplicationRoles.SuperAdmin + "," + ApplicationRoles.Admin)]
    public class QuestionController : ApiController
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

        private IQuestion _service;

        public IQuestion Service
        {
            get
            {
                return _service ?? QuestionService.Create(Context);
            }
            private set
            {
                _service = value;
            }
        }
        #endregion

        [HttpPost]
        public async Task AddElement(QuestionBindingModel model)
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
        public async Task UpdElement(QuestionBindingModel model)
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
