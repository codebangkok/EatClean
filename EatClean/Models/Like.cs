using System;
namespace EatClean.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ApplicationUser User { get; set; }
    }
}
