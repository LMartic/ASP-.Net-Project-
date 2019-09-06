using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfCreateOfferCommand : ICreateOfferCommand
    {
        private readonly AdsContext _context;

        public EfCreateOfferCommand(AdsContext context)
        {
            _context = context;
        }
        public void Execute(CreateOfferDto request)
        {
            if (_context.Offers.Any(x => x.UserId == request.UserId && x.AdId == request.AdId))
            {
                throw new EntityAlreadyExistsException("Offer");
            }

            _context.Offers
                .Add(new Offer()
                {
                    AdId = request.AdId,
                    UserId = request.UserId,
                    Amount = request.Amount
                });
            _context.SaveChanges();
        }
    }
}