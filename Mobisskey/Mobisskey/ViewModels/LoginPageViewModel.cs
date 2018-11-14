using Disboard.Misskey.Enums;
using Disboard.Misskey.Extensions;
using Disboard.Misskey.Models;
using Mobisskey.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobisskey.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { SetProperty(ref url, value); }
        }

        private string error;
        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        private bool haveToOpenUrl;
        public bool HaveToOpenUrl
        {
            get { return haveToOpenUrl; }
            set { SetProperty(ref haveToOpenUrl, value); }
        }

        private bool isPending;
        public bool IsPending
        {
            get { return isPending; }
            set { SetProperty(ref isPending, value); }
        }

        private bool isLoginMode = true;
        public bool IsLoginMode
        {
            get { return isLoginMode; }
            set { SetProperty(ref isLoginMode, value); }
        }

        Session session;

        public DelegateCommand Login => new DelegateCommand(async () =>
        {
            try
            {
                Error = "";
                IsEnabled = false;
                
                var cli = Misskey.I.SwitchClient(Url);

                await cli.App.CreateAsync($"Mobisskey for {GetDeviceName()}", "A native misskey client for mobile terminals.", (Enum.GetValues(typeof(Permission)) as Permission[]).Select(p => p.ToStr()).ToArray(), "about:blank");

                session = await Misskey.I.Client.Auth.Session.GenerateAsync();

                // XAMLのバインディングは式ぐらい書かせろ:anger:
                IsLoginMode = false;
                HaveToOpenUrl = true;

                IsEnabled = true;
            }
            catch (Exception ex)
            {
                Error = $"エラー({ex.GetType().Name}) {ex.Message}";
                IsEnabled = true;
            }
        });

        public DelegateCommand OpenUrl => new DelegateCommand(() =>
        {
            HaveToOpenUrl = false;
            IsPending = true;
            Device.OpenUri(new Uri(session.Url));
        });

        public DelegateCommand UserKey => new DelegateCommand(async () =>
        {
            if (session == null)
            {
                Error = "トークンが消失しました。もう一度お試しください。";
                IsLoginMode = true;
                IsPending = false;
                return;
            }
            await Misskey.I.Client.Auth.Session.UserKeyAsync(session.Token);
            Misskey.I.Save(Misskey.I.Client.Credential);
            await NavigationService.NavigateAsync("/MainPage/NavigationPage/MainDetailPage");
        });

        static string GetDeviceName()
        {
            // Android と iOS 以外は知らない
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return "Android";
                case Device.iOS:
                    return "iOS";
                default:
                    return "Unknown";
            }
        }
    }
}
