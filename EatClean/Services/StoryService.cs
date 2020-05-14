using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EatClean.Models;
using Xamarin.Essentials;
using System.Text.Json;
using Plugin.Media.Abstractions;
using System.Text;

namespace EatClean.Services
{
    public class StoryService
    {
        HttpClient client;
        List<Story> stories;
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public StoryService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(App.AzureBackendUrl);

            stories = new List<Story>();
        }

        public async Task<List<Story>> ListAsync(string hashtag = "")
        {
            if (IsConnected)
            {
                var uri = $"api/story?userId={Services.Setting.UserId}";
                uri = string.IsNullOrEmpty(hashtag) ? uri : $"{uri}&hashtag={hashtag}";
                var stream = await client.GetStreamAsync(uri);
                stories = await JsonSerializer.DeserializeAsync<List<Story>>(stream);
            }

            return stories;
        }

        public async Task<List<Story>> ListByProfileAsync(string userId)
        {
            if (IsConnected)
            {
                var uri = $"api/story/{userId}";                
                var stream = await client.GetStreamAsync(uri);
                stories = await JsonSerializer.DeserializeAsync<List<Story>>(stream);
            }

            return stories;
        }

        public async Task<string> AddAsync(string caption, string hashtag, string ingredient, string recipe, string userId, MediaFile photoFile)
        {
            var content = new MultipartFormDataContent();
            var captionContent = new StringContent(caption ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(captionContent, "caption");
            var hashtagContent = new StringContent(hashtag ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(hashtagContent, "hashtag");
            var ingredientContent = new StringContent(ingredient ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(ingredientContent, "ingredient");
            var recipeContent = new StringContent(recipe ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(recipeContent, "recipe");
            var userIdContent = new StringContent(userId ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(userIdContent, "userId");

            if (photoFile != null)
            {
                var contentFile = new StreamContent(photoFile.GetStreamWithImageRotatedForExternalStorage());
                content.Add(contentFile, "file", photoFile.Path);
            }                        

            var response = await client.PostAsync($"api/story", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else return "";
        }

        public async Task<string> UpdateAsync(int id, string caption, string hashtag, string ingredient, string recipe, MediaFile photoFile)
        {
            var content = new MultipartFormDataContent();

            var idContent = new StringContent(id.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(idContent, "id");
            var captionContent = new StringContent(caption ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(captionContent, "caption");
            var hashtagContent = new StringContent(hashtag ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(hashtagContent, "hashtag");
            var ingredientContent = new StringContent(ingredient ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(ingredientContent, "ingredient");
            var recipeContent = new StringContent(recipe ?? "", Encoding.UTF8, "application/x-www-form-urlencoded");
            content.Add(recipeContent, "recipe");
            

            if (photoFile != null)
            {
                var contentFile = new StreamContent(photoFile.GetStreamWithImageRotatedForExternalStorage());
                content.Add(contentFile, "file", photoFile.Path);
            }

            var response = await client.PutAsync($"api/story/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else return "";
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!IsConnected) return false;

            var response = await client.DeleteAsync($"api/story/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
