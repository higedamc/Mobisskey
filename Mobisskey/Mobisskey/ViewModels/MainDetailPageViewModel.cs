using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;

namespace Mobisskey.ViewModels
{
	public class MainDetailPageViewModel : ViewModelBase
	{
		public MainDetailPageViewModel(INavigationService navigationService) : base(navigationService)
		{
		}


		public DelegateCommand CreateNoteCommand => new DelegateCommand(() =>
		{
			NavigationService.NavigateAsync("NewNotePage", null);
		});
	}
}
