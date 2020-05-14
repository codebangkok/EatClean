using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EatClean.Models;
using EatClean.Services;
using EatClean.Views;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Linq;

namespace EatClean.ViewModels
{
    public class StoryViewModel : BaseViewModel
    {
        public StoryService StoryService => DependencyService.Get<StoryService>();
        public LikeService LikeService => DependencyService.Get<LikeService>();

        public ObservableCollection<Story> Stories { get; set; }
        public Command LoadStoriesCommand { get; set; }        

        public StoryViewModel()
        {
            Title = "Eat Clean";
            Stories = new ObservableCollection<Story>();
            LoadStoriesCommand = new Command(async () => await ExecuteLoadStoriesCommand());

            MessagingCenter.Subscribe<NewStoryPage, Story>(this, "AddItem", async (obj, story) =>
            {
                IsBusy = true;
                await StoryService.AddAsync(story.Caption, story.Hashtag, story.Ingredient, story.Recipe, Services.Setting.UserId, story.PhotoFile);
                await ExecuteLoadStoriesCommand();
            });

            MessagingCenter.Subscribe<StoriesPage, int>(this, "ToggleLike", async (obj, id) =>
            {
                var story = Stories.Where(s => s.Id == id).SingleOrDefault();
                story.IsLike = !story.IsLike;
                story.LikeCount = story.IsLike ? story.LikeCount + 1 : story.LikeCount - 1;

                await LikeService.ToggleAsync(id, Services.Setting.UserId);                
            });
        }

        public async Task ExecuteLoadStoriesCommand()
        {
            IsBusy = true;

            try
            {                
                var stories = await StoryService.ListAsync("", 20, true);

                Stories.Clear();
                foreach (var story in stories.OrderByDescending(s => s.Id))
                {
                    Stories.Add(story);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
