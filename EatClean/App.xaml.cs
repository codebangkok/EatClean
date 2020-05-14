using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EatClean.Services;
using EatClean.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace EatClean
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        //public static string AzureBackendUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "http://localhost:5000";
        public static string AzureBackendUrl = "https://eatclean.azurewebsites.net";
        public static App Instance { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<StoryService>();
            DependencyService.Register<CommentService>();
            DependencyService.Register<LikeService>();
            DependencyService.Register<UserService>();

            Instance = this;

            if (Services.Setting.IsLoggedIn) MainPage = new AppShell();            
            else MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            //AppCenter.Start("android=238032a3-6fb7-4afe-80c1-f5cf4e0c8083;" +
            //      "uwp={Your UWP App secret here};" +
            //      "ios={Your iOS App secret here}",
            //      typeof(Analytics), typeof(Crashes));
            AppCenter.Start("android=238032a3-6fb7-4afe-80c1-f5cf4e0c8083", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
