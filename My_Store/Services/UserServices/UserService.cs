using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public class UserService :  IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository)
        {
            _userRepository = UserRepository;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAll(int Page, int PageSize)
        {
            try
            {
                Result<int> AuxPage = Helper.IsGreaterThanZero(Page);


                if (!AuxPage.IsValid)
                {
                    throw new ArgumentException("The page is not valid");
                }

                Result<int> AuxPageSize = Helper.IsGreaterThanZero(PageSize);
                if (!AuxPageSize.IsValid)
                {
                    throw new ArgumentException("The page size is not valid");
                }

                int Skip = (AuxPage.Value - 1) * AuxPageSize.Value;
                int Take = AuxPageSize.Value;

                if (Skip < 0 || Take < 0)
                {
                    throw new Exception("The conversition for doing the pagination failed");
                }

                IEnumerable<UserResponseDTO> Users = await _userRepository.GetAll(Skip,Take);

                return Users;
            }catch (Exception Ex)
            {
                throw new Exception($"An error has occourred:{Ex.Message}");
            }

        }


        public async Task<UserResponseDTO?> GetById(int Id)
        {
            Result<int>UserId=Helper.IsGreaterThanZero(Id);

            if (!UserId.IsValid)
            {
                throw new ArgumentException("The Id is invalid");
            }

            try
            {
                
                UserResponseDTO? UserById = await _userRepository.GetById(UserId.Value);

                if(UserById == null)
                {
                    return null;
                }
                return UserById;

            }catch (Exception Ex)
            {
                throw new Exception($"The user couldn't be gotten from the database:{Ex.Message}");
            }
        }
        public async Task<bool> EmailExists(string Email)
        {
            Result<string> EmailForEvaluate = Helper.ToValidateUserEmail(Email);
            if (!EmailForEvaluate.IsValid)
            {
                throw new ArgumentException("The email is null or empty");
            }
            try
            {
                bool ItExists = await _userRepository.EmailExists(EmailForEvaluate.Value);

                if (!ItExists)
                {
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occourred: {ex.Message}");
            }

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

                Result<User> NewUser = User.Create(ValidatedUser.Value.Email, PasswordHash.Value);

                if (!NewUser.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                UserResponseDTO? NewUserResponseDTO = await _userRepository.Create(NewUser.Value);

                if (NewUserResponseDTO == null)
                {
                    return null;
                }
                return NewUserResponseDTO;
            }
            catch(Exception ex) 
            {
                throw new Exception($"An error occourred: {ex.Message}");
            }

        }

        public async Task<UserResponseDTO> Login(UserCreateDTO UserDTO)
        {
            try
            {
                if (UserDTO == null)
                {
                    throw new ArgumentNullException("An error  occurred while creating the user ");
                }

                Result<(string Email, string Password)> ValidatedUser = Helper.ToValidateUserCredentials(UserDTO.Email, UserDTO.Password);

                if (!ValidatedUser.IsValid)
                {
                    throw new ArgumentException("The user validation's failed");
                }

                Result<string> PasswordHash = SecurityHelper.ToGetPasswordHash(ValidatedUser.Value.Password);

                if (!PasswordHash.IsValid)
                {
                    throw new ArgumentException("The hashing process has failed");
                }


                Result<User> UserToLogIn = User.Create(ValidatedUser.Value.Email, PasswordHash.Value);

                if (!UserToLogIn.IsValid)
                {
                    throw new ArgumentException("The user is not valid");
                }

                UserResponseDTO? LoggedUser = await _userRepository.Login(UserToLogIn.Value);

                if (LoggedUser == null)
                {
                    throw new ArgumentException("The user is not valid");
                }

                //Acá tengo que llamar al token jwt y tengo que ver si coloco esa propiedadad en el UserResponseDTO o creo un nuevo DTO
                //para el logged user. 

                return LoggedUser;
            }
            catch (Exception Ex)
            {
                throw new Exception($"An error occurred:{Ex.Message}");
            }
        }


        public async Task<UserResponseDTO?> Patch(UserUpdateDTO UserDTO)
        {
            if (UserDTO == null)
            {
                throw new ArgumentNullException("The user DTO is null");
            }



            Result<int> ValidatedUserId = Helper.IsGreaterThanZero(UserDTO.Id);
            Result<string> ValidatedUserEmail = Helper.ToValidateUserEmail(UserDTO.Email);
            Result<TypeOfUser> ValidatedUserType = Helper.ToValidateUserType(UserDTO.UserType);

            if (!ValidatedUserId.IsValid)
            {
                throw new ArgumentException("The user's Id is not valid");
            }

            if (!ValidatedUserEmail.IsValid)
            {
                throw new ArgumentException("The user's email is not valid");
            }
            if ((!ValidatedUserType.IsValid) || (ValidatedUserType.Value == TypeOfUser.Invalid))
            {
                throw new ArgumentException("The user's type is null or not valid");
            }

            User Aux = new User();
            Aux.Id = ValidatedUserId.Value;
            Aux.Email = ValidatedUserEmail.Value;
            Aux.UserType = ValidatedUserType.Value;

            Result<string?> ValidatedFirstName = Helper.ToValidateUserName(UserDTO.FirstName);
            if (ValidatedFirstName.Value!=null)
            {
                Aux.FirstName = ValidatedFirstName.Value;
            }

            Result<string?> ValidatedLastName = Helper.ToValidateUserName(UserDTO.LastName);
            if (ValidatedLastName.Value!=null)
            {
                Aux.LastName = ValidatedLastName.Value;
            }

            Result<DateTime?> ValidatedBirthday = Helper.ToValidateUserBirthday(UserDTO.Birthday);
            if (ValidatedBirthday.Value != null )
            {
                Aux.Birthday = ValidatedBirthday.Value;
            }

            Result<string?> ValidatedProfilePicture = Helper.ToValidateProfilePicture(UserDTO.ProfilePicture);
            if (ValidatedProfilePicture.Value!=null)
            {
                Aux.ProfilePicture = ValidatedProfilePicture.Value;
            }

            try
            {
                UserResponseDTO? UpdatedUser = await _userRepository.Patch(Aux);


                if (UpdatedUser == null)
                {
                    throw new ArgumentException("The user couldn't be update");
                }

                return UpdatedUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occourred:{ex.Message}");
            }

        }

    }
}
