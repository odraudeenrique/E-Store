using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;
using My_Store.Infrastructure.UserInfrastructure;
using My_Store.Models.UserModels;
using My_Store.Services.Interfaces;
using My_Store.Shared.Helper;
using My_Store.Shared.SecurityHelper;

namespace My_Store.Services.UserServices    
{
    public class UserService : IService<UserCreateDTO,UserResponseDTO>, IUserService<UserCreateDTO,UserResponseDTO>
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
                if (UserDTO == null)
                {
                    throw new ArgumentNullException("An error  occurred while creating the user ");
                }

                Result<(string Email, string Password)> ValidatedUser = Helper.ToValidateUserCredentials(UserDTO.Email, UserDTO.Password);

                if ((!ValidatedUser.IsValid))
                {
                    throw new ArgumentException("The user validation's failed");
                }

                Result<string> PasswordHash = SecurityHelper.ToGetPasswordHash(ValidatedUser.Value.Password);

                if (!PasswordHash.IsValid)
                {
                    throw new ArgumentException("The hashing process has failed");
                }
                
                Result<User> NewUser = User.Create(ValidatedUser.Value.Email,PasswordHash.Value);

                if (!NewUser.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                UserResponseDTO NewUserResponseDTO = await _repository.Create(NewUser.Value);

                if(NewUserResponseDTO == null)
                {
                    return null;
                }
                return NewUserResponseDTO;
            }
            catch
            {
                throw new ArgumentException("An error occurred while creating the user");
            }

        }

        public async Task<UserResponseDTO> Login (UserCreateDTO UserDTO)
        {
            try
            {
                if (UserDTO == null)
                {
                    throw new ArgumentNullException("An error  occurred while creating the user ");
                }

                Result<(string Email, string Password)>ValidatedUser=Helper.ToValidateUserCredentials(UserDTO.Email,UserDTO.Password);

                if (!ValidatedUser.IsValid)
                {
                    throw new ArgumentException("The user validation's failed");
                }

                Result<string> PasswordHash = SecurityHelper.ToGetPasswordHash(ValidatedUser.Value.Password);

                if (!PasswordHash.IsValid)
                {
                    throw new ArgumentException("The hashing process has failed");
                }


                //Acá tengo que ver si la validación va a ir en esta capa o en el controlador, porque al crear el usuario se vuelve a aplicar el hash ( Leer chat gpt)
                Result<User> UserToLogIn = User.Create(ValidatedUser.Value.Email,PasswordHash.Value);

                if (!UserToLogIn.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                IUserRepository<User, UserResponseDTO> _IUserRepository=new UserInfrastructure();

                UserResponseDTO LoggedUser= await _IUserRepository.Login(UserToLogIn.Value);

                if(LoggedUser == null)
                {
                    throw new ArgumentException("The user is not valid");
                }

                return LoggedUser;
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("An error occurred while creating the user");
            }
        }


        public void Update(UserCreateDTO UserDTO)
        {
            
        }

    }
}
