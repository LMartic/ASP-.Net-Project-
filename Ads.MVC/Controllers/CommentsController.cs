using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.Domain;
using Ads.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ads.MVC.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class CommentsController : Controller
    {
        private readonly IGetAdCommentsCommand _getAdCommentsCommand;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetCommentCommand _getCommentCommand;
        private readonly IEditCommentCommand _editCommentCommand;
        private readonly ICreateCommentCommand _createCommentCommand;
        private readonly IDeleteAdCommand _deleteAdCommand;

        public CommentsController(IGetAdCommentsCommand getAdCommentsCommand,
            UserManager<ApplicationUser> userManager,
            IGetCommentCommand getCommentCommand,
            IEditCommentCommand editCommentCommand,
            ICreateCommentCommand createCommentCommand)
        {
            _getAdCommentsCommand = getAdCommentsCommand;
            _userManager = userManager;
            _getCommentCommand = getCommentCommand;
            _editCommentCommand = editCommentCommand;
            _createCommentCommand = createCommentCommand;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            ViewData["AdId"] = id;
            var response = _getAdCommentsCommand.Execute(id);
            return View(response);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var response = _getCommentCommand.Execute(id);
            return View(response);
        }

        [HttpPost]
        public ActionResult Edit(int id, Comment comment)
        {
            _editCommentCommand.Execute(new EditCommentDto()
            {
                Id = id,
                Comment = comment.Comments,
                UserId = GetUserId().Result
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int id, CreateCommentViewModel model)
        {
            _createCommentCommand.Execute(new CreateCommandDto()
            {
                AdId = id,
                Comment = model.Comment,
                UserId = GetUserId().Result
            });
            return RedirectToAction("Index", "Comments", new { id = id });
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }
    }
}