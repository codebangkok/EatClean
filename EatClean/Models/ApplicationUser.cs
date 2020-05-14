using System;
namespace EatClean.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl => $"https://eatclean.blob.core.windows.net/profile/{Photo}";
    }
}
