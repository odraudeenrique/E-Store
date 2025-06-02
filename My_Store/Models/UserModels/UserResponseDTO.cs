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


        //Optional fields
        private string _firstname;
        public string?Firstname { get { return this._firstname; } set { this._firstname = value; } }
        private string _lastName;
        public string? LastName { get { return this._lastName; } set { this._lastName = value; } }
        private DateTime _birthday;
        public DateTime? Birthday { get { return this._birthday; } set { this._birthday = value; } }
        private string _profilePicture;
        public string? ProfilePicture { get { return this._profilePicture; } set { this._profilePicture = value; } }
    }
}
