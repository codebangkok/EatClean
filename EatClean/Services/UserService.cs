using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EatClean.Models;
using Xamarin.Essentials;

namespace EatClean.Services
{
    public class UserService
    {
        HttpClient client;
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public UserService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(App.AzureBackendUrl);
        }

        public async Task<ApplicationUser> Register(string username, string password)
        {
            var param = new Dictionary<string, string>();
            param.Add("username", username);
            param.Add("password", password);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync($"api/user/register", content);

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApplicationUser>(stream);
        }

        public async Task<ApplicationUser> Login(string username, string password)
        {
            var param = new Dictionary<string, string>();
            param.Add("username", username);
            param.Add("password", password);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync($"api/user/login", content);

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApplicationUser>(stream);
        }
    }
}
