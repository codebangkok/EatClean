using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;

namespace EatClean.MobileAppService.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Caption { get; set; }
        public string Hashtag { get; set; }
        public string Ingredient { get; set; }
        public string Recipe { get; set; }
        public DateTime CreateDateTime { get; set; }       

        [JsonIgnore]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public List<Like> Likes { get; set; }

        [NotMapped]
        public int CommentCount => Comments.Count;

        [NotMapped]
        public int LikeCount => Likes.Count;

        [NotMapped]
        public bool IsLike { get; set; }
    }
}