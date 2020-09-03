using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerDemo.MvcClient
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
            services.AddControllersWithViews();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = "Cookies";
                o.DefaultChallengeScheme = "oidc";
            })

            .AddCookie("Cookies", o =>
            {
                o.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddOpenIdConnect("oidc", o =>
            {
                o.ClientId = "client2";
                o.ClientSecret = "client2_secret_code";
                o.SignInScheme = "Cookies";
                o.Authority = "https://localhost:44357";
                o.RequireHttpsMetadata = false;
                o.ResponseType = "code id_token";
                o.SaveTokens = true;
                o.GetClaimsFromUserInfoEndpoint = true;
                //o.Prompt = "consent";
                o.Scope.Add("employeesWebApi");
                o.Scope.Add("roles");
                o.Scope.Add("email");
                o.Scope.Add("phone");
                o.Scope.Add("address");
                o.ClaimActions.MapUniqueJsonKey("role", "role");
                o.ClaimActions.MapUniqueJsonKey("email", "email");
                o.ClaimActions.MapUniqueJsonKey("phone", "phone");
                o.ClaimActions.MapUniqueJsonKey("address", "address");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role"
                };
            });

            services.AddAuthorization(o => o.AddPolicy("AdminCountryPolicy", p =>
                {
                    p.RequireAssertion(ctx =>
                    {
                        return (ctx.User.IsInRole("Admin")
                                && (ctx.User.HasClaim("address", "USA")
                                || ctx.User.HasClaim("address", "UK")));
                    });
                }
            ));
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
