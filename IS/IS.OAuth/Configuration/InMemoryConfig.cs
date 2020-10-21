using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IS.OAuth.Configuration
{
    public static class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
          new List<IdentityResource>
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
              new IdentityResource("position", "Your position", new List<string> { "position" }),
              new IdentityResource("country", "Your country", new List<string> { "country" })

          };

        public static List<TestUser> GetUsers() =>
          new List<TestUser>
          {
              new TestUser
              {
                  SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                  Username = "Shweta",
                  Password = "Password@1",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Shweta"),
                      new Claim("family_name", "Naik"),
                      new Claim("address", "Bangalore 1"),
                      new Claim("role", "Admin"),
                      new Claim("position", "Administrator"),
                      new Claim("country", "India")
                  }
              },
              new TestUser
              {
                  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                  Username = "Amit",
                  Password = "Password@1",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Amit"),
                      new Claim("family_name", "Naik"),
                      new Claim("address", "Bangalore 2"),
                      new Claim("role", "Visitor"),
                      new Claim("position", "Viewer"),
                      new Claim("country", "India")
                  }
              }
          };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
               new Client
               {
                    ClientId = "company-employee",
                    ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
                },
                new Client
                {
                    ClientName = "MVC Client",
                    ClientId = "mvc-client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                    RequirePkce = false,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "companyApi",
                        "position",
                        "country"
                    },
                    ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                    PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" },
                    RequireConsent = true
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> { new ApiScope("companyApi", "CompanyEmployee API") };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("companyApi", "CompanyEmployee API")
                {
                    Scopes = { "companyApi" }
                }
            };
    }
}
