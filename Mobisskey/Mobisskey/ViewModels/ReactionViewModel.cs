using Prism.Mvvm;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using Disboard.Misskey.Enums;

namespace Mobisskey.ViewModels
{
	public class ReactionViewModel : BindableBase
	{
		public ReadOnlyReactiveProperty<string> Background { get; private set; }
		public ReadOnlyReactiveProperty<string> Display { get; private set; }
		public DelegateCommand ToggleReaction { get; private set; }


		private int count;
		public int Count
		{
			get { return count; }
			set { SetProperty(ref count, value); }
		}

		private string reaction;
		public string Reaction
		{
			get { return reaction; }
			set { SetProperty(ref reaction, value); }
		}

		private string emoji;
		public string Emoji
		{
			get { return emoji; }
			set { SetProperty(ref emoji, value); }
		}

		private bool isMyReaction;
		public bool IsMyReaction
		{
			get { return isMyReaction; }
			set { SetProperty(ref isMyReaction, value); }
		}

		private NoteViewModel note;

		public Reaction ReactionEnum { get; set; }

		public ReactionViewModel(NoteViewModel note, Reaction reaction)
		{
			Background = this.ToReactivePropertyAsSynchronized(p => p.IsMyReaction).Select(isMyReaction => isMyReaction ? "#BDBDBD" : "Transparent").ToReadOnlyReactiveProperty();
			Display = this.ToReactivePropertyAsSynchronized(p => p.Count).Select(c => $"{Emoji} {c}").ToReadOnlyReactiveProperty();
			ReactionEnum = reaction;
			ToggleReaction = new DelegateCommand(async () =>
			{
				await note.ReactionAsync(this);
			});
		}
	}
}