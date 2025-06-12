using My_Store.Models.UserModels;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure : IRepository<User,UserResponseDTO>, IUserRepository
    {
        private DataAccess _data;
        private DataAccess Data { get { return this._data; } set { this._data = value; } }

        public UserInfrastructure()
        {
            Data = new DataAccess();
        }

        public async Task<UserResponseDTO?>GetById(int Id)
        {
            Data.ToSetProcedure("GetById");
            Data.ToSetParameters("@Id", Id);

            try
            {
                SqlDataReader Reader =await  Data.ToRead();

                while (await Reader.ReadAsync())
                {
                    UserResponseDTO User = new UserResponseDTO();


                    Result<int> UserId = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> UserEmail = Helper.ToValidateUserEmail((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);

                    if((!UserId.IsValid)||(!UserEmail.IsValid)|| (!UserType.IsValid))
                    {
                        return null;
                    }

                    User.Id = UserId.Value;
                    User.Email = UserEmail.Value;
                    User.UserType = UserType.Value;

                    Result<string?> UserFirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    if((UserFirstName.IsValid)&& (UserFirstName != null))
                    {
                        User.FirstName = UserFirstName.Value;
                    }
                    else
                    {
                        User.FirstName = null;
                    }

                    Result<string?> UserLastName = Helper.ToValidateUserName(Reader["LastName"]);
                    if ((UserLastName.IsValid) && (UserLastName!=null))
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
                    if ((UserProfilePicture.IsValid) && (UserProfilePicture!=null))
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
            }catch(Exception Ex)
            {
                ErrorInfo Error=ErrorInfo.FromException(Ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if(Data != null)
                {
                    await Data.DisposeAsync();
                }
            }

            
        }
        public async Task<UserResponseDTO> Login(User User )
        {
            Data.ToSetProcedure("StoredToLogin");

            Data.ToSetParameters("@Email", User.Email);
            Data.ToSetParameters("@Password", User.Password);

            try
            {
                SqlDataReader Reader = await Data.ToRead();

                while (await Reader.ReadAsync())
                {

                    Result<int> Id = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> Email = Helper.ToValidateString((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);

                    if ( (!Id.IsValid)|| (!Email.IsValid) || (!UserType.IsValid) || (UserType.Value == TypeOfUser.Invalid) )
                    {
                        return null;
                    }
                    UserResponseDTO Aux = new UserResponseDTO();

                    Aux.Id = Id.Value;
                    Aux.Email = Email.Value;
                    Aux.UserType= UserType.Value;

                    return Aux;
                }

                return null;

            }catch (Exception Ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(Ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                await Data.DisposeAsync();
            }


        }
        public async Task<UserResponseDTO> Create  (User User)
        {
            try
            {
                int RegularUserType = 1;
                Data.ToSetProcedure("StoredToCreateUser");

                Data.ToSetParameters("@Email", User.Email);
                Data.ToSetParameters("@Password", User.Password);
                Data.ToSetParameters("@TypeOfUser", RegularUserType);

               var Reader= await Data.ToExecuteWithResult();
          

                while (await Reader.ReadAsync())
                {
                    Result<int> Id = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> Email = Helper.ToValidateString((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["TypeOfUser"]);


                    if ((!Id.IsValid)||(!Email.IsValid) || (!UserType.IsValid) || (UserType.Value == TypeOfUser.Invalid))
                    {
                        return null;
                    }

                    UserResponseDTO Aux = new UserResponseDTO();
                    Aux.Id = Id.Value;
                    Aux.Email = Email.Value;
                    Aux.UserType = UserType.Value;

                    return Aux; 

                }

                
                return  null;
            }

            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if(Data!= null)
                {
                    await Data.DisposeAsync();
                }
            }
        }

        public async Task<UserResponseDTO?>Patch(User User)
        {
            try
            {
                Data.ToSetProcedure("StoredPatchUser");
            

                if (User.Id != null)
                {
                    Data.ToSetParameters("@Id",User.Id); 
                }
                else
                {
                    throw new ArgumentException("User's Id is missing and it's required");
                }



                if (User.FirstName != null)
                {
                    Data.ToSetParameters("@FirstName", User.FirstName);
                }
                else
                {
                    Data.ToSetParameters("@FirstName",DBNull.Value);
                }

                if(User.LastName != null)
                {
                    Data.ToSetParameters("@LastName", User.LastName);
                }
                else
                {
                    Data.ToSetParameters("@LastName", DBNull.Value);
                }

                if(User.Birthday != null)
                {
                    Data.ToSetParameters("@Birthday",User.Birthday);
                }
                else
                {
                    Data.ToSetParameters("@Birthday", DBNull.Value);
                }

                if (User.ProfilePicture!=null)
                {
                    Data.ToSetParameters("@ProfilePicture", User.ProfilePicture);
                }
                else
                {
                    Data.ToSetParameters("@ProfilePicture", DBNull.Value);
                }

                var Reader = await Data.ToExecuteWithResult();

                UserResponseDTO UpdatedUser = null;
                while (await Reader.ReadAsync())
                {
                    Result<int> UserId = Helper.IsGreaterThanZero((int)Reader["Id"]);
                    Result<string> UserEmail = Helper.ToValidateUserEmail((string)Reader["Email"]);
                    Result<TypeOfUser> UserType = Helper.GetUserType((int)Reader["UserType"]);


                    UpdatedUser = new UserResponseDTO();
                    if((!UserId.IsValid) || (!UserEmail.IsValid) || (!UserType.IsValid))
                    {
                        return null;
                    }

                    UpdatedUser.Id=UserId.Value;
                    UpdatedUser.Email = UserEmail.Value;
                    UpdatedUser.UserType = UserType.Value;

                    Result<string?> UserFirstName = Helper.ToValidateUserName(Reader["FirstName"]);
                    UpdatedUser.FirstName = ((UserFirstName.IsValid) && (UserFirstName.Value != null))? UserFirstName.Value : null;

                    Result<string?> UserLastName = Helper.ToValidateUserName(Reader["LastName"]);
                    UpdatedUser.LastName = ((UserLastName.IsValid) && (UserLastName.Value != null))? UserLastName.Value : null;

                    Result<DateTime?> UserBirthday = Helper.ToValidateUserBirthday(Reader["Birthday"]);
                    UpdatedUser.Birthday = ((UserBirthday.IsValid) && (UserBirthday.Value != null)) ? UserBirthday.Value : null;

                    Result<string?> UserProfilePicture = Helper.ToValidateProfilePicture(Reader["ProfilePicture"]);
                    UpdatedUser.ProfilePicture = ((UserProfilePicture.IsValid) && (UserProfilePicture.Value != null)) ? UserProfilePicture.Value : null;

                    return UpdatedUser;
                }

                    //This is going to be null if the it doesn´t enter to the loop
                return UpdatedUser;

            }catch(Exception ex)
            {
                ErrorInfo Error=ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
            finally
            {
                if (Data != null)
                {
                    await Data.DisposeAsync();  
                }
            }
        }
        
    }
}
