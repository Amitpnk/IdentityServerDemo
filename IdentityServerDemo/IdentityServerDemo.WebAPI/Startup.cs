using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServerDemo.WebAPI
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
            services.AddControllers();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication("Bearer")
                    .AddJwtBearer(o =>
                    {
                        o.Authority = "https://localhost:44357";
                        o.RequireHttpsMetadata = false;
                        o.Audience = "employeesWebApiResource";
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
