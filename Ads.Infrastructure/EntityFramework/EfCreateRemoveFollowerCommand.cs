using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfCreateRemoveFollowerCommand : ICreateRemoveFollowerCommand
    {
        private readonly AdsContext _context;

        public EfCreateRemoveFollowerCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        public void Execute(CreateFollowerDto request)
        {
            var existFollower = _context.Followers.SingleOrDefault(x => x.AdId == request.AdId && x.UserId == request.UserId);
            if (existFollower != null)
            {
                _context.Remove(existFollower);
            }
            else
            {
                _context.Add(new Follower()
                {
                    AdId = request.AdId,
                    UserId = request.UserId
                });
            }

            _context.SaveChanges();
        }
    }
}