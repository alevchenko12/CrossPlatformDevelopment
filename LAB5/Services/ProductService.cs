using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LAB5.Models;

namespace LAB5.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch all products with optional filtering and pagination
        public async Task<IEnumerable<Product>> GetProductsAsync(int? brandId = null, string? productTypeCode = null, string? search = null, int? pageNumber = null, int? pageSize = null)
        {
            var queryParams = new List<string>();

            if (brandId.HasValue)
                queryParams.Add($"brandId={brandId.Value}");
            if (!string.IsNullOrEmpty(productTypeCode))
                queryParams.Add($"productTypeCode={Uri.EscapeDataString(productTypeCode)}");
            if (!string.IsNullOrEmpty(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");
            if (pageNumber.HasValue)
                queryParams.Add($"pageNumber={pageNumber.Value}");
            if (pageSize.HasValue)
                queryParams.Add($"pageSize={pageSize.Value}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            var response = await _httpClient.GetAsync($"Product{queryString}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        }

        // Fetch a single product by ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Product/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>();
        }

        // Create a new product
        public async Task<Product> CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("Product", product);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>();
        }

        // Update an existing product
        public async Task UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"Product/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        // Delete a product by ID
        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Product/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
