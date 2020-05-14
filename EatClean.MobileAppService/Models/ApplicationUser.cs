using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EatClean.MobileAppService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Photo { get; set; }

        [JsonIgnore]
        public List<Story> Stories { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public List<Like> Likes { get; set; }

        [JsonIgnore]
        public override string Email { get; set; }

        [JsonIgnore]
        public override string PhoneNumber { get; set; }

        [JsonIgnore]
        public override string NormalizedUserName { get; set; }

        [JsonIgnore]
        public override string NormalizedEmail { get; set; }

        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }
    }
}