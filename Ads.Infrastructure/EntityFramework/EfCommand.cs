using Ads.DataAccess.EfDataAccess;

namespace Ads.Infrastructure.EntityFramework
{
    public abstract class EfCommand
    {
        protected readonly AdsContext Context;

        protected EfCommand(AdsContext context)
        {
            Context = context;
        }
    }
}