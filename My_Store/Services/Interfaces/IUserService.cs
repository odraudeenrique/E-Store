namespace My_Store.Services.Interfaces
{
    public interface IUserService<UserCreateDTO,UserResponseDTO>
    {
        Task<UserResponseDTO> Login(UserCreateDTO User);
    }
}
