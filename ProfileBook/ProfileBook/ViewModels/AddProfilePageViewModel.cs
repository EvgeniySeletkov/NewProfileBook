using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services.Profile;
using ProfileBook.Services.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class AddProfilePageViewModel : BindableBase, INavigationAware
    {
        private INavigationService navigationService;
        private IProfileService profileService;

        private ProfileModel _profile;

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string nickName;
        public string NickName
        {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private string image;
        public string Image
        {
            get
            {
                if (image == null)
                {
                    return "pic_profile.png";
                }
                else
                {
                    return image;
                }
            }
            set => SetProperty(ref image, value);
        }


        public AddProfilePageViewModel(INavigationService navigationService,
                                       IProfileService profileService)
        {
            this.navigationService = navigationService;
            this.profileService = profileService;
            GalleryAction += TakePhotoFromGallery;
            CameraAction += TakePhotoWithCamera;
        }

        public ICommand PickImageCommand => new Command(OnPickImage);
        public ICommand SaveProfileTapCommand => new Command(OnSaveProfileTap);
        public Action GalleryAction;
        public Action CameraAction;

        private void OnPickImage()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig().
                SetTitle("Title").
                Add("Gallery", GalleryAction, "ic_collections_black.png").
                Add("Camera", CameraAction, "ic_camera_alt_black.png"));
        }

        private async void TakePhotoFromGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                Image = photo.FullPath;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Сообщение об ошибке", "OK");
            }
        }

        private async void TakePhotoWithCamera()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("ddMMyyyyhhmmss")}.jpg"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                Image = photo.FullPath;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Сообщение об ошибке", "OK");
            }
        }

        private async void OnSaveProfileTap()
        {
            if (_profile == null)
            {
                _profile = new ProfileModel()
                {
                    Image = Image,
                    NickName = NickName,
                    Name = Name,
                    Description = Description,
                    Date = DateTime.Now
                };
            }
            else
            {
                _profile.Image = Image;
                _profile.NickName = NickName;
                _profile.Name = Name;
                _profile.Description = Description;
                _profile.Date = DateTime.Now;
            }
            
            profileService.SaveProfile(_profile);
            await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(Views.MainListPage)}");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            var profile = parameters.GetValue<ProfileModel>("profile");
            if (profile != null)
            {
                Title = "Edit Profile";
                _profile = profile;
                Image = profile.Image;
                NickName = profile.NickName;
                Name = profile.Name;
                Description = profile.Description;
            }
            else
            {
                Title = "Add Profile";
            }
        }
    }
}
