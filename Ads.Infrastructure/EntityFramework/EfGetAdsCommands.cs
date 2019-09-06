using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetAdsCommands : IGetAdsCommand
    {
        private readonly AdsContext _context;

        public EfGetAdsCommands(AdsContext context)
        {
            _context = context;
        }
        public IEnumerable<AdListDto> Execute(AdSearch request)
        {
            var ads = _context.Ads
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Followers)
                .Include(x => x.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                ads = ads.Where(x => x.Category.Name == request.CategoryName);
            }

            if (!string.IsNullOrEmpty(request.Subject))
            {
                ads = ads.Where(w => w.Subject.Contains(request.Subject));
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                ads = ads.Where(w => w.Description.Contains(request.Description));
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                ads = ads.Where(w => w.User.Email == request.Email);
            }

            if (request.Id > 0)
            {
                ads = ads.Where(w => w.Id == request.Id);
            }


            return ads.Select(s => new AdListDto()
            {
                Id = s.Id,
                Description = s.Description,
                Subject = s.Subject,
                UserName = s.User.UserName,
                CategoryName = s.Category.Name,
                AddedDateTime = s.AddedDateTime,
                IsFollowed = s.Followers.Any(x => x.UserId == request.FollowerUserId)
            }).ToList();

        }
    }
}