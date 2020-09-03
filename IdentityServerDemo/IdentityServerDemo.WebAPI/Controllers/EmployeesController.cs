using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServerDemo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [Authorize(Policy = "AdminCountryPolicy")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public List<string> Get()
        {
            var givenName = User.Claims.FirstOrDefault(c => c.Type == "given_name").Value;
            var familyName = User.Claims.FirstOrDefault(c => c.Type == "family_name").Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == "email").Value;
            var phone = User.Claims.FirstOrDefault(c => c.Type == "phone").Value;
            var address = User.Claims.FirstOrDefault(c => c.Type == "address").Value;

            return new List<string>() {
                givenName,
                familyName,
                email,
                address,
                phone,
                "Nancy Davolio",
                "Andrew Fuller",
                "Janet Leverling"
            };
        }
    }
}
