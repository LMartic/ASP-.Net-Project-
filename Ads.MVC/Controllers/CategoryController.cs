using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Ads.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IGetCategoriesCommand _listCategoryCommand;
        private readonly ICreateCategoryCommand _createCategoryCommand;

        public CategoryController(IGetCategoriesCommand listCategoryCommand, ICreateCategoryCommand createCategoryCommand)
        {
            _listCategoryCommand = listCategoryCommand;
            _createCategoryCommand = createCategoryCommand;
        }

        [HttpGet]
        public ActionResult Index(CategorySearch model)
        {
            var response = _listCategoryCommand.Execute(model);
            return View(response);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CreateCategoryDto request)
        {
            try
            {
                _createCategoryCommand.Execute(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

    }
}