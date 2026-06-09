using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using TestDealershipApi.Api.ResponseModels;
using TestDealershipApi.Models;

namespace TestDealershipApi.Api
{
    public partial class ApiService
    {
        private const string BASE_URL = "https://localhost:7052";

        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient ??= new HttpClient();
        }

        public async Task<ApiResponse<Manager>> LoginAsync(string login, string password)
        {
            var request = new { Login = login, Password = password };
            return await RequestTemplates.PostAsync<Manager>(_httpClient, $"{BASE_URL}/managers", request);
        }


        public async Task<ApiResponse<Client>> AddClientAsync(string fcs, string phoneNumber, string address, DateOnly purchaseDate)
        {
            var request = new { FCs = fcs, PhoneNumber = phoneNumber, Address = address, PurchaseDate = purchaseDate };
            return await RequestTemplates.PostAsync<Client>(_httpClient, $"{BASE_URL}/clients", request);
        }


        public async Task<ApiResponse<Client>> DeleteClientAsync(int id)
        {
            return await RequestTemplates.DeleteAsync<Client>(_httpClient, $"{BASE_URL}/clients/{id}");
        }


        public async Task<ApiResponse<List<Client>>> GetAllClientsAsync()
        {
            return await RequestTemplates.GetAsync<List<Client>>(_httpClient, $"{BASE_URL}/clients");  
        }


        public async Task<ApiResponse<Client>> GetClientByIdAsync(int id)
        {
            return await RequestTemplates.GetAsync<Client>(_httpClient, $"{BASE_URL}/clients/{id}");  
        }


        public async Task<ApiResponse<Client>> UpdateClientAsync(int id, string fcs, string phoneNumber, string address, DateOnly purchaseDate)
        {
            var request = new { FCs = fcs, PhoneNumber = phoneNumber, Address = address, PurchaseDate = purchaseDate };
            return await RequestTemplates.PutAsync<Client>(_httpClient, $"{BASE_URL}/clients/{id}", request);
        }


        public async Task<ApiResponse<Car>> AddCarAsync(string brand, string model, int releaseYear, string color, decimal price, string condition)
        {
            var request = new { Brand = brand, Model = model, ReleaseYear = releaseYear, Color = color, Price = price, Condition = condition };
            return await RequestTemplates.PostAsync<Car>(_httpClient, $"{BASE_URL}/cars", request);
        }


        public async Task<ApiResponse<Car>> DeleteCarAsync(int id)
        {
            return await RequestTemplates.DeleteAsync<Car>(_httpClient, $"{BASE_URL}/cars/{id}");
        }


        public async Task<ApiResponse<List<Car>>> GetAllCarsAsync()
        {
            return await RequestTemplates.GetAsync<List<Car>>(_httpClient, $"{BASE_URL}/cars");
        }


        public async Task<ApiResponse<Car>> GetCarByIdAsync(int id)
        {
            return await RequestTemplates.GetAsync<Car>(_httpClient, $"{BASE_URL}/cars/{id}");
        }


        public async Task<ApiResponse<Car>> UpdateCarAsync(int id, string brand, string model, int releaseYear, string color, decimal price, string condition)
        {
            var request = new { Brand = brand, Model = model, ReleaseYear = releaseYear, Color = color, Price = price, Condition = condition };
            return await RequestTemplates.PutAsync<Car>(_httpClient, $"{BASE_URL}/cars/{id}", request);
        }
    }
}
