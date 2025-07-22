namespace My_Store.Models.UserModels
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public TypeOfUser UserType { get; set; } 
        public string? FirstName {  get; set; } 
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive {  get; set; } 

    }
}
