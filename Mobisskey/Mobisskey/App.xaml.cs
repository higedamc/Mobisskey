using Prism;
using Prism.Ioc;
using Mobisskey.ViewModels;
using Mobisskey.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobisskey.Models;
using System.Linq;
using Prism.Logging;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mobisskey
{
	public partial class App
	{
		/* 
		 * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
		 * This imposes a limitation in which the App class must have a default constructor. 
		 * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
		 */
		public App() : this(null) { }

		public App(IPlatformInitializer initializer) : base(initializer) { }

		protected override async void OnInitialized()
		{
			InitializeComponent();

			if (Misskey.I.Credentials != null)
			{
				var c = Misskey.I.Credentials.FirstOrDefault();
				if (c != null)
				{
					Misskey.I.SwitchClient(c);
					await NavigationService.NavigateAsync("/MainPage/NavigationPage/MainDetailPage");
					return;
				}
			}

			await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
		}

		protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
		{
			base.RegisterRequiredTypes(containerRegistry);
			containerRegistry.RegisterSingleton<ILoggerFacade, DebugLogger>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<NewNotePage, NewNotePageViewModel>();
			containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<TimelinePage, TimelinePageViewModel>();
			containerRegistry.RegisterForNavigation<MainDetailPage, MainDetailPageViewModel>();
		}
	}
}
