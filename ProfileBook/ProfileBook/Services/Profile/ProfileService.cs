using ProfileBook.Models;
using ProfileBook.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private IRepository repository;

        public ProfileService(IRepository repository)
        {
            this.repository = repository;
        }

        public async void DeleteProfile(ProfileModel profileModel)
        {
            if (profileModel != null)
            {
                await repository.DeleteAsync(profileModel);
            }
        }

        // await and async
        public void SaveProfile(ProfileModel profileModel)
        {
            if (profileModel.Id == 0)
            {
                repository.InsertAsync(profileModel);
            }
            else
            {
                repository.UpdateAsync(profileModel);
            }
        }

        public async Task<List<ProfileModel>> GetAllProfiles()
        {
            return await repository.GetAllAsync<ProfileModel>();
        }
    }
}
