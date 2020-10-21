using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace IdentityServerDemo.Oauth
{
    public static class SeedIdentity
    {
        public static void SeedIdentityServerData(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var configDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!configDbContext.IdentityResources.Any())
            {
                foreach (var r in ServerConfiguration.IdentityResources)
                {
                    configDbContext.IdentityResources.Add(r.ToEntity());
                }
                configDbContext.SaveChanges();
            }

            if (!configDbContext.ApiResources.Any())
            {
                foreach (var r in ServerConfiguration.ApiResources)
                {
                    configDbContext.ApiResources.Add(r.ToEntity());
                }
                configDbContext.SaveChanges();
            }

            if (!configDbContext.ApiScopes.Any())
            {
                foreach (var s in ServerConfiguration.ApiScopes)
                {
                    configDbContext.ApiScopes.Add(s.ToEntity());
                }
                configDbContext.SaveChanges();
            }

            if (!configDbContext.Clients.Any())
            {
                foreach (var c in ServerConfiguration.Clients)
                {
                    configDbContext.Clients.Add(c.ToEntity());
                }
                configDbContext.SaveChanges();
            }
        }
    }
}
