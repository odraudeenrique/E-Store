namespace My_Store.Models.UserModels
{
    public class UserResponseDTO
    {
        public string _email;
        public string Email { get { return this._email; } set { this._email = value; } }
        private string _password;
        public string Password { get { return this._password; } set { this._password = value; } }
        private TypeOfUser _userType;
        public TypeOfUser UserType { get { return this._userType; } set { this._userType = value; } }
    }
}
