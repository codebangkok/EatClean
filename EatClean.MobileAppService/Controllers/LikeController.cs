using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using EatClean.MobileAppService.Models;
using EatClean.MobileAppService.Data;
using System.Threading.Tasks;
using System;

namespace EatClean.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        ApplicationDbContext db;

        public LikeController(ApplicationDbContext db)
        {
            this.db = db;    
        }

        [HttpGet("{storyId}")]
        public async Task<List<Like>> Get(int storyId)
        {
            return await db.Likes
                           .Include(l => l.User)
                           .Where(l => l.StoryId == storyId)
                           .OrderBy(l => l.Id)
                           .ToListAsync();
        }

        [HttpPost]
        public async Task<bool> Toggle([FromForm]int storyId, [FromForm]string userId)
        {
            var isLike = false;

            var like = await db.Likes.Where(l => l.StoryId == storyId).SingleOrDefaultAsync();
            if (like == null)
            {
                like = new Like
                {
                    StoryId = storyId,
                    UserId = userId,
                    CreateDateTime = DateTime.UtcNow
                };
                await db.AddAsync(like);
                isLike = true;
            }
            else db.Remove(like);            

            await db.SaveChangesAsync();
            return isLike;
        }
    }
}