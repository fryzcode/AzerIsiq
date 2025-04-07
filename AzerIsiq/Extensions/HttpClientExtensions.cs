using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzerIsiq.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest data,
        JsonSerializerOptions? options = null)
    {
        var response = await httpClient.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TResponse>(options);
        return result;
    }

    public static async Task<T?> GetJsonAsync<T>(
        this HttpClient httpClient,
        string url,
        JsonSerializerOptions? options = null)
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<T>(options);
        return result;
    }
}
