using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services.Authorization;
using ProfileBook.Services.Repository;
using ProfileBook.Services.Settings;
using ProfileBook.Services.Validators;
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
        IAuthorizationService authorizationService;
        IValidators validators;

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

        private string confirmPassword;

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public SignUpPageViewModel(INavigationService navigationService,
                                   IAuthorizationService authorizationService,
                                   IValidators validators)
        {
            Title = "Users SignUp";
            this.navigationService = navigationService;
            this.authorizationService = authorizationService;
            this.validators = validators;
        }

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private void ClearEntries()
        {
            Login = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private bool IsLoginValidate()
        {
            if (validators.IsFirstSymbolDigit(Login))
            {
                UserDialogs.Instance.Alert("First simbol doesn`t have to be digit in login!", "Alert", "OK");
                ClearEntries();
                return false;
            }
            if (!validators.IsCorrectLength(Login, 3))
            {
                UserDialogs.Instance.Alert("Number of characters less than 3 or more than 20!", "Alert", "OK");
                ClearEntries();
                return false;
            }
            return true;
        }

        private bool IsPassValidate()
        {
            if (!validators.IsPassAvailable(Password))
            {
                UserDialogs.Instance.Alert("Must be a number, uppercase or lowercase letter!", "Alert", "OK");
                ClearEntries();
                return false;
            }
            if (!validators.ArePasswordsEquals(Password, ConfirmPassword))
            {
                UserDialogs.Instance.Alert("Password and confirm password are not equal!", "Alert", "OK");
                ClearEntries();
                return false;
            }
            if (!validators.IsCorrectLength(Password, 6))
            {
                UserDialogs.Instance.Alert("Number of characters less than 6 or more than 20!", "Alert", "OK");
                ClearEntries();
                return false;
            }
            return true;
        }

        private bool EntriesAreEmpty()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                UserDialogs.Instance.Alert("One or more fields are empty!", "Alert", "OK");
                return true;
            }
            return false;
        }

        private UserModel CreateUser()
        {
            UserModel userModel = null;

            if (Login != Password)
            {
                userModel = new UserModel()
                {
                    Login = Login,
                    Password = Password
                };
            }
            else
            {
                UserDialogs.Instance.Alert("Login and password don`t have to be equal!", "Alert", "OK");
                ClearEntries();
            }

            return userModel;
        }

        private async void OnSignUpTap()
        {
            if (!EntriesAreEmpty())
            {
                if (IsLoginValidate() && IsPassValidate())
                {
                    var isLoginBusy = await authorizationService.IsLoginBusy(Login);
                    if (isLoginBusy)
                    {
                        UserDialogs.Instance.Alert("This login is busy!", "Alert", "OK");
                        ClearEntries();
                    }
                    else
                    {
                        var userModel = CreateUser();
                        if (userModel != null)
                        {
                            authorizationService.SignUp(userModel);
                            await navigationService.GoBackAsync();
                        }
                    }
                }
            }

        }
    }
}
