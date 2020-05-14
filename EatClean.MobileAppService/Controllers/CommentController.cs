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
    public class CommentController : ControllerBase
    {
        ApplicationDbContext db;

        public CommentController(ApplicationDbContext db)
        {
            this.db = db;    
        }

        [HttpGet("{storyId}")]
        public async Task<List<Comment>> Get(int storyId)
        {
            return await db.Comments
                           .Include(c => c.User)
                           .Where(c => c.StoryId == storyId)
                           .OrderBy(c => c.Id)
                           .ToListAsync();
        }

        [HttpPost]
        public async Task Create([FromForm]int storyId, [FromForm]string description, [FromForm]string userId)
        {
            var comment = new Comment {
                StoryId = storyId,
                Description = description,                
                UserId = userId,
                CreateDateTime = DateTime.UtcNow
            };
            await db.AddAsync(comment);
            await db.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Update([FromForm]int id, [FromForm]string description)
        {
            var comment = await db.Comments.FindAsync(id);
            comment.Description = description;
            db.Update(comment);
            await db.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            db.Remove(comment);
            await db.SaveChangesAsync();
        }
    }
}