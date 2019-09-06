using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetAdCommentsCommand : IGetAdCommentsCommand
    {
        private readonly AdsContext _context;

        public EfGetAdCommentsCommand(AdsContext context)
        {
            _context = context;
        }
        public IEnumerable<CommentListDto> Execute(int request)
        {
            var response = _context.Comments
                .Include(x => x.Ad)
                .ThenInclude(x => x.User)
                .Where(w => w.AdId == request)
                .Select(s => new CommentListDto()
                {
                    Comment = s.Comments,
                    Email = s.User.Email,
                    Id = s.Id
                }).ToList();
            return response;
        }
    }
}