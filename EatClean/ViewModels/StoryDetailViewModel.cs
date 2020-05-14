using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EatClean.Models;
using EatClean.Services;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using EatClean.Views;

namespace EatClean.ViewModels
{
    public class StoryDetailViewModel : BaseViewModel
    {
        public CommentService CommentService => DependencyService.Get<CommentService>();
        public LikeService LikeService => DependencyService.Get<LikeService>();

        public ObservableCollection<Comment> Comments { get; set; }
        public Command LoadCommentsCommand { get; set; }
        public Command CommentCommand { get; set; }

        public Story Story { get; set; }

        private string commentDescription;
        public string CommentDescription
        {
            get { return commentDescription; }
            set
            {
                commentDescription = value;
                OnPropertyChanged("CommentDescription");
            }
        }

        public StoryDetailViewModel(Story story = null)
        {
            Title = story?.Caption;
            Story = story;

            Comments = new ObservableCollection<Comment>();
            LoadCommentsCommand = new Command(async () => await ExecuteLoadCommentsCommand());
            CommentCommand = new Command(async () => await ExecuteCommentCommand());

            MessagingCenter.Subscribe<StoryDetailPage, int>(this, "ToggleLike", async (obj, id) =>
            {
                story.IsLike = !story.IsLike;
                story.LikeCount = story.IsLike ? story.LikeCount + 1 : story.LikeCount - 1;

                await LikeService.ToggleAsync(id, Services.Setting.UserId);
            });
        }

        async Task ExecuteLoadCommentsCommand()
        {
            IsBusy = true;

            try
            {
                var comments = await CommentService.ListAsync(Story.Id, true);

                Comments.Clear();
                foreach (var comment in comments.OrderBy(s => s.Id))
                {
                    Comments.Add(comment);
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

        async Task ExecuteCommentCommand()
        {
            await CommentService.AddAsync(Story.Id, CommentDescription, Services.Setting.UserId);
            Story.CommentCount++;
            await ExecuteLoadCommentsCommand();
            CommentDescription = "";
        }
    }
}
