using Ads.Application.Commands;
using Ads.Application.Dto;
using Ads.Application.Exceptions;
using Ads.Application.Interfaces;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;


namespace Ads.Infrastructure.EntityFramework
{
    public class EfCreateAdCommand : ICreateAdCommand
    {
        private readonly AdsContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public EfCreateAdCommand(AdsContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public void Execute(CreateAdDto request)
        {
            if (!_context.Categories.Any(x => x.Id == request.CategoryId))
            {
                throw new EntityNotFoundException("Category");
            }

            _context.Ads.Add(new Ad
            {
                AddedDateTime = DateTime.Now,
                Subject = request.Subject,
                Description = request.Description,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            });


            var user = _userManager.FindByIdAsync(request.UserId);
            _context.SaveChanges();

            _emailSender.Subject = "Uspesno kreiran oglas";
            _emailSender.Body = "Uspesno ste kreirali oglas";
            _emailSender.ToEmail = user.Result.Email;
            _emailSender.Send();
        }


    }
}