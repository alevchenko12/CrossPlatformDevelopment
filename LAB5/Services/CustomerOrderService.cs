using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LAB5.Models;

namespace LAB5.Services
{
    public class CustomerOrderService
    {
        private readonly HttpClient _httpClient;

        public CustomerOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CustomerOrder>> GetOrdersAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            List<string>? statusCodes = null,
            string? startsWith = null,
            string? endsWith = null)
        {
            var queryParams = new List<string>();

            if (startDate.HasValue)
                queryParams.Add($"startDate={startDate.Value:O}"); 
            if (endDate.HasValue)
                queryParams.Add($"endDate={endDate.Value:O}");
            if (statusCodes != null && statusCodes.Count > 0)
                queryParams.Add($"statusCodes={Uri.EscapeDataString(string.Join(",", statusCodes))}");
            if (!string.IsNullOrEmpty(startsWith))
                queryParams.Add($"startsWith={Uri.EscapeDataString(startsWith)}");
            if (!string.IsNullOrEmpty(endsWith))
                queryParams.Add($"endsWith={Uri.EscapeDataString(endsWith)}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            var response = await _httpClient.GetAsync($"CustomerOrder{queryString}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<CustomerOrder>>();
        }

        public async Task<CustomerOrder> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"CustomerOrder/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CustomerOrder>();
        }
    }
}
