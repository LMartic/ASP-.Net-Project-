using Ads.Application.Commands;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetCommentCommand : IGetCommentCommand
    {
        private readonly AdsContext _context;

        public EfGetCommentCommand(AdsContext context)
        {
            _context = context;
        }

        public Comment Execute(int request)
        {
            return _context.Comments.SingleOrDefault(w => w.Id == request);
        }
    }
}