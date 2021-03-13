using ProfileBook.Models;
using ProfileBook.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Authorization
{
    class AuthorizationService : IAuthorizationService
    {
        private IRepository repository;

        public AuthorizationService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> IsLoginBusy(string login)
        {
            var users = await repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(x => x.Login == login);

            return user != null;
        }

        public async Task<bool> SignIn(string login, string password)
        {
            var users = await repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(x => x.Login == login && x.Password == password);

            return user != null;
        }

        public async void SignUp(UserModel userModel)
        {
            await repository.InsertAsync(userModel);
        }
    }
}
