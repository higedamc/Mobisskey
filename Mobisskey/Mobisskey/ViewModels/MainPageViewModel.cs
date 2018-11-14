using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Xamarin.Forms;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Mobisskey.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<MenuViewModel> Menu { get; } = new ObservableCollection<MenuViewModel>
        {

        };

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public DelegateCommand<MenuViewModel> ItemSelectedCommand => new DelegateCommand<MenuViewModel>((mvm) => 
        {
            NavigationService.NavigateAsync($"{mvm.ViewName}");
        });

    }

    public class MenuViewModel : BindableBase
    {
        public string Title { get; set; }
        public ImageSource Icon { get; set; }
        public string ViewName { get; set; }

        public MenuViewModel(string title, ImageSource icon, string viewName)
        {
            Title = title;
            Icon = icon;
            ViewName = viewName;
        }
    }
}
