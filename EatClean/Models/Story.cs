using System;
using System.ComponentModel;
using Plugin.Media.Abstractions;

namespace EatClean.Models
{
    public class Story : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Caption { get; set; }
        public string Hashtag { get; set; }
        public string Ingredient { get; set; }
        public string Recipe { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeString
        {
            get
            {
                var time = DateTime.UtcNow - CreateDateTime;
                if (time.TotalHours < 1) return $"{Math.Floor(time.TotalMinutes)} minutes ago.";
                else if (time.TotalDays < 1) return $"{Math.Floor(time.TotalHours)} hours ago.";
                else return CreateDateTime.AddHours(7).ToString("ddd dd MMM yyyy HH:mm");

            }
        }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        private int commentCount;
        public int CommentCount
        {
            get { return commentCount; }
            set
            {
                commentCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommentCount"));
            }
        }

        private int likeCount;
        public int LikeCount
        {
            get { return likeCount; }
            set
            {
                likeCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LikeCount"));
            }
        }

        public MediaFile PhotoFile { get; set; }
        public string PhotoUrl => $"https://eatclean.blob.core.windows.net/story/{Photo}";

        private bool isLike;
        public bool IsLike
        {
            get { return isLike; }
            set
            {
                isLike = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Heart"));
            }
        }
        public string Heart => IsLike ? "heart" : "heartblack";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
