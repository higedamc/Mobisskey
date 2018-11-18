using System;
using System.Linq;
using System.Threading.Tasks;
using Mobisskey.Models;
using Disboard.Misskey.Models.Streaming;
using System.Reactive.Linq;

namespace Mobisskey.ViewModels
{
	public class SocialTimelineViewModel : NotesPageViewModel
	{
		public override IObservable<NoteMessage> InitializeStream()
		{
			//// Local + Home
			return Misskey.I.Client.Streaming
					.LocalTimelineAsObservable()
					.Merge(Misskey.I.Client.Streaming.HomeTimelineAsObservable())
					.OfType<NoteMessage>()
					.DistinctUntilChanged((n) => n.Id);
		}

		internal override async Task OnRefreshAsync()
		{
			var notes = await Misskey.I.Client.Notes.HybridTimelineAsync();
			Notes.Clear();
			notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
		}
	}
}
