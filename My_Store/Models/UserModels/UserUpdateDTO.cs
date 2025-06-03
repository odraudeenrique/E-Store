using System.ComponentModel.DataAnnotations;

namespace My_Store.Models.UserModels
{
    public class UserUpdateDTO
    {
        [Required]  
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public TypeOfUser UserType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
