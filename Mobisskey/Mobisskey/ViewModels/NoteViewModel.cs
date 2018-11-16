using Prism.Mvvm;
using Disboard.Misskey.Models;
using Xamarin.Forms;
using System;

namespace Mobisskey.ViewModels
{
	public class NoteViewModel : BindableBase
	{
		private Note model;

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

		public NoteViewModel(Note note)
		{
			model = note;
			UserName = "@" + model.User.Username + (model.User.Host != null ? "@" + model.User.Host : "");
			// 表示名未指定のときはnullらしい
			ScreenName =  model.User.Name ?? model.User.Username;
			Icon = ImageSource.FromUri(new Uri(note.User.AvatarUrl));
			Text = model.Text;

			if (model.Renote is Note r)
			{
				IsRenote = true;
				RenoteUserName = ScreenName;
				UserName = "@" + r.User.Username + (r.User.Host != null ? "@" + r.User.Host : "");
				ScreenName = r.User.Name ?? r.User.Username;
				Icon = ImageSource.FromUri(new Uri(r.User.AvatarUrl));
				Text = r.Text;
			}
		}
	}
}
