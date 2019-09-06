using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfEditCommentCommand : IEditCommentCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfEditCommentCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void Execute(EditCommentDto request)
        {
            var comment = _context.Comments.SingleOrDefault(w => w.UserId == request.UserId && w.Id == request.Id);

            if (comment == null)
                throw new EntityNotFoundException("Comment");

            comment.Comments = request.Comment;

            _context.SaveChanges();
        }
    }
}