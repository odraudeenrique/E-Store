using My_Store.Shared;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.ErrorInfrastructure;

namespace My_Store.Models.UserModels
{
    public class User
    {
        private string _id;
        public string Id { get { return _id; } }

        private string _email;

        [Required] public string Email { get { return _email; } set { _email = value; } }
        private string _password;
        [Required] private string Password { get { return _password; }  set { _password = value; } }

        private string _firstname;
        public string Firstname { get { return _firstname; } set { _firstname = value; } }
        private string _lastName;
        public string LastName { get { return _lastName; } set { _lastName = value; } }
        private DateTime _birthday;
        public DateTime Birthday { get { return _birthday; } set { _birthday = value; } }
        private string _profilePicture;
        public string ProfilePicture { get { return _profilePicture; } set { _profilePicture = value; } }
        private TypeOfUser _userType;
        public TypeOfUser UserType { get { return _userType; } set { _userType = value; } }



        public User(string Email, string Password)
        {
            Result<string> AuxEmail = Helper.ToValidateIfStringValid(Email);
            Result<string> AuxPassword = Helper.ToValidateIfStringValid(Password);

            if (!AuxEmail.IsValid || !AuxPassword.IsValid)
            {
                throw new ArgumentException();
            }

            this.Email = Email;
            this.Password = ToSetPasswordHash(Password);

        }


        public string ToSetPasswordHash(string Pass)
        {
            Result<string> Aux = Helper.ToValidateIfStringValid(Pass);

            if (!Aux.IsValid)
            {
                return "";
            }

            string Hash = ToGetdHash(Aux.Value);
            return !string.IsNullOrEmpty(Hash) ? Hash : "";
        }

        public string ToGetdHash(string Pass)
        {
            Result<string> Aux = Helper.ToValidateIfStringValid(Pass);


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
