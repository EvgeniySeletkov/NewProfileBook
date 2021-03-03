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
    class SignUpPageViewModel : BindableBase
    {
        INavigationService navigationService;

        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public SignUpPageViewModel(INavigationService navigationService)
        {
            Title = "Users SignUp";
            this.navigationService = navigationService;
        }

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private async void OnSignUpTap()
        {
            await navigationService.GoBackAsync();
        }
    }
}
