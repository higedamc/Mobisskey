using Prism.Mvvm;
using Disboard.Misskey.Models;
using Xamarin.Forms;
using System;
using Mobisskey.Models;
using Disboard.Misskey.Enums;
using Disboard.Misskey.Extensions;
using Disboard.Exceptions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Mobisskey.ViewModels
{
	public class NoteViewModel : BindableBase
	{
		private readonly Note model;

		private ImageSource icon;
		public ImageSource Icon
		{
			get { return icon; }
			set { SetProperty(ref icon, value); }
		}

		private string userName;
		public string UserName
		{
			get { return userName; }
			set { SetProperty(ref userName, value); }
		}

		private string screenName;
		public string ScreenName
		{
			get { return screenName; }
			set { SetProperty(ref screenName, value); }
		}

		private string text;
		public string Text
		{
			get { return text; }
			set { SetProperty(ref text, value); }
		}

		private bool isRenote;
		public bool IsRenote
		{
			get { return isRenote; }
			set { SetProperty(ref isRenote, value); }
		}

		private string renoteUserName;
		public string RenoteUserName
		{
			get { return renoteUserName; }
			set { SetProperty(ref renoteUserName, value); }
		}

		private NoteViewModel repliedNote;
		public NoteViewModel RepliedNote
		{
			get { return repliedNote; }
			set { SetProperty(ref repliedNote, value); }
		}

		private ObservableCollection<ReactionViewModel> reactions;
		public ObservableCollection<ReactionViewModel> Reactions
		{
			get { return reactions; }
			set { SetProperty(ref reactions, value); }
		}

		public NoteViewModel(Note note)
		{
			model = note;
			// テキストが空のrenote
			if (model.Renote is Note r && model.Text == null)
			{
				RenoteUserName = note.User.Name ?? note.User.Username;
				IsRenote = true;
				model = r;
			}
			UserName = "@" + model.User.Username + (model.User.Host != null ? "@" + model.User.Host : "");
			// 表示名未指定のときはnullらしい
			ScreenName = model.User.Name ?? model.User.Username;

			SetIconAsync();

			Text = model.Text;
			Reactions = new ObservableCollection<ReactionViewModel>
			{
				{ this, Reaction.Like, model.MyReaction, model.ReactionCounts?.Like ?? 0 },
				{ this, Reaction.Love, model.MyReaction, model.ReactionCounts?.Love ?? 0 },
				{ this, Reaction.Laugh, model.MyReaction, model.ReactionCounts?.Laugh ?? 0 },
				{ this, Reaction.Hmm, model.MyReaction, model.ReactionCounts?.Hmm ?? 0 },
				{ this, Reaction.Surprise, model.MyReaction, model.ReactionCounts?.Surprise ?? 0 },
				{ this, Reaction.Congrats, model.MyReaction, model.ReactionCounts?.Congrats ?? 0 },
				{ this, Reaction.Angry, model.MyReaction, model.ReactionCounts?.Angry ?? 0 },
				{ this, Reaction.Confused, model.MyReaction, model.ReactionCounts?.Confused ?? 0 },
				{ this, Reaction.Rip, model.MyReaction, model.ReactionCounts?.Rip ?? 0 },
				{ this, Reaction.Pudding, model.MyReaction, model.ReactionCounts?.Pudding ?? 0 },
			};
			Console.WriteLine("test");
		}

		async Task SetIconAsync()
		{
			Icon = ImageSource.FromFile(await ImageCache.I.DownloadFileAsync(model.User.AvatarUrl, model.User.Id));
		}

		/// <summary>
		/// リアクションをトグルします。
		/// </summary>
		/// <param name="reaction">Reaction.</param>
		public async Task ReactionAsync(ReactionViewModel reaction)
		{
			try
			{
				// そのリアクションを押している場合
				if (model.MyReaction is Reaction r && reaction.Reaction == r.ToStr())
				{
					await Misskey.I.Client.Notes.Reactions.DeleteAsync(model.Id);
				}
				else if (model.MyReaction == default)
				{
					await Misskey.I.Client.Notes.Reactions.CreateAsync(model.Id, reaction.ReactionEnum);
				}
			}
			catch (DisboardException ex)
			{
				Console.WriteLine($"{ex.GetType().Name} {ex.Message}\n{ex.StackTrace}");
			}
		}
	}
}