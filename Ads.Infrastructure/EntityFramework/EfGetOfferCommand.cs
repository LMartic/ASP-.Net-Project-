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
    public class EfGetOfferCommand : IGetOfferCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfGetOfferCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IEnumerable<OfferListDto> Execute(OfferSearch request)
        {
            var offers = _context
                .Offers
                .Include(x => x.Ad)
                .ThenInclude(x => x.User)
                .Where(w => w.UserId == request.UserId)
                .Select(s => new OfferListDto()
                {
                    AdSubject = s.Ad.Subject,
                    Id = s.Id,
                    Amount = s.Amount,
                    Email = s.User.Email
                }).ToList();
            return offers;
        }
    }
}