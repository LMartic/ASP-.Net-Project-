using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfCreateCommentCommand : ICreateCommentCommand
    {
        private readonly AdsContext _context;

        public EfCreateCommentCommand(AdsContext context)
        {
            _context = context;
        }
        public void Execute(CreateCommandDto request)
        {
            if (!_context.Ads.Any(x => x.Id == request.AdId))
                throw new EntityNotFoundException("Ad");

            _context.Comments.Add(new Comment()
            {
                AdId = request.AdId,
                Comments = request.Comment,
                UserId = request.UserId
            });
            _context.SaveChanges();

        }
    }
}