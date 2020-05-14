using System;
using Xamarin.Essentials;

namespace EatClean.Services
{
    public static class Setting
    {
        public static string UserId
        {
            get => Preferences.Get("UserId", string.Empty);
            set => Preferences.Set("UserId", value);
        }

        public static string UserName
        {
            get => Preferences.Get("UserName", string.Empty);
            set => Preferences.Set("UserName", value);
        }

        public static string Photo
        {
            get => Preferences.Get("Photo", string.Empty);
            set => Preferences.Set("Photo", value);
        }

        public static string Name
        {
            get => Preferences.Get("Name", string.Empty);
            set => Preferences.Set("Name", value);
        }

        public static bool IsLoggedIn
        {
            get => Preferences.Get("IsLoggedIn", false);
            set => Preferences.Set("IsLoggedIn", value);
        }
    }
}
