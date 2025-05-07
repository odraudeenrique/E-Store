using My_Store.Infrastructure.Interfaces;
using My_Store.Infrastructure.UserInfrastructure;
using My_Store.Models.UserModels;
using My_Store.Shared.Helper;

namespace My_Store.Services.UserServices
{
    public class UserService : IService<UserDTO>
    {
        private readonly IRepository<User> _repository;

        public UserService()
        {
            _repository = new UserInfrastructure();
        }

        public async Task Create(UserDTO UserDTO)
        {
            //Ahora tengo que ver cómo hago esto separando la validación para no crear el usuario de una 
            Result<User> NewUser = Helper.ToValidateUser(UserDTO.Email, UserDTO.Password);

            if((!NewUser.IsValid))
            {
                throw new ArgumentException("Invalid Email or Password");
            }

          

            try
            {
                await _repository.Create(NewUser.Value);
            }
            catch
            {
                throw new ArgumentException("An error occour; the user couldn't be created.");
            }
            
        }
        public void Update(UserDTO UserDTO)
        {
            
        }

    }
}
