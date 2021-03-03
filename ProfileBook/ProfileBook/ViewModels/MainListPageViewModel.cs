using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services.Profile;
using ProfileBook.Services.Repository;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class MainListPageViewModel : BindableBase, IInitializeAsync
    {
        private INavigationService navigationService;
        private IProfileService profileService;

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private ObservableCollection<ProfileModel> profileList;
        public ObservableCollection<ProfileModel> ProfileList
        {
            get => profileList;
            set => SetProperty(ref profileList, value);
        }

        public MainListPageViewModel(INavigationService navigationService,
                                     IProfileService profileService)
        {
            Title = "Main List";
            this.navigationService = navigationService;
            this.profileService = profileService;
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var profileList = await profileService.GetAllProfiles();

            ProfileList = new ObservableCollection<ProfileModel>(profileList);
        }

        public ICommand AddProfileTapCommand => new Command(OnAddProfileTap);
        public ICommand DeleteProfileTapCommand => new Command<ProfileModel>(OnDeleteProfileTap);
        public ICommand EditProfileTapCommand => new Command<ProfileModel>(OnEditProfileTap);
        public ICommand SignOutTapCommand => new Command(OnSignOutTap);

        private async void OnAddProfileTap()
        {
            await navigationService.NavigateAsync($"{nameof(AddProfilePage)}");
        }

        private void OnDeleteProfileTap(ProfileModel profileModel)
        {
            profileService.DeleteProfile(profileModel);
            ProfileList.Remove(profileModel);
        }

        private async void OnEditProfileTap(ProfileModel profileModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add("profile", profileModel);

            await navigationService.NavigateAsync($"{nameof(AddProfilePage)}", parameters);
        }

        private async void OnSignOutTap()
        {
            await navigationService.GoBackAsync();
        }
    }
}
