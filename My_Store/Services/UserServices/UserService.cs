using My_Store.Infrastructure.Interfaces;
using My_Store.Infrastructure.UserInfrastructure;
using My_Store.Models.UserModels;
using My_Store.Shared.Helper;

namespace My_Store.Services.UserServices
{
    public class UserService : IService<UserCreateDTO>
    {
        private readonly IRepository<User> _repository;

        public UserService()
        {
            _repository = new UserInfrastructure();
        }

        public async Task Create(UserCreateDTO UserDTO)
        {
            try
            {
                //User NewUser=new User(UserCredentials.Value.Email, UserCredentials.Value.Password);
                Result<User> NewUser = User.Create(UserDTO.Email, UserDTO.Password);

                if (!NewUser.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                await _repository.Create(NewUser.Value);
            }
            catch
            {
                throw new ArgumentException("An error occour; the user couldn't be created.");
            }
            
        }
        public void Update(UserCreateDTO UserDTO)
        {
            
        }

    }
}
