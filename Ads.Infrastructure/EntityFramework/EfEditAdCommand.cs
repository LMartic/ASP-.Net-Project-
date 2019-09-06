using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Ads.Infrastructure.EntityFramework
{
    public class EfEditAdCommand : IEditAdCommand
    {
        private readonly AdsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfEditAdCommand(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void Execute(EditAdDto request)
        {
            var ad = _context.Ads.SingleOrDefault(w => w.Id == request.Id);
            var user = _userManager.FindByIdAsync(request.UserId).Result;
            var role = _userManager.IsInRoleAsync(user, "Admin").Result;
            if (ad == null)
                throw new EntityNotFoundException("Ad");

            if (ad.UserId != request.UserId && !role)
                throw new ApplicationException("Zabranjen pristup");



            ad.AddedDateTime = DateTime.Now;
            ad.Subject = request.Subject;
            ad.Description = request.Description;

            _context.SaveChanges();

        }
    }
}