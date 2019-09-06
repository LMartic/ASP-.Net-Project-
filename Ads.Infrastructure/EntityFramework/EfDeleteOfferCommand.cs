using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfDeleteOfferCommand : IDeleteOfferCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AdsContext _context;

        public EfDeleteOfferCommand(UserManager<ApplicationUser> userManager, AdsContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public void Execute(DeleteOfferDto request)
        {
            var offer = _context.Offers
                .SingleOrDefault(w => w.AdId == request.Id && w.UserId == request.UserId);
            var user = _userManager.FindByIdAsync(request.UserId);
            DeleteOffer(offer);

            _context.SaveChanges();
        }

        private void DeleteOffer(Offer offer)
        {
            _context.Offers.Remove(offer);
        }
    }
}