using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tsl.IdentityServer4.Data;
using Tsl.IdentityServer4.Models;
using Tsl.IdentityServer4.Services;

namespace Tsl.IdentityServer4
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddMicrosoftAccount(options =>
                {
                    options.ClientId = "32598a15-76ae-46af-a476-03b77791e974";
                    options.ClientSecret = "dprDXS1606[irfkWMSK6(+!";
                    options.Events.OnCreatingTicket += OnMicrosoftCreatingTicket;
                }
            );



            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();


        }

        private async Task OnMicrosoftCreatingTicket(OAuthCreatingTicketContext ctx)
        {

            var email =  ctx.Identity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.First().Value;
            var id = ctx.Identity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                ?.First().Value;


            var msAccountLinker = new MsAccountHandler(null);
            msAccountLinker.VerifySignIn(email, id);


            //throw new NotImplementedException();
        }

        //private Task OnRedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> redirectContext)
        //{
        //    //throw new NotImplementedException();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class MsAccountHandler
    {
        private IAuthRepo _repo;

        public MsAccountHandler(IAuthRepo repo)
        {
            _repo = repo;
        }


        public void VerifySignIn(string email, string id)
        {
            //Look to see if we already have this email registiered.

            //We do, make sure there's a linked account
                //No linked account, make one

            //No email registered, setup account, and link
        }
    }

    public class Repo : IAuthRepo
    {
        private ApplicationDbContext _ctx;

        public Repo(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }


        //public void LinkExistingAccount()

        public void CreateLinkedAccount()
        {
            //var token = new AspNetUserTokens<>();
            //token.LoginProvider = "Microsoft";
            //token.UserId = 
            //_ctx.UserTokens.Add(new IdentityUserToken<string>())
        }
    }

    public interface IAuthRepo
    {


    }
}
