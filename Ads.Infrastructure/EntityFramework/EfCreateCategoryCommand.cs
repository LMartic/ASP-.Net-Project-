using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfCreateCategoryCommand : ICreateCategoryCommand
    {
        private readonly AdsContext _context;

        public EfCreateCategoryCommand(AdsContext context)
        {
            _context = context;
        }
        public void Execute(CreateCategoryDto request)
        {

            if (_context.Categories.Any(x => x.Name == request.Name))
            {
                throw new EntityAlreadyExistsException("Category");
            }


            _context.Categories.Add(new Category
            {
                Name = request.Name,
            });


            _context.SaveChanges();

        }
    }
}