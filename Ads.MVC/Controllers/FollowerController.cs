using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ads.MVC.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class FollowerController : Controller
    {
        private readonly ICreateRemoveFollowerCommand _createRemoveFollowerCommand;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowerController(ICreateRemoveFollowerCommand createRemoveFollowerCommand, UserManager<ApplicationUser> userManager)
        {
            _createRemoveFollowerCommand = createRemoveFollowerCommand;
            _userManager = userManager;
        }

        [HttpPost]
        public ActionResult Update(int AdId)
        {
            _createRemoveFollowerCommand.Execute(new CreateFollowerDto()
            {
                AdId = AdId,
                UserId = GetUserId().Result
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