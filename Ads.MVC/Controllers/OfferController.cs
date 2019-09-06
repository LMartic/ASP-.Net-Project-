using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ads.MVC.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class OfferController : Controller
    {
        private readonly IGetOfferByIdCommand _getOfferByAdIdCommand;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICreateOfferCommand _createOfferCommand;
        private readonly IGetOfferCommand _getAllUserOffers;
        private readonly IDeleteOfferCommand _deleteOfferCommand;

        public OfferController(IGetOfferByIdCommand getOfferByAdIdCommand,
            UserManager<ApplicationUser> userManager,
            ICreateOfferCommand createOfferCommand,
            IGetOfferCommand getAllUserOffers,
            IDeleteOfferCommand deleteOfferCommand)
        {
            _getOfferByAdIdCommand = getOfferByAdIdCommand;
            _userManager = userManager;
            _createOfferCommand = createOfferCommand;
            _getAllUserOffers = getAllUserOffers;
            _deleteOfferCommand = deleteOfferCommand;
        }

        [HttpGet("{id}")]
        public ActionResult Index(int id)
        {
            ViewData["AdId"] = id;
            var response = _getOfferByAdIdCommand.Execute(new OfferSearch()
            {
                AdId = id,
                UserId = GetUserId().Result
            });
            return View(response);
        }


        [HttpPost]
        public ActionResult Create(CreateOfferDto offer)
        {
            try
            {
                offer.AdId = offer.AdId;
                offer.UserId = GetUserId().Result;
                _createOfferCommand.Execute(offer);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var response = _getAllUserOffers.Execute(new OfferSearch()
            {
                UserId = GetUserId().Result

            });

            var currentOffer = response.SingleOrDefault(w => w.Id == id);
            return View(currentOffer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, DeleteAdDto model)
        {
            model.Id = id;
            model.UserId = GetUserId().Result;
            _deleteOfferCommand.Execute(new DeleteOfferDto()
            {
                Id = model.Id,
                UserId = model.UserId
            });
            return RedirectToAction("Index", "Home");
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }
    }
}