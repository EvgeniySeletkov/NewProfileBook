using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Services.Authorization;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class SignInPageViewModel : BindableBase
    {
        INavigationService navigationService;
        IAuthorizationService authorizationService;

        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string login;

        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public SignInPageViewModel(INavigationService navigationService,
                                   IAuthorizationService authorizationService)
        {
            Title = "Users SignIn";
            this.navigationService = navigationService;
            this.authorizationService = authorizationService;
        }

        public ICommand SignInTapCommand => new Command(OnSignInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private async void OnSignInTap()
        {
            var isAutorize = await authorizationService.SignIn(login, password);

            if (isAutorize)
            {
                await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
            }
            //await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
            //await navigationService.NavigateAsync($"{nameof(MainPage)}");
        }

        private async void OnSignUpTap()
        {
            //Login = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            await navigationService.NavigateAsync($"{nameof(SignUpPage)}");
        }
    }
}
