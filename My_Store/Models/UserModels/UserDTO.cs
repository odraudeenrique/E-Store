namespace My_Store.Models.UserModels
{
    public class UserDTO
    {
        public string _email;
        public string Email { get { return this._email; } set { this._email = value; } }
        private string _password;   
        public string Password { get{ return this._password; } set { this._password = value; } }
    }
}
