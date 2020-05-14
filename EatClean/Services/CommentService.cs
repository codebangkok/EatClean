using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EatClean.Models;
using Xamarin.Essentials;
using System.Text.Json;

namespace EatClean.Services
{
    public class CommentService
    {
        HttpClient client;
        List<Comment> comments;
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public CommentService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(App.AzureBackendUrl);

            comments = new List<Comment>();
        }

        public async Task<List<Comment>> ListAsync(int storyId)
        {
            if (IsConnected)
            {
                var stream = await client.GetStreamAsync($"api/comment/{storyId}");
                comments = await JsonSerializer.DeserializeAsync<List<Comment>>(stream);
            }

            return comments;
        }

        public async Task<bool> AddAsync(int storyId, string description, string userId)
        {
            if (!IsConnected) return false;

            var param = new Dictionary<string, string>();
            param.Add("storyId", storyId.ToString());
            param.Add("description", description);
            param.Add("userId", userId);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync($"api/comment", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, string description)
        {
            if (!IsConnected) return false;

            var param = new Dictionary<string, string>();
            param.Add("id", id.ToString());
            param.Add("description", description);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PutAsync($"api/comment/{id}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!IsConnected) return false;

            var response = await client.DeleteAsync($"api/comment/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
