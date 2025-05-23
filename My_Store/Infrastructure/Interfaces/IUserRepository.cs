using My_Store.Models.UserModels;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IUserRepository<User, UserResponseDTO>
    {
        Task<UserResponseDTO>Login(User User);
    }
}
