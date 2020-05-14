using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using EatClean.MobileAppService.Models;
using EatClean.MobileAppService.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using EatClean.MobileAppService.Helpers;

namespace EatClean.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        ApplicationDbContext db;
        AzureStorageConfig storageConfig;

        public StoryController(ApplicationDbContext db, IOptions<AzureStorageConfig> config)
        {
            this.db = db;
            this.storageConfig = config.Value;
        }

        [HttpGet]
        public async Task<List<Story>> GetByLatest(string userId, string hashtag = "", int count = 20)
        {
            var hashtags = hashtag.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var stories = await db.Stories
                           .Include(s => s.Comments)
                           .ThenInclude(c => c.User)
                           .Include(s => s.Likes)
                           .Include(s => s.User)
                           .Where(s => s.Caption.Contains(hashtag) || hashtags.Contains(s.Hashtag))
                           .OrderByDescending(s => s.Id)
                           .Take(count)
                           .ToListAsync();
            foreach (var story in stories)
            {
                story.IsLike = story.Likes.Where(l => l.UserId == userId).Any();
            }
            return stories;
        }

        [HttpGet("{userId}")]
        public async Task<List<Story>> GetByProfile(string userId)
        {
            var stories = await db.Stories
                           .Include(s => s.Comments)
                           .ThenInclude(c => c.User)
                           .Include(s => s.Likes)
                           .Include(s => s.User)
                           .Where(s => s.UserId == userId)
                           .OrderByDescending(s => s.Id)
                           .ToListAsync();
            foreach (var story in stories)
            {
                story.IsLike = story.Likes.Where(l => l.UserId == userId).Any();
            }
            return stories;
        }

        //[HttpGet("{id}")]
        //public async Task<Story> GetById(int id)
        //{
        //    return await db.Stories
        //                   .Include(s => s.Comments)
        //                   .ThenInclude(c => c.User)
        //                   .Include(s => s.Likes)
        //                   .Include(s => s.User)
        //                   .Where(s => s.Id == id)
        //                   .SingleOrDefaultAsync();
        //}

        [HttpPost]
        public async Task<string> Create([FromForm]string caption, [FromForm]string hashtag, [FromForm]string ingredient, [FromForm]string recipe, [FromForm]string userId, [FromForm]IFormFile file)
        {            
            var story = new Story {
                Caption = caption,
                Hashtag = hashtag,
                Ingredient = ingredient,
                Recipe = recipe,
                UserId = userId,
                Photo = "noimage.png",
                CreateDateTime = DateTime.UtcNow
            };

            if (file != null && file.Length > 0)
            {
                var index = file.FileName.LastIndexOf('.');
                var fileName = $"{Guid.NewGuid()}{file.FileName.Substring(index)}";                
                                
                using (var stream = file.OpenReadStream())
                {
                    if (await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, "story"))
                    {
                        story.Photo = fileName;
                    }
                }
            }
            
            await db.AddAsync(story);
            await db.SaveChangesAsync();
            return story.Photo;
        }

        [HttpPut("{id}")]
        public async Task<string> Update([FromForm]int id, [FromForm]string caption, [FromForm]string hashtag, [FromForm]string ingredient, [FromForm]string recipe, [FromForm]IFormFile file)
        {
            var story = await db.Stories.FindAsync(id);
            story.Caption = caption;
            story.Hashtag = hashtag;
            story.Ingredient = ingredient;
            story.Recipe = recipe;

            if (file != null && file.Length > 0)
            {
                var index = file.FileName.LastIndexOf('.');
                var fileName = $"{Guid.NewGuid()}{file.FileName.Substring(index)}";

                using (var stream = file.OpenReadStream())
                {
                    if (await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, "story"))
                    {
                        story.Photo = fileName;
                    }
                }
            }

            db.Update(story);
            await db.SaveChangesAsync();
            return story.Photo;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var story = await db.Stories.FindAsync(id);
            db.Remove(story);
            await db.SaveChangesAsync();
        }
    }
}