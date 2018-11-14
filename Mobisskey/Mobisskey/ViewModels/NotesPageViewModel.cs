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

namespace Mobisskey.ViewModels
{
    public abstract class NotesPageViewModel : BindableBase
    {
        public NotesPageViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();
            OnRefreshAsync();
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
        internal override async Task OnRefreshAsync()
        {
            var notes = await Misskey.I.Client.Notes.TimelineAsync();
            Notes.Clear();
            notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
        }
    }
}
