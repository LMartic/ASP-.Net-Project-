using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetAdOffersCommand : IGetAdOffersCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfGetAdOffersCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<AdOffersListDto> Execute(AdOfferSearch request)
        {
            var adOffers = _context.Offers
                .Include(x => x.User)
                .Include(x => x.Ad)
                .Where(w => w.AdId == request.AdId && w.Ad.UserId == request.UserId)
                .Select(s => new AdOffersListDto()
                {
                    Amount = s.Amount,
                    Email = s.User.Email
                }).ToList();
            return adOffers;
        }
    }
}