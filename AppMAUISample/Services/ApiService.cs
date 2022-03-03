using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppMAUISample.Services
{
    public static class ApiService
    {
        static HttpClient httpClient;
        static JsonSerializerOptions serializerOptions;
        public static string ErrorMessage;

        static ApiService()
        {
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(Utils.Constants.Api.BASE_URL),
                Timeout = TimeSpan.FromMinutes(2)
            };
        }

        public static async Task<List<AppUser>> GetAllAsync(string search = null)
        {
            ErrorMessage = string.Empty;
            var items = new List<AppUser>();
            try
            {
                var response = await httpClient.GetAsync($"/api/users?search={search}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    items = await JsonSerializer.DeserializeAsync<List<AppUser>>(content, serializerOptions);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException ||
                               ex is InvalidOperationException ||
                               ex is TaskCanceledException)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);  
            }
            return items;
        }

        public static async Task DeleteAsync(AppUser user)
        {
            ErrorMessage = string.Empty;
            try
            {
                var response = await httpClient.DeleteAsync($"/api/users/{user.Id}");
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) when (ex is HttpRequestException ||
                               ex is InvalidOperationException ||
                               ex is TaskCanceledException)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task SaveAsync(AppUser user)
        {
            ErrorMessage = string.Empty;
            try
            {
                var json = JsonSerializer.Serialize(user, serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = user.Id == 0
                    ? await httpClient.PostAsync($"/api/users", content)
                    : await httpClient.PutAsync($"/api/users/{user.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) when (ex is HttpRequestException ||
                               ex is InvalidOperationException ||
                               ex is TaskCanceledException)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
            }
        }
    }
}
