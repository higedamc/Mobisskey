using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.Collections.ObjectModel;
using Mobisskey.Models;
using Prism.Mvvm;
using Disboard.Misskey.Models.Streaming;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Mobisskey.ViewModels
{
    public abstract class NotesPageViewModel : BindableBase
    {
        public NotesPageViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();
            // 遅延読み込み
            Observable.Timer(new TimeSpan(0, 0, 1))
                      .Subscribe((_) => OnRefreshAsync());
        }

        private ObservableCollection<NoteViewModel> notes;
        public ObservableCollection<NoteViewModel> Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public DelegateCommand RefreshCommand => new DelegateCommand(async () =>
        {
            await OnRefreshAsync();
            IsRefreshing = false;
        });

        internal abstract Task OnRefreshAsync();
    }

    public class HomeTimelineViewModel : NotesPageViewModel
    {
        public HomeTimelineViewModel() : base()
        {
            Misskey.I.Client.Streaming
                   .HomeTimelineAsObservable()
                   .OfType<NoteMessage>()
                   .Subscribe(n => Notes.Insert(0, new NoteViewModel(n)));
        }

        internal override async Task OnRefreshAsync()
        {
            var notes = await Misskey.I.Client.Notes.TimelineAsync();
            Notes.Clear();
            notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
        }
    }

    public class LocalTimelineViewModel : NotesPageViewModel
    {
        public LocalTimelineViewModel() : base()
        {
            Misskey.I.Client.Streaming
                   .LocalTimelineAsObservable()
                   .OfType<NoteMessage>()
                   .Subscribe(n => Notes.Insert(0, new NoteViewModel(n)));
        }

        internal override async Task OnRefreshAsync()
        {
            var notes = await Misskey.I.Client.Notes.LocalTimelineAsync();
            Notes.Clear();
            notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
        }
    }

    public class GlobalTimelineViewmodel : NotesPageViewModel
    {
        public GlobalTimelineViewmodel() : base()
        {
            Misskey.I.Client.Streaming
                   .GlobalTimelineAsObservable()
                   .OfType<NoteMessage>()
                   .Subscribe(n => Notes.Insert(0, new NoteViewModel(n)));
        }

        internal override async Task OnRefreshAsync()
        {
            var notes = await Misskey.I.Client.Notes.GlobalTimelineAsync();
            Notes.Clear();
            notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
        }
    }

    public class SocialTimelineViewModel : NotesPageViewModel
    {
        public SocialTimelineViewModel() : base()
        {
            //// Local + Home
            Misskey.I.Client.Streaming
                   .LocalTimelineAsObservable()
                   .Merge(Misskey.I.Client.Streaming.HomeTimelineAsObservable())
                   .OfType<NoteMessage>()
                   .DistinctUntilChanged((n) => n.Id)
                   .Subscribe(n => Notes.Insert(0, new NoteViewModel(n)));
        }

        internal override async Task OnRefreshAsync()
        {
            var notes = await Misskey.I.Client.Notes.HybridTimelineAsync();
            Notes.Clear();
            notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
        }
    }
}
