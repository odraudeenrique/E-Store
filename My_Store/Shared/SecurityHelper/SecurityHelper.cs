using System.Security.Cryptography;
using My_Store.Infrastructure.ErrorInfrastructure;
using System.Text;
using My_Store.Shared.Helper;

namespace My_Store.Shared.SecurityHelper
{
    public static class SecurityHelper
    {
        public static Result<string> ToGetPasswordHash(string Password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] OriginalPasswordToHash = Encoding.UTF8.GetBytes(Password);
                    byte[] HashPassword = sha256.ComputeHash(OriginalPasswordToHash);
                    string Hash= Convert.ToBase64String(HashPassword);
                    if (string.IsNullOrWhiteSpace(Hash))
                    {
                        return Result<string>.Failed("The hashing convertion failed");
                    }

                    return Result<string>.Successful(Hash);
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

