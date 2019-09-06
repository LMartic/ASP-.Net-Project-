using Ads.Application.Commands;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetAdByIdCommand : IGetAddByIdCommand
    {
        private readonly AdsContext _context;

        public EfGetAdByIdCommand(AdsContext context)
        {
            _context = context;
        }
        public Ad Execute(int request)
        {
            if (!_context.Ads.Any(x => x.Id == request))
                throw new EntityNotFoundException("Ad");

            return _context.Ads.SingleOrDefault(s => s.Id == request);
        }
    }
}