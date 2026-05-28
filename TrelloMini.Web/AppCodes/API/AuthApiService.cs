using System.Net.Http.Json;
using TrelloMini.Models;

namespace TrelloMini.Web.AppCodes.API
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // POST: /auth/register
        public async Task<bool> Register(User model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/auth/register",
                model
            );

            return response.IsSuccessStatusCode;
        }

        // POST: /auth/login
        public async Task<User?> Login(User model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/auth/login",
                model
            );

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }
    }
}