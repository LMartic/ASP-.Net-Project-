using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetOfferByIdCommand : IGetOfferByIdCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfGetOfferByIdCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public OfferDto Execute(OfferSearch search)
        {
            return _context.Offers
                .Include(x => x.Ad)
                .Include(x => x.User)
                .Where(w => w.AdId == search.AdId && w.UserId == search.UserId)
                .Select(s => new OfferDto()
                {
                    AdSubject = s.Ad.Subject,
                    Amount = s.Amount,
                    Id = s.AdId
                }).SingleOrDefault();
        }

    }
}