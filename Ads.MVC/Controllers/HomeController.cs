using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.Domain;
using Ads.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ads.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetAdsCommand _listAdCommand;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetCategoriesCommand _categoryCommand;
        private readonly ICreateAdCommand _createAdCommand;
        private readonly IDeleteAdCommand _deleteAdCommand;
        private readonly IEditAdCommand _editAdCommand;

        public HomeController(IGetAdsCommand listAdCommand,
            UserManager<ApplicationUser> userManager,
            IGetCategoriesCommand categoryCommand,
            ICreateAdCommand createAdCommand, IDeleteAdCommand deleteAdCommand, IEditAdCommand editAdCommand)
        {
            _listAdCommand = listAdCommand;
            _userManager = userManager;
            _categoryCommand = categoryCommand;
            _createAdCommand = createAdCommand;
            _deleteAdCommand = deleteAdCommand;
            _editAdCommand = editAdCommand;
        }

        public IActionResult Index(AdSearch model)
        {
            return View(GetAds(model));
        }

        [HttpGet]
        public ActionResult Add(CategorySearch search)
        {
            ViewData["Categories"] = new SelectList(GetCategories(search), "Id", "Name");
            return View();
        }

        private IEnumerable GetCategories(CategorySearch search)
        {
            return _categoryCommand.Execute(search);
        }

        [HttpPost]
        public ActionResult Add(CreateAdDto request)
        {
            try
            {
                request.UserId = GetUserId().Result;
                _createAdCommand.Execute(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int? id, AdSearch search)
        {
            var ads = _listAdCommand.Execute(search);
            var singleAd = ads.Where(w => w.Id == id).First();
            return View(singleAd);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, DeleteAdDto model)
        {
            model.Id = id;
            model.UserId = GetUserId().Result;
            _deleteAdCommand.Execute(model);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id, AdSearch search)
        {
            var ads = _listAdCommand.Execute(search);
            var editAd = ads.Where(w => w.Id == id).SingleOrDefault();
            return View(new CreateAdDto()
            {
                Id = editAd.Id,
                Subject = editAd.Subject,
                Description = editAd.Description
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, [Bind("Subject", "Description")]CreateAdDto model)
        {

            _editAdCommand.Execute(new EditAdDto()
            {
                Id = id,
                Description = model.Description,
                Subject = model.Subject,
                UserId = GetUserId().Result,
            });
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IEnumerable<AdListDto> GetAds(AdSearch request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.FollowerUserId = GetUserId().Result;
            }
            return _listAdCommand.Execute(request);
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }

    }
}
