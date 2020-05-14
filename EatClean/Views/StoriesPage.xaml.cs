using System;
using System.Collections.Generic;
using EatClean.Models;
using EatClean.ViewModels;
using Xamarin.Forms;

namespace EatClean.Views
{
    public partial class StoriesPage : ContentPage
    {
        StoryViewModel viewModel;

        public StoriesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new StoryViewModel();
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

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewStoryPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Stories.Count == 0) viewModel.IsBusy = true;
        }
    }
}
