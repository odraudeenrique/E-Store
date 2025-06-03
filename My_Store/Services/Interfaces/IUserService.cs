using My_Store.Models.UserModels;

namespace My_Store.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Login(UserCreateDTO User);
        Task<UserResponseDTO> Update(UserUpdateDTO User);
    }
}
