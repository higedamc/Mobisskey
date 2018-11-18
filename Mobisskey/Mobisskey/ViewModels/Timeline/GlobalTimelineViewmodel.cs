using System;
using System.Linq;
using System.Threading.Tasks;
using Mobisskey.Models;
using Disboard.Misskey.Models.Streaming;
using System.Reactive.Linq;

namespace Mobisskey.ViewModels
{
	public class GlobalTimelineViewmodel : NotesPageViewModel
	{
		public override IObservable<NoteMessage> InitializeStream()
		{
			return Misskey.I.Client.Streaming
				   .GlobalTimelineAsObservable()
						  .OfType<NoteMessage>();
		}

		internal override async Task OnRefreshAsync()
		{
			var notes = await Misskey.I.Client.Notes.GlobalTimelineAsync();
			Notes.Clear();
			notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
		}
	}
}
