using System.Net.Http;
using System.Text.Json;
using PCStore.Models;

namespace PCStore.Services
{
    public class ExternalProductService
    {
        private readonly HttpClient _httpClient;

        public ExternalProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProductsFromExternalServiceAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/cache"); // или друг външен URL
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return products ?? new List<Product>();
        }
    }
}