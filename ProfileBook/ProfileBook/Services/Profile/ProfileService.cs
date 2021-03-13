using ProfileBook.Models;
using ProfileBook.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async void SaveProfile(ProfileModel profileModel)
        {
            if (profileModel.Id == 0)
            {
                await repository.InsertAsync(profileModel);
            }
            else
            {
                await repository.UpdateAsync(profileModel);
            }
        }

        public async Task<List<ProfileModel>> GetAllProfiles(int userId)
        {
            var profiles = await repository.GetAllAsync<ProfileModel>();
            return profiles.Where(x => x.UserId == userId).ToList();
            //return profiles;
        }
    }
}
