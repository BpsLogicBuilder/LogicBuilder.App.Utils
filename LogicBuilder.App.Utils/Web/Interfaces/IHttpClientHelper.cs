using System.Text.Json;
using System.Threading.Tasks;

namespace LogicBuilder.App.Utils.Web.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<TResult> GetAsync<TResult>(string url, JsonSerializerOptions? options = null, string httpClientName = "");
        Task<TResult> PostAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null, string httpClientName = "");
        Task<TResult> PutAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null, string httpClientName = "");
    }
}
