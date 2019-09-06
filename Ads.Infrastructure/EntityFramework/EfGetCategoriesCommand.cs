using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Searches;
using Ads.DataAccess.EfDataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfGetCategoriesCommand : IGetCategoriesCommand
    {
        private readonly AdsContext _context;

        public EfGetCategoriesCommand(AdsContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryListDto> Execute(CategorySearch request)
        {
            var categories = _context.Categories
                .ToList().AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
            {
                categories = categories.Where(w => w.Name == request.Name);
            }

            if (request.Id > 0)
            {
                categories = categories.Where(w => w.Id == request.Id);
            }

            return categories.Select(s => new CategoryListDto()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}