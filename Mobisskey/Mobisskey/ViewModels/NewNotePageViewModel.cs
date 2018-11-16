using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Mobisskey.Models;

namespace Mobisskey.ViewModels
{
	public class NewNotePageViewModel : ViewModelBase
	{
		public NewNotePageViewModel(INavigationService navigationService) : base(navigationService)
		{
			Title = "新規投稿";
		}

		private string cw;
		public string CW
		{
			get { return cw; }
			set { SetProperty(ref cw, value); }
		}

		private bool useCW;
		public bool UseCW
		{
			get { return useCW; }
			set { SetProperty(ref useCW, value); }
		}

		private string text;
		public string Text
		{
			get { return text; }
			set { SetProperty(ref text, value); }
		}

		public DelegateCommand Post => new DelegateCommand(async () =>
		{
			await Misskey.I.Client.Notes.CreateAsync(Text, cw: UseCW ? CW : null, viaMobile: true);
			await NavigationService.GoBackAsync();
		});
	}
}
