using System;
using System.ComponentModel;
using EatClean.Models;
using EatClean.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EatClean.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel;
        MediaFile photoFile;

        public AboutPage()
        {
            InitializeComponent();
            viewModel = BindingContext as AboutViewModel;
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var story = layout.BindingContext as Story;
            await Navigation.PushAsync(new StoryDetailPage(new StoryDetailViewModel(story)));
        }

        void OnItemLike(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var story = layout.BindingContext as Story;
            MessagingCenter.Send(this, "ToggleLike", story.Id);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Stories.Count == 0) viewModel.IsBusy = true;
        }

        async void logoutButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (await DisplayAlert("", "Logout?", "Yes", "No"))
            {
                App.Instance.MainPage = new MainPage();
                Services.Setting.IsLoggedIn = false;
            }
        }

        async void photoButton_Clicked(System.Object sender, System.EventArgs e)
        {
            photoFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Profile",
                Name = "profile.jpg",
                DefaultCamera = CameraDevice.Front
            });

            if (photoFile == null)
                return;

            photoButton.Source = ImageSource.FromStream(() =>
            {
                var stream = photoFile.GetStream();
                return stream;
            });

            MessagingCenter.Send(this, "ChangeProfile", photoFile);
        }
    }
}