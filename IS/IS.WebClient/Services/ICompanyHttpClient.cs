using System.Net.Http;
using System.Threading.Tasks;

namespace IS.WebClient.Services
{
    public interface ICompanyHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
