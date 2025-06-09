using My_Store.Models.UserModels;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDTO>Login(User User);
        Task<UserResponseDTO?>Patch(User User);
    }
}
