using IdentityModel.Client;
using IdentityServerDemo.MvcClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IdentityServerDemo.MvcClient.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize(Policy = "AdminCountryPolicy")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var accessToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.SetBearerToken(accessToken);
            }
            var response = client.GetAsync("https://localhost:44326/api/employees").Result;
            var jsonData = response.Content.ReadAsStringAsync().Result;
            List<string> data = JsonSerializer.Deserialize<List<string>>(jsonData);
            return View(data);


            #region calling api


            /*
            // get access token
            var serverClient = new HttpClient();
            serverClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("client_id", "client1");
            param.Add("client_secret", "client1_secret_code");
            param.Add("grant_type", "password");
            param.Add("username", "user1");
            param.Add("password", "password1");
            param.Add("scope", "employeesWebApi roles");

            var content = new FormUrlEncodedContent(param);
            var serverResponse = serverClient.PostAsync("https://localhost:44357/connect/token", content).Result;
            string jsonData = serverResponse.Content.ReadAsStringAsync().Result;

            var accessToken = JsonSerializer.Deserialize<Token>(jsonData);

            // call web api
            var apiClient = new HttpClient();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            apiClient.SetBearerToken(accessToken.access_token);

            var apiResponse = apiClient.GetAsync("https://localhost:44326/api/employees").Result;
            var jsonApiData = apiResponse.Content.ReadAsStringAsync().Result;

            List<string> apiData = JsonSerializer.Deserialize<List<string>>(jsonApiData);

            return View(apiData);
            */
            #endregion
        }

        public IActionResult Privacy()
        {
            var serverClient = new HttpClient();
            serverClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));

            var tokenResponse = serverClient.RequestPasswordTokenAsync
                                (new PasswordTokenRequest
                                {
                                    Address = "https://localhost:44357/connect/token",
                                    ClientId = "client1",
                                    ClientSecret = "client1_secret_code",
                                    GrantType = "password",
                                    UserName = "user1",
                                    Password = "password1",
                                    Scope = "employeesWebApi roles"
                                }).Result;


            // call web api
            var apiClient = new HttpClient();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var apiResponse = apiClient.GetAsync("https://localhost:44326/api/employees").Result;
            var jsonApiData = apiResponse.Content.ReadAsStringAsync().Result;

            List<string> apiData = JsonSerializer.Deserialize<List<string>>(jsonApiData);

            return View(apiData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
