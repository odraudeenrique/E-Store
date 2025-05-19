using My_Store.Infrastructure.Interfaces;
using My_Store.Infrastructure.UserInfrastructure;
using My_Store.Models.UserModels;
using My_Store.Shared.Helper;

namespace My_Store.Services.UserServices
{
    public class UserService : IService<UserCreateDTO,UserResponseDTO>
    {
        private readonly IRepository<User,UserResponseDTO> _repository;

        public UserService()
        {
            _repository = new UserInfrastructure();
        }

        public async Task<UserResponseDTO> Create(UserCreateDTO UserDTO)
        {
            try
            {
                Result<User> NewUser = User.Create(UserDTO.Email, UserDTO.Password);

                if (!NewUser.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                UserResponseDTO NewUserResponseDTO = await _repository.Create(NewUser.Value);
                return NewUserResponseDTO;
            }
            catch
            {
                throw new ArgumentException("An error occurred while creating the user");
                throw new ArgumentException("An error occurred while creating the user");
            }
            
        }
        public void Update(UserCreateDTO UserDTO)
        {
            
        }

    }
}
