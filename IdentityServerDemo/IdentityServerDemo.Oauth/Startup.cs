using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace IdentityServerDemo.Oauth
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

            const string connectionString = @"Data Source=amit-pc\\sqlexpress;Initial Catalog=IdentityDb;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddControllersWithViews();


            string connStr = Configuration.GetConnectionString("AppDb");

            services.AddIdentityServer()
                    //.AddConfigurationStore(options =>
                    //{
                    //    options.ConfigureDbContext = builder =>
                    //        builder.UseSqlServer(connectionString,
                    //            sql => sql.MigrationsAssembly(migrationsAssembly));
                    //})

                    
                    .AddTestUsers(ServerConfiguration.TestUsers)
                    .AddDeveloperSigningCredential()
                    .AddConfigurationStore(o =>
                    {
                        o.ConfigureDbContext = builder => builder.UseSqlServer(connStr, b => b.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddOperationalStore(o =>
                    {
                        o.ConfigureDbContext = builder => builder.UseSqlServer(connStr, b => b.MigrationsAssembly(migrationsAssembly));
                    });

            //.AddInMemoryApiResources(ServerConfiguration.ApiResources)
            //.AddInMemoryApiScopes(ServerConfiguration.ApiScopes)
            //.AddInMemoryClients(ServerConfiguration.Clients)
            //.AddInMemoryIdentityResources(ServerConfiguration.IdentityResources)
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

            SeedIdentityServerData(app);

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SeedIdentityServerData(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!context.IdentityResources.Any())
            {
                foreach (var r in ServerConfiguration.IdentityResources)
                {
                    context.IdentityResources.Add(r.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var r in ServerConfiguration.ApiResources)
                {
                    context.ApiResources.Add(r.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var s in ServerConfiguration.ApiScopes)
                {
                    context.ApiScopes.Add(s.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                foreach (var c in ServerConfiguration.Clients)
                {
                    context.Clients.Add(c.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
