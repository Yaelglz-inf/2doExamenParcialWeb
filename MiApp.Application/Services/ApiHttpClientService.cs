using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiApp.Application.Services
{
    public interface IApiHttpClientService
    {
        Task<T> GetAsync<T>(string url, string bearerToken = null);
        Task<T> PostAsync<T>(string url, object data, string bearerToken = null);
        Task<string> PostAsync(string url, object data, string bearerToken = null);
    }

    public class ApiHttpClientService : IApiHttpClientService
    {
        private readonly HttpClient _httpClient;

        public ApiHttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url, string bearerToken = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {bearerToken}");
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en GET {url}: {ex.Message}", ex);
            }
        }

        public async Task<T> PostAsync<T>(string url, object data, string bearerToken = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {bearerToken}");
                }

                request.Content = JsonContent.Create(data);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en POST {url}: {ex.Message}", ex);
            }
        }

        public async Task<string> PostAsync(string url, object data, string bearerToken = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {bearerToken}");
                }

                request.Content = JsonContent.Create(data);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en POST {url}: {ex.Message}", ex);
            }
        }
    }
}
