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

                    //Tengo que arreglar lo que no se devuelve el usuario y además leer lo de http 
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

        public async Task<UserResponseDTO>Patch(User User)
        {
            Data.ToSetProcedure("StoredPatchUser");
            

            if (User.Id != null)
            {
                Data.ToSetParameters("Id",User.Id); 
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


            //Tengo que modificar el procedimiento para que me devuelva el usuario con sus datos actualizados 


            return null;
        }
        //public void Update(User User)
        //{

        //}
    }
}
