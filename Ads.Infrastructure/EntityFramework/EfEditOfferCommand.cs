using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfEditOfferCommand : IEditOfferCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfEditOfferCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Execute(EditOfferDto request)
        {
            var offer = _context.Offers
                .SingleOrDefault(w => w.Id == request.Id);
            var user = _userManager.FindByIdAsync(request.UserId).Result;
            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
            if (offer != null)
            {
                if (offer.UserId != request.UserId && !isAdmin)
                {
                    throw new ApplicationException("zabranjen pristup");
                }
                else
                {
                    offer.Amount = request.Amount;
                    _context.SaveChanges();
                }
            }
        }
    }
}