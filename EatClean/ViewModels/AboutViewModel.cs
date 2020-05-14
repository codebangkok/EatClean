using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EatClean.Models;
using EatClean.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using EatClean.Views;

namespace EatClean.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public StoryService StoryService => DependencyService.Get<StoryService>();
        public LikeService LikeService => DependencyService.Get<LikeService>();

        public ObservableCollection<Story> Stories { get; set; }
        public Command LoadStoriesCommand { get; set; }        

        public ApplicationUser User { get; set; }

        public AboutViewModel()
        {
            Title = "Profile";
            Stories = new ObservableCollection<Story>();
            LoadStoriesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            User = new ApplicationUser
            {
                Id = Services.Setting.UserId,
                Name = Services.Setting.Name,
                UserName = Services.Setting.UserName,
                Photo = Services.Setting.Photo
            };

            MessagingCenter.Subscribe<AboutPage, int>(this, "ToggleLike", async (obj, id) =>
            {
                var story = Stories.Where(s => s.Id == id).SingleOrDefault();
                story.IsLike = !story.IsLike;
                story.LikeCount = story.IsLike ? story.LikeCount + 1 : story.LikeCount - 1;

                await LikeService.ToggleAsync(id, Services.Setting.UserId);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var stories = await StoryService.ListByProfileAsync(User.Id, true);

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