using My_Store.Shared;
using My_Store.Models.UserModels    ;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.DataAccessInfrastructure;
using My_Store.Infrastructure.ErrorInfrastructure;


namespace My_Store.Infrastructure.UserInfrastructure
{
    public class UserInfrastructure
    {
        private DataAccess _data;
        DataAccess data = new DataAccess();
        private DataAccess Data { get { return _data; } set { _data = value; } }

        public UserInfrastructure()
        {
            Data = new DataAccess();
        }


        public void CreateUser(string Email, string Pass)
        {
            Result<string> AuxEmail = Helper.ToValidateIfStringValid(Email);
            Result<string> AuxPassword = Helper.ToValidateIfStringValid(Pass);

            if (!AuxEmail.IsValid || !AuxPassword.IsValid)
            {
                return;
            }

            try
            {
                Data.ToSetProcedure("StoredToCreateUse");
                Data.ToSetParameters("@Email", AuxEmail.Value);
                Data.ToSetParameters("@Password", AuxPassword.Value);


                string UserStatus = Data.ToExecuteScalarString();
            }
            catch (Exception ex)
            {
                ErrorInfo Error = ErrorInfo.FromException(ex);
                ErrorLogger.LogToDatabase(Error);
                throw;
            }

        }
    }
}
