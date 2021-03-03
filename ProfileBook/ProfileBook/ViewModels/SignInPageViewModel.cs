using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class SignInPageViewModel : BindableBase
    {
        INavigationService navigationService;

        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }


        public SignInPageViewModel(INavigationService navigationService)
        {
            Title = "Users SignIn";
            this.navigationService = navigationService;
        }

        public ICommand SignInTapCommand => new Command(OnSignInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private async void OnSignInTap()
        {
            await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
        }

        private async void OnSignUpTap()
        {
            await navigationService.NavigateAsync($"{nameof(SignUpPage)}");
        }
    }
}
