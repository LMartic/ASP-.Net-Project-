using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.EfDataAccess;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetFollowersCommand : IGetFollowersCommand
    {
        private readonly AdsContext _context;

        public EfGetFollowersCommand(AdsContext context)
        {
            _context = context;
        }

        public IEnumerable<FollowerListDto> Execute(FollowerSearch request)
        {
            var followedByMe = _context.Followers
                .Include(x => x.Ad)
                .Where(w => w.UserId == request.UserId)
                .Select(s => new FollowerListDto()
                {
                    AdId = s.AdId,
                    AdSubject = s.Ad.Subject
                })
                .ToList();

            return followedByMe;
        }
    }
}