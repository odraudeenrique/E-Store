using My_Store.Infrastructure.Interfaces;
using My_Store.Infrastructure.UserInfrastructure;
using My_Store.Models.UserModels;
using My_Store.Shared;
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

        public void Create(UserDTO UserDTO)
        {
           
            Result<string> ValidatedEmail= Helper.ToValidateIfStringValid(UserDTO.Email);
            Result<string> ValidatePassword=Helper.ToValidateIfStringValid(UserDTO.Password);

            if((!ValidatedEmail.IsValid) || (!ValidatePassword.IsValid))
            {
                throw new ArgumentException("Invalid Email or Password");
            }

            User User = new User(ValidatedEmail.Value,ValidatePassword.Value);

            _repository.Create(User);
        }
        public void Update(UserDTO UserDTO)
        {
            
        }

    }
}
