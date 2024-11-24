using LAB5.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LAB5.Services
{
    public class RetailerService
    {
        private readonly HttpClient _httpClient;

        public RetailerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Retailer>> GetRetailersAsync()
        {
            var response = await _httpClient.GetAsync("Retailer");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Retailer>>();
        }

        public async Task<Retailer> GetRetailerByIdAsync(int retailerId)
        {
            var response = await _httpClient.GetAsync($"Retailer/{retailerId}"); 
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Retailer>();
            }
            return null;
        }
    }
}
