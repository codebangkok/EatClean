using System;
namespace EatClean.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ApplicationUser User { get; set; }
    }
}
