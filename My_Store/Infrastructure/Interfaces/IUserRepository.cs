using My_Store.Models.UserModels;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDTO> Create(User user);   
        Task<IEnumerable<UserResponseDTO>> GetAll(int Skip, int Take);
        Task<UserResponseDTO> GetById(int Id);
        Task<UserResponseDTO>Login(User User);
        Task<UserResponseDTO?>Patch(User User);
        Task<bool>EmailExists(string Email);
        Task<UserResponseDTO> UpdateEmail(UserUpdateDTO User);
    }
}
