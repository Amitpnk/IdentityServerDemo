# Identity Server 
 
IdentityServer is an implementation of OpenID Connect and OAuth2 specifications that ASP.NET Core developers can use to secure their applications and APIs


## Application URLs:

|Project|Launch URL|
|---|---|
|Identity server|https://localhost:5005/|
|API |https://localhost:5001/|
|Mvc client|https://localhost:5010/|


## How to Run:

Navigate to [IdentityServerDemo.Oauth](https://github.com/Amitpnk/IdentityServerDemo/tree/master/IS/IS.OAuth) and  run these commands:


```sh
dotnet ef migrations add OpMigration -c PersistedGrantDbContext -o Migrations/OpDb
dotnet ef migrations add ConfigMigration -c ConfigurationDbContext -o Migrations/ConfigDb

dotnet ef database update --context PersistedGrantDbContext
dotnet ef database update --context ConfigurationDbContext
```
