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
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public TypeOfUser UserType { get; set; }    

        public string? FirstName {  get; set; } 
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ProfilePicture { get; set; } 
        public bool IsActive {  get; set; }  
        

        public User()
        {

        }


        //I need to work on the CreateAdmin 
        public static Result<User> Create(string AuxEmail, string AuxPassword)
        {

            if ((string.IsNullOrWhiteSpace(AuxEmail)) || (string.IsNullOrWhiteSpace(AuxPassword)))
            {
                return Result<User>.Failed("The credentials are not valid");
            }

            User NewUser = new User
            {
                Email = AuxEmail,
                Password = AuxPassword,
                UserType=TypeOfUser.Regular,
                IsActive = true
            };

            return Result<User>.Successful(NewUser);
        }

      
    }
}
