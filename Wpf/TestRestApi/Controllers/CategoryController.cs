using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TestService.Interfaces;
using TestService.BindingModels;

namespace TestRestApi.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly ICategory _service;

        public CategoryController(ICategory service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(CategoryBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(CategoryBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(CategoryBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}