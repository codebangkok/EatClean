using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EatClean.Models;
using Plugin.Media.Abstractions;
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

        public async Task<string> Update(string userId, string name, MediaFile photoFile)
        {
            var content = new MultipartFormDataContent();

            var userIdContent = new StringContent(userId, Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(userIdContent, "userId");
            var nameContent = new StringContent(name, Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(nameContent, "name");            

            if (photoFile != null)
            {
                var contentFile = new StreamContent(photoFile.GetStreamWithImageRotatedForExternalStorage());
                content.Add(contentFile, "file", photoFile.Path);
            }

            var response = await client.PutAsync($"api/user/update", content);

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            return await response.Content.ReadAsStringAsync();            
        }
    }
}
