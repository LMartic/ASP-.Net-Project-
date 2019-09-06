using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Authentication;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfDeleteAdCommand : IDeleteAdCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfDeleteAdCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void Execute(DeleteAdDto request)
        {
            if (!_context.Ads.Any(x => x.Id == request.Id))
                throw new EntityNotFoundException("Ad");
            var ad = _context.Ads.SingleOrDefault(w => w.Id == request.Id);

            var user = _userManager.FindByIdAsync(request.UserId).Result;

            var role = _userManager.IsInRoleAsync(user, "Admin").Result;

            if (ad.UserId != request.UserId && !role)
                throw new AuthenticationException("Zabranjen pristup");

            _context.Ads.Remove(ad);
            _context.SaveChanges();
        }
    }
}