using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EatClean.Models;
using Xamarin.Essentials;
using System.Text.Json;

namespace EatClean.Services
{
    public class LikeService
    {
        HttpClient client;
        List<Like> likes;
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public LikeService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(App.AzureBackendUrl);

            likes = new List<Like>();
        }

        public async Task<List<Like>> ListAsync(int storyId)
        {
            if (IsConnected)
            {
                var stream = await client.GetStreamAsync($"api/like?storyId={storyId}");
                likes = await JsonSerializer.DeserializeAsync<List<Like>>(stream);
            }

            return likes;
        }

        public async Task<bool> ToggleAsync(int storyId, string userId)
        {
            if (!IsConnected) return false;

            var param = new Dictionary<string, string>();
            param.Add("storyId", storyId.ToString());
            param.Add("userId", userId);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync($"api/like", content);

            if (!response.IsSuccessStatusCode) return false;

            var isLike = await response.Content.ReadAsStringAsync();
            return bool.Parse(isLike);
        }
    }
}
