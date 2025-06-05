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
    public class UserService : IService<UserCreateDTO,UserResponseDTO>, IUserService
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

                IUserRepository _IUserRepository=new UserInfrastructure();

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


        public async Task<UserResponseDTO> Update(UserUpdateDTO UserDTO)
        {
            if(UserDTO == null)
            {
                throw new ArgumentNullException("The user DTO is null");
            }

           

            Result<int> ValidatedUserId = Helper.IsGreaterThanZero(UserDTO.Id);
            Result<string>ValidatedUserEmail=Helper.ToValidateUserEmail(UserDTO.Email);
            Result<TypeOfUser>ValidatedUserType=Helper.ToValidateUserType(UserDTO.UserType);

            if (!ValidatedUserId.IsValid)
            {
                throw new ArgumentException("The user's Id is not valid");
            }

            if (!ValidatedUserEmail.IsValid)
            {
                throw new ArgumentException("The user's email is not valid");
            }
            if((!ValidatedUserType.IsValid ) || (ValidatedUserType.Value==TypeOfUser.Invalid))
            {
                throw new ArgumentException("The user's type is null or not valid");
            }

            User Aux = new User();
            Aux.Id=ValidatedUserId.Value;
            Aux.Email = ValidatedUserEmail.Value;
            Aux.UserType= ValidatedUserType.Value;

            Result<string?>ValidatedFirstName=Helper.ToValidateUserName(UserDTO.FirstName);
            if (ValidatedFirstName.IsValid)
            {
                Aux.FirstName=ValidatedFirstName.Value;
            }

            Result<string?>ValidatedLastName=Helper.ToValidateUserName(UserDTO.LastName);
            if (ValidatedLastName.IsValid)
            {
                Aux.LastName=ValidatedLastName.Value;
            }

            Result<DateTime?>ValidatedBirthday=Helper.ToValidateUserBirthday(UserDTO.Birthday);
            if (ValidatedBirthday.IsValid)
            {
                Aux.Birthday=ValidatedBirthday.Value;
            }

            Result<string?>ValidatedProfilePicture=Helper.ToValidateProfilePicture(UserDTO.ProfilePicture);
            if (ValidatedProfilePicture.IsValid)
            {
                Aux.ProfilePicture=ValidatedProfilePicture.Value;
            }


            IUserRepository Infrastructure= new UserInfrastructure();   
            UserResponseDTO UpdatedUser= await Infrastructure.Patch(Aux);

            if (UpdatedUser == null)
            {
                throw new ArgumentException("The user couldn't be update");
            }

            return UpdatedUser;


           
        }

    }
}
