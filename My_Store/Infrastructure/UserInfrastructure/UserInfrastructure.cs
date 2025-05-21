using My_Store.Models.UserModels;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;

namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure : IRepository<User,UserResponseDTO>
    {
        private DataAccess _data;
        private DataAccess Data { get { return this._data; } set { this._data = value; } }

        public UserInfrastructure()
        {
            Data = new DataAccess();
        }


        public async Task<UserResponseDTO> Create  (User Aux)
        {
            try
            {
                int RegularUserType = 1;
                Data.ToSetProcedure("StoredToCreateUser");
                Data.ToSetParameters("@Email", Aux.Email);
                Data.ToSetParameters("@Password", Aux.Password);
                Data.ToSetParameters("@TypeOfUser", RegularUserType);

               var Reader= await Data.ToExecuteWithResult();
               UserResponseDTO UserDTO = new UserResponseDTO();
                while (Reader.Read())
                {
                    const string InvalidValue = "";
                    const int InvalidIntegerValue = -1;
                    
                    UserDTO.Email = !string.IsNullOrWhiteSpace(((string)Reader["Email"]))? ((string)Reader["Email"]):InvalidValue;
                    //UserDTO.Password = !string.IsNullOrWhiteSpace(((string)Reader["Password"]))?((string)Reader["Password"]):InvalidValue;
                    UserDTO.UserType = ((int)Reader["TypeOfUser"]) > 0 ? ((TypeOfUser)Reader["TypeOfUser"]) : TypeOfUser.Invalid;
                }

                
                return  UserDTO;
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
