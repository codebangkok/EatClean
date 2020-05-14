using System;
using System.Text.Json.Serialization;

namespace EatClean.MobileAppService.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }        
        public DateTime CreateDateTime { get; set; }
        
        [JsonIgnore]
        public int StoryId { get; set; }
        [JsonIgnore]
        public Story Story { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }     
    }
}