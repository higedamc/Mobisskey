using System;
using System.Linq;
using System.Threading.Tasks;
using Mobisskey.Models;
using Disboard.Misskey.Models.Streaming;
using System.Reactive.Linq;

namespace Mobisskey.ViewModels
{
	public class HomeTimelineViewModel : NotesPageViewModel
	{
		public override IObservable<NoteMessage> InitializeStream()
		{
			return Misskey.I.Client.Streaming
				   .HomeTimelineAsObservable()
						  .OfType<NoteMessage>();
		}

		internal override async Task OnRefreshAsync()
		{
			var notes = await Misskey.I.Client.Notes.TimelineAsync();
			Notes.Clear();
			notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
		}
	}
}
