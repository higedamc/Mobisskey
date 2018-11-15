using System.Collections.Generic;
using Mobisskey.ViewModels;
using Xamarin.Forms;

namespace Mobisskey.Views
{
    public partial class TimelinePage
    {
        NotesPage home, local, social, global;
        ContentPage prevPage;

        public TimelinePage()
        {
            InitializeComponent();

            Children.Add(home = new NotesPage
            {
                Title = "ホーム"
            });
            Children.Add(local = new NotesPage
            {
                Title = "ローカル"
            });
            Children.Add(social = new NotesPage
            {
                Title = "ソーシャル"
            });
            Children.Add(global = new NotesPage
            {
                Title = "グローバル"
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage.Title;

            if (prevPage != null)
                prevPage.BindingContext = null;

            if (CurrentPage == home)
            {
                CurrentPage.BindingContext = new HomeTimelineViewModel();
            }
            else if (CurrentPage == local)
            {
                CurrentPage.BindingContext = new LocalTimelineViewModel();
            }
            else if (CurrentPage == social)
            {
                CurrentPage.BindingContext = new SocialTimelineViewModel();
            }
            else if (CurrentPage == global)
            {
                CurrentPage.BindingContext = new GlobalTimelineViewmodel();
            }
            prevPage = CurrentPage;
        }
    }
}
