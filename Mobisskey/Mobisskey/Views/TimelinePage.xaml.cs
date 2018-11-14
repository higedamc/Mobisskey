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
            });
            Children.Add(new NotesPage
            {
                Title = "ソーシャル",
            });
            Children.Add(new NotesPage
            {
                Title = "グローバル",
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage.Title;
        }
    }
}
