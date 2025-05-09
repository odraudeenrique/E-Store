using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using My_Store.Shared.Helper;
using My_Store.Infrastructure.ErrorInfrastructure;
using My_Store.Shared.SecurityHelper;

namespace My_Store.Models.UserModels
{
    public class User
    {
        private string _id;
        public string Id { get { return this._id; } }

        private string _email;

        [Required] public string Email { get { return this._email; } set { this._email = value; } }
        private string _password;
        [Required] public string Password { get { return this._password; }  set { this._password = value; } }

        private string _firstname;
        public string Firstname { get { return this._firstname; } set { this._firstname = value; } }
        private string _lastName;
        public string LastName { get { return this._lastName; } set { this._lastName = value; } }
        private DateTime _birthday;
        public DateTime Birthday { get { return this._birthday; } set { this._birthday = value; } }
        private string _profilePicture;
        public string ProfilePicture { get { return this._profilePicture; } set { this._profilePicture = value; } }
        private TypeOfUser _userType;
        public TypeOfUser UserType { get { return this._userType; } set { this._userType = value; } }



        public User(string Email, string Password)
        {

            Result<(string Email, string Password)> ValidatedUserCredentials=Helper.ToValidateUserCredentials(Email, Password);

            if (!ValidatedUserCredentials.IsValid)
            {
                throw new ArgumentException("Empty Email, or Password");
            }
            Result<string> PasswordHash=SecurityHelper.ToGetPasswordHash(ValidatedUserCredentials.Value.Password);

            if (!PasswordHash.IsValid)
            {
                throw new ArgumentException("Hashing failed");
            }

            this.Email = ValidatedUserCredentials.Value.Email;
            this.Password = PasswordHash.Value;

        }  

        private Result<T> Create(string AuxEmail, string AuxPassword)
        {
            Result<(string Email, string Password)> ValidatedUserCredentials = Helper.ToValidateUserCredentials(Email, Password);

            if (!ValidatedUserCredentials.IsValid)
            {
                return Result<string>.Failed("The credentials are not valid");
            }
            Result<string> PasswordHash = SecurityHelper.ToGetPasswordHash(ValidatedUserCredentials.Value.Password);

            if (!PasswordHash.IsValid)
            {
                throw new ArgumentException("Hashing failed");
            }
            User NewUser = new User {
                this.Email = ValidatedUserCredentials.Value.Email;
                this.Password = ValidatedUserCredentials.Value.Password;
            };

            return Result<User>.Successful(NewUser);
        }
    }
}
