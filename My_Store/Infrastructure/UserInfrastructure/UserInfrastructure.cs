using My_Store.Models.UserModels;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Infrastructure.Interfaces;


namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure : IRepository<User>
    {
        private DataAccess _data;
        private DataAccess Data { get { return this._data; } set { this._data = value; } }

        public UserInfrastructure()
        {
            Data = new DataAccess();
        }


        public async Task<int> Create(User User)
        {
            Result<(string Email, string Password)> UserCredentials = Helper.ToValidateUserCredentials(User.Email, User.Password);

            
            if (!UserCredentials.IsValid)
            {
                const int InvalidValue = -1;
                return InvalidValue;
            }

            try
            {
                Data.ToSetProcedure("StoredToCreateUser");
                Data.ToSetParameters("@Email", UserCredentials.Value.Email);
                Data.ToSetParameters("@Password", UserCredentials.Value.Password);


                int UserStatus = await Data.ToExecuteScalarInt();
                return UserStatus;
            }
            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }
        }
        public void Update(User User)
        {

        }
    }
}
