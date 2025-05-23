using My_Store.Models.UserModels;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure : IRepository<User,UserResponseDTO>, IUserRepository<User,UserResponseDTO>
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

                    if ( (!Id.IsValid)|| (!Email.IsValid) || (!UserType.IsValid) || (UserType.Value != TypeOfUser.Invalid) )
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
        //public void Update(User User)
        //{

        //}
    }
}
