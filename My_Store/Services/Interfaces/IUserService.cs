using My_Store.Models.UserModels;

namespace My_Store.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAll();
        Task<UserResponseDTO?> GetById(int Id);
        Task<UserResponseDTO> Login(UserCreateDTO User);
        Task<UserResponseDTO?> Patch(UserUpdateDTO User);
        Task<bool> EmailExists(string Email);
    }
}
