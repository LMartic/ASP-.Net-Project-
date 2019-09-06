﻿using Ads.Application.Commands;
using Ads.Application.Interfaces;
using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Ads.Infrastructure.Auth;
using Ads.Infrastructure.EntityFramework;
using Ads.MVC.Extenisions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ads.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddDbContext<AdsContext>();
            services.AddTransient<ICreateCategoryCommand, EfCreateCategoryCommand>();
            services.AddTransient<ICreateAdCommand, EfCreateAdCommand>();
            services.AddTransient<IGetCategoriesCommand, EfGetCategoriesCommand>();
            services.AddTransient<IGetAdsCommand, EfGetAdsCommands>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IDeleteAdCommand, EfDeleteAdCommand>();
            services.AddTransient<IEditAdCommand, EfEditAdCommand>();
            services.AddTransient<ICreateRemoveFollowerCommand, EfCreateRemoveFollowerCommand>();
            services.AddTransient<IGetFollowersCommand, EfGetFollowersCommand>();
            services.AddTransient<ICreateOfferCommand, EfCreateOfferCommand>();
            services.AddTransient<IDeleteOfferCommand, EfDeleteOfferCommand>();
            services.AddTransient<IEditOfferCommand, EfEditOfferCommand>();
            services.AddTransient<IGetAdOffersCommand, EfGetAdOffersCommand>();
            services.AddTransient<ICreateCommentCommand, EfCreateCommentCommand>();
            services.AddTransient<IEditCommentCommand, EfEditCommentCommand>();
            services.AddTransient<IGetCommentCommand, EfGetCommentCommand>();
            services.AddTransient<IGetAdCommentsCommand, EfGetAdCommentsCommand>();
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            services.AddTransient<IGetOfferByIdCommand, EfGetOfferByIdCommand>();
            services.AddTransient<IGetOfferCommand, EfGetOfferCommand>();
            var section = Configuration.GetSection("Email");

            var sender =
                new SmtpEmailSender(section["host"], Int32.Parse(section["port"]), section["fromaddress"], section["password"]);
            services.AddSingleton<IEmailSender>(sender);

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequiredLength = 4;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<AdsContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
