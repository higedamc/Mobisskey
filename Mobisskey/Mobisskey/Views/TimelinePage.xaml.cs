using Mobisskey.ViewModels;

namespace Mobisskey.Views
{
    public partial class TimelinePage
    {
        public TimelinePage()
        {
            InitializeComponent();
            Children.Add(new NotesPage
            {
                Title = "ホーム",
                BindingContext = new HomeTimelineViewModel(),
            });
            Children.Add(new NotesPage
            {
                Title = "ローカル",
                BindingContext = new LocalTimelineViewModel(),
            });
            Children.Add(new NotesPage
            {
                Title = "ソーシャル",
                BindingContext = new SocialTimelineViewModel(),
            });
            Children.Add(new NotesPage
            {
                Title = "グローバル",
                BindingContext = new GlobalTimelineViewmodel(),
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage.Title;
        }
    }
}
