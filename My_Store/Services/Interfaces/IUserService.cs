using My_Store.Models.UserModels;

namespace My_Store.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Create(UserCreateDTO User);
        Task<IEnumerable<UserResponseDTO>> GetAll(int Page, int PageSize);
        Task<UserResponseDTO?> GetById(int Id);
        Task<UserResponseDTO> Login(UserCreateDTO User);
        Task<UserResponseDTO?> Patch(UserUpdateDTO User);
        Task<bool> EmailExists(string Email);
        Task<UserResponseDTO> UpdateEmail(UserUpdateDTO User);
    }
}
