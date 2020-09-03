using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerDemo.Oauth
{
    public static  class SeedIdentity
    {
        public static void SeedIdentityServerData(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var configDbCtx = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!configDbCtx.IdentityResources.Any())
            {
                foreach (var r in ServerConfiguration.IdentityResources)
                {
                    configDbCtx.IdentityResources.Add(r.ToEntity());
                }
                configDbCtx.SaveChanges();
            }

            if (!configDbCtx.ApiResources.Any())
            {
                foreach (var r in ServerConfiguration.ApiResources)
                {
                    configDbCtx.ApiResources.Add(r.ToEntity());
                }
                configDbCtx.SaveChanges();
            }

            if (!configDbCtx.ApiScopes.Any())
            {
                foreach (var s in ServerConfiguration.ApiScopes)
                {
                    configDbCtx.ApiScopes.Add(s.ToEntity());
                }
                configDbCtx.SaveChanges();
            }

            if (!configDbCtx.Clients.Any())
            {
                foreach (var c in ServerConfiguration.Clients)
                {
                    configDbCtx.Clients.Add(c.ToEntity());
                }
                configDbCtx.SaveChanges();
            }
        }
    }
}
