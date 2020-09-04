# Identity Server 
 
IdentityServer is an implementation of OpenID Connect and OAuth2 specifications that ASP.NET Core developers can use to secure their applications and APIs


Identity server
https://localhost:44357/

API 
https://localhost:44326/

Mvc client
https://localhost:44363/

Redirect to [IdentityServerDemo.Oauth](https://github.com/Amitpnk/IdentityServerDemo/tree/master/IdentityServerDemo/IdentityServerDemo.Oauth) folder and run below command

```sh
dotnet ef migrations add OpMigration -c PersistedGrantDbContext -o Migrations/OpDb
dotnet ef migrations add ConfigMigration -c ConfigurationDbContext -o Migrations/ConfigDb

dotnet ef database update --context PersistedGrantDbContext
dotnet ef database update --context ConfigurationDbContext
```