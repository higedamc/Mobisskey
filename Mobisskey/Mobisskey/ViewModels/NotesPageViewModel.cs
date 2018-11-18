using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Disboard.Misskey.Models.Streaming;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Mobisskey.ViewModels
{
	public abstract class NotesPageViewModel : BindableBase
	{
		IDisposable stream;
		protected NotesPageViewModel()
		{
			Notes = new ObservableCollection<NoteViewModel>();
			// 遅延読み込み
			OnRefreshAsync();

			stream = InitializeStream().Subscribe((m) => 
			{
				Notes.Insert(0, new NoteViewModel(m));
			});
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

		public abstract IObservable<NoteMessage> InitializeStream();

		public DelegateCommand RefreshCommand => new DelegateCommand(async () =>
		{
			await OnRefreshAsync();
			IsRefreshing = false;
		});

		internal abstract Task OnRefreshAsync();
	}
}
