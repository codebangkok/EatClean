using System;
using System.Collections.Generic;
using EatClean.Models;
using EatClean.ViewModels;
using Xamarin.Forms;

namespace EatClean.Views
{
    public partial class StoryDetailPage : ContentPage
    {
        StoryDetailViewModel viewModel;

        public StoryDetailPage()
        {
            InitializeComponent();
        }

        public StoryDetailPage(StoryDetailViewModel viewModel) :this()
        {
            BindingContext = this.viewModel = viewModel;

        }

        void OnItemLike(object sender, EventArgs args)
        {
            MessagingCenter.Send(this, "ToggleLike", viewModel.Story.Id);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Comments.Count == 0) viewModel.IsBusy = true;
        }
    }
}
