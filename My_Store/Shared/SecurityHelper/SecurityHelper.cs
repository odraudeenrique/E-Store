using System.Security.Cryptography;
using My_Store.Infrastructure.ErrorInfrastructure;
using System.Text;
using My_Store.Shared.Helper;

namespace My_Store.Shared.SecurityHelper
{
    public static class SecurityHelper
    {
        public static string ToGetPasswordHash(string Password)
        {
            Result<string> Aux = Helper.ToValidateIfStringValid(Password);

            if (!Aux.IsValid)
            {
                return "";
            }

            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] OriginalPasswordToHash = Encoding.UTF8.GetBytes(Aux.Value);
                    byte[] HashPassword = sha256.ComputeHash(OriginalPasswordToHash);
                    return Convert.ToBase64String(HashPassword);
                }
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

