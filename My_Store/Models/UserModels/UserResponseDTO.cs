namespace My_Store.Models.UserModels
{
    public class UserResponseDTO
    {
        private int _id;
        public int Id { get { return this._id; } set{this._id = value; } }
        public string _email;
        public string Email { get { return this._email; } set { this._email = value; } }
 
        private TypeOfUser _userType;
        public TypeOfUser UserType { get { return this._userType; } set { this._userType = value; } }
    }
}
