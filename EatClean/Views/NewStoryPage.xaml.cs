using System;
using System.Collections.Generic;
using EatClean.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace EatClean.Views
{
    public partial class NewStoryPage : ContentPage
    {
        public Story Story { get; set; }
        MediaFile photoFile;

        public NewStoryPage()
        {
            InitializeComponent();

            Story = new Story();

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Story);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Camera_Clicked(System.Object sender, System.EventArgs e)
        {
            photoFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Story",
                Name = "food.jpg"
            });

            if (photoFile == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = photoFile.GetStream();
                return stream;
            });

            Story.PhotoFile = photoFile;
        }
    }
}
