using My_Store.Models.UserModels;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using Microsoft.AspNetCore.Routing.Tree;

namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure : IUserRepository
    {
        private readonly IDataAccess _data;
        //private DataAccess Data { get { return this._data; } set { this._data = value; } }

        public UserInfrastructure(IDataAccess IData)
        {
            _data = IData;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAll(int Skip, int Take)
        {
            _data.ToSetProcedure("GetAll");
            try
            {
                _data.ToSetParameters("@Skip", Skip);
                _data.ToSetParameters("@Take", Take);

                SqlDataReader Reader = await _data.ToRead();

                int TotalNumberOfUser = 0;

                if (await Reader.ReadAsync())
                {
                    TotalNumberOfUser = (int)Reader["TotalCount"];
                }

                Reader.NextResult();

                List<UserResponseDTO> Users = new List<UserResponseDTO>();


                while (await Reader.ReadAsync())
                {
                    Result<int> UserId = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> UserEmail = Helper.ToValidateUserEmail((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);
                    Result<bool> UserActive = Helper.ToValidateActiveUser(Reader["IsActive"]);


                    if (((!UserId.IsValid) || (!UserEmail.IsValid)) || ((!UserType.IsValid) && (UserType.Value != TypeOfUser.Invalid)) || (!UserActive.IsValid))
                    {
                        continue;
                    }

                    UserResponseDTO Aux = new UserResponseDTO();

                    Aux.Id = UserId.Value;
                    Aux.Email = UserEmail.Value;
                    Aux.UserType = UserType.Value;
                    Aux.IsActive = UserActive.Value;

                    Result<string?> UserFirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    if ((UserFirstName.IsValid) && (UserFirstName != null))
                    {
                        Aux.FirstName = UserFirstName.Value;
                    }
                    else
                    {
                        Aux.FirstName = null;
                    }

                    Result<string?> UserLastName = Helper.ToValidateUserName(Reader["LastName"]);
                    if ((UserLastName.IsValid) && (UserLastName != null))
                    {
                        Aux.LastName = UserLastName.Value;
                    }
                    else
                    {
                        Aux.LastName = null;
                    }

                    Result<DateTime?> UserBirthday = Helper.ToValidateUserBirthday(Reader["Birthday"]);
                    if ((UserBirthday.IsValid) && (UserBirthday != null))
                    {
                        Aux.Birthday = UserBirthday.Value;
                    }
                    else
                    {
                        Aux.Birthday = null;
                    }

                    Result<string?> UserProfilePicture = Helper.ToValidateProfilePicture(Reader["ProfilePicture"]);
                    if ((UserProfilePicture.IsValid) && (UserProfilePicture != null))
                    {
                        Aux.ProfilePicture = UserProfilePicture.Value;
                    }
                    else
                    {
                        Aux.ProfilePicture = null;
                    }
                    Users.Add(Aux);

                }

                return Users;

            }
            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (_data != null)
                {
                    await _data.DisposeAsync();
                }
            }

        }


        public async Task<UserResponseDTO?> GetById(int Id)
        {
            _data.ToSetProcedure("GetById");
            _data.ToSetParameters("@Id", Id);

            try
            {
                SqlDataReader Reader = await _data.ToRead();

                while (await Reader.ReadAsync())
                {
                    UserResponseDTO User = new UserResponseDTO();


                    Result<int> UserId = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> UserEmail = Helper.ToValidateUserEmail((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);
                    Result<bool> UserActive = Helper.ToValidateActiveUser(Reader["IsActive"]);

                    if ((!UserId.IsValid) || (!UserEmail.IsValid) || (!UserType.IsValid) || (!UserActive.IsValid))
                    {
                        return null;
                    }

                    User.Id = UserId.Value;
                    User.Email = UserEmail.Value;
                    User.UserType = UserType.Value;
                    User.IsActive = UserActive.Value;

                    Result<string?> UserFirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    if ((UserFirstName.IsValid) && (UserFirstName != null))
                    {
                        User.FirstName = UserFirstName.Value;
                    }
                    else
                    {
                        User.FirstName = null;
                    }

                    Result<string?> UserLastName = Helper.ToValidateUserName(Reader["LastName"]);
                    if ((UserLastName.IsValid) && (UserLastName != null))
                    {
                        User.LastName = UserLastName.Value;
                    }
                    else
                    {
                        User.LastName = null;
                    }

                    Result<DateTime?> UserBirthday = Helper.ToValidateUserBirthday(Reader["Birthday"]);
                    if ((UserBirthday.IsValid) && (UserBirthday != null))
                    {
                        User.Birthday = UserBirthday.Value;
                    }
                    else
                    {
                        User.Birthday = null;
                    }

                    Result<string?> UserProfilePicture = Helper.ToValidateProfilePicture(Reader["ProfilePicture"]);
                    if ((UserProfilePicture.IsValid) && (UserProfilePicture != null))
                    {
                        User.ProfilePicture = UserProfilePicture.Value;
                    }
                    else
                    {
                        User.ProfilePicture = null;
                    }

                    return User;

                }

                return null;
            }
            catch (Exception Ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(Ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (_data != null)
                {
                    await _data.DisposeAsync();
                }
            }
        }

        public async Task<bool> EmailExists(string Email)
        {
            _data.ToSetProcedure("EmailExists");

            _data.ToSetParameters("@Email", Email);

            try
            {
                SqlDataReader Reader = await _data.ToExecuteWithResult();

                if (await Reader.ReadAsync())
                {
                    Object? Value = Reader["ItExists"];

                    if (Value is bool Aux)
                    {
                        return Aux;
                    }

                    if (Value is byte B)
                    {
                        bool Convertion = (B == 1);
                        return Convertion;
                    }
                    throw new Exception("Unexpected value type received from the database.");

                }
                throw new Exception("No data returned from the stored procedure.");

            }
            catch (Exception Ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(Ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (_data != null)
                {
                    await _data.DisposeAsync();
                }
            }
        }


        public async Task<UserResponseDTO> Create(User User)
        {
            try
            {
                _data.ToSetProcedure("StoredToCreateUser");

                _data.ToSetParameters("@Email", User.Email);
                _data.ToSetParameters("@Password", User.Password);
                _data.ToSetParameters("@TypeOfUser", User.IsActive);

                var Reader = await _data.ToExecuteWithResult();


                while (await Reader.ReadAsync())
                {
                    Result<int> Id = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> Email = Helper.ToValidateString((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["TypeOfUser"]);
                    Result<bool> UserActive = Helper.ToValidateActiveUser(Reader["IsActive"]);
                    //Acá tengo que hacer un método que verifique que el usuario sea activo en mi Helper 


                    if ((!Id.IsValid) || (!Email.IsValid) || (!UserType.IsValid) || (UserType.Value == TypeOfUser.Invalid) || (!UserActive.IsValid))
                    {
                        return null;
                    }

                    UserResponseDTO Aux = new UserResponseDTO();
                    Aux.Id = Id.Value;
                    Aux.Email = Email.Value;
                    Aux.UserType = UserType.Value;
                    Aux.IsActive = UserActive.Value;

                    return Aux;
                }

                return null;
            }

            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (_data != null)
                {
                    await _data.DisposeAsync();
                }
            }
        }



        public async Task<UserResponseDTO> Login(User User)
        {
            _data.ToSetProcedure("StoredToLogin");

            _data.ToSetParameters("@Email", User.Email);
            _data.ToSetParameters("@Password", User.Password);

            try
            {
                SqlDataReader Reader = await _data.ToRead();

                while (await Reader.ReadAsync())
                {

                    Result<int> Id = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> Email = Helper.ToValidateString((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);
                    Result<bool> UserActive = Helper.ToValidateActiveUser(Reader["IsActive"]);

                    if ((!Id.IsValid) || (!Email.IsValid) || (!UserType.IsValid) || (UserType.Value == TypeOfUser.Invalid) || (!UserActive.IsValid))
                    {
                        return null;
                    }
                    UserResponseDTO Aux = new UserResponseDTO();

                    Aux.Id = Id.Value;
                    Aux.Email = Email.Value;
                    Aux.UserType = UserType.Value;
                    Aux.IsActive = UserActive.Value;

                    Result<string?> FirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    if ((FirstName.IsValid) && (FirstName != null))
                    {
                        Aux.FirstName = FirstName.Value;
                    }
                    else
                    {
                        Aux.FirstName = null;
                    }

                    Result<string?> LastName = Helper.ToValidateUserName(Reader["LastName"]);
                    if ((LastName.IsValid) && (LastName != null))
                    {
                        Aux.LastName = LastName.Value;
                    }
                    else
                    {
                        Aux.LastName = null;
                    }

                    Result<DateTime?> Birthday = Helper.ToValidateUserBirthday(Reader["Birthday"]);
                    if ((Birthday.IsValid) && (Birthday != null))
                    {
                        Aux.Birthday = Birthday.Value;
                    }
                    else
                    {
                        Aux.Birthday = null;
                    }

                    Result<string?> ProfilePicture = Helper.ToValidateProfilePicture(Reader["ProfilePicture"]);
                    if ((ProfilePicture.IsValid) && (ProfilePicture != null))
                    {
                        Aux.ProfilePicture = ProfilePicture.Value;
                    }
                    else
                    {
                        Aux.ProfilePicture = null;
                    }


                    return Aux;
                }

                return null;

            }
            catch (Exception Ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(Ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                await _data.DisposeAsync();
            }


        }


        public async Task<UserResponseDTO?> Patch(User User)
        {
            try
            {
                _data.ToSetProcedure("StoredPatchUser");


                if (User.Id != null)
                {
                    _data.ToSetParameters("@Id", User.Id);
                }
                else
                {
                    throw new ArgumentException("User's Id is missing and it's required");
                }


                if (User.FirstName != null)
                {
                    _data.ToSetParameters("@FirstName", User.FirstName);
                }
                else
                {
                    _data.ToSetParameters("@FirstName", DBNull.Value);
                }

                if (User.LastName != null)
                {
                    _data.ToSetParameters("@LastName", User.LastName);
                }
                else
                {
                    _data.ToSetParameters("@LastName", DBNull.Value);
                }

                if (User.Birthday != null)
                {
                    _data.ToSetParameters("@Birthday", User.Birthday);
                }
                else
                {
                    _data.ToSetParameters("@Birthday", DBNull.Value);
                }

                if (User.ProfilePicture != null)
                {
                    _data.ToSetParameters("@ProfilePicture", User.ProfilePicture);
                }
                else
                {
                    _data.ToSetParameters("@ProfilePicture", DBNull.Value);
                }


                var Reader = await _data.ToExecuteWithResult();

                UserResponseDTO UpdatedUser = null;
                while (await Reader.ReadAsync())
                {
                    Result<int> UserId = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> UserEmail = Helper.ToValidateUserEmail((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);
                    Result<bool> UserActive = Helper.ToValidateActiveUser(Reader["IsActive"]);


                    UpdatedUser = new UserResponseDTO();
                    if ((!UserId.IsValid) || (!UserEmail.IsValid) || (!UserType.IsValid) || (!UserActive.IsValid))
                    {
                        return null;
                    }

                    UpdatedUser.Id = UserId.Value;
                    UpdatedUser.Email = UserEmail.Value;
                    UpdatedUser.UserType = UserType.Value;
                    UpdatedUser.IsActive = UserActive.Value;

                    Result<string?> UserFirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    UpdatedUser.FirstName = ((UserFirstName.IsValid) && (UserFirstName.Value != null)) ? UserFirstName.Value : null;

                    Result<string?> UserLastName = Helper.ToValidateUserName(Reader["LastName"]);
                    UpdatedUser.LastName = ((UserLastName.IsValid) && (UserLastName.Value != null)) ? UserLastName.Value : null;

                    Result<DateTime?> UserBirthday = Helper.ToValidateUserBirthday(Reader["Birthday"]);
                    UpdatedUser.Birthday = ((UserBirthday.IsValid) && (UserBirthday.Value != null)) ? UserBirthday.Value : null;

                    Result<string?> UserProfilePicture = Helper.ToValidateProfilePicture(Reader["ProfilePicture"]);
                    UpdatedUser.ProfilePicture = ((UserProfilePicture.IsValid) && (UserProfilePicture.Value != null)) ? UserProfilePicture.Value : null;

                    return UpdatedUser;
                }

                //This is going to be null if the it doesn´t enter to the loop
                return UpdatedUser;

            }
            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (_data != null)
                {
                    await _data.DisposeAsync();
                }
            }
        }

        public async Task<UserResponseDTO>UpdateEmail(UserUpdateDTO User)
        {
            return null;
        }

    }
}
