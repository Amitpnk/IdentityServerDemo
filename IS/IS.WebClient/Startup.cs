using IS.WebClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IS.WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICompanyHttpClient, CompanyHttpClient>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", opt =>
                 {
                     opt.SignInScheme = "Cookies";
                     opt.Authority = "https://localhost:5005";
                     opt.ClientId = "mvc-client";
                     opt.ResponseType = "code id_token";
                     opt.SaveTokens = true;
                     opt.ClientSecret = "MVCSecret";
                     opt.GetClaimsFromUserInfoEndpoint = true;

                     opt.ClaimActions.DeleteClaim("sid");
                     opt.ClaimActions.DeleteClaim("idp");

                     opt.Scope.Add("address");
                     opt.Scope.Add("roles");
                     opt.ClaimActions.MapUniqueJsonKey("role", "role");

                     opt.TokenValidationParameters = new TokenValidationParameters
                     {
                         RoleClaimType = "role"
                     };

                     opt.Scope.Add("companyApi");

                     opt.Scope.Add("position");
                     opt.Scope.Add("country");
                     opt.ClaimActions.MapUniqueJsonKey("position", "position");
                     opt.ClaimActions.MapUniqueJsonKey("country", "country");
                 });

            services.AddAuthorization(authOpt =>
            {
                authOpt.AddPolicy("CanCreateAndModifyData", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireClaim("position", "Administrator");
                    policyBuilder.RequireClaim("country", "USA");
                });
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
