using Microsoft.AspNetCore.Mvc;
using My_Store.Models.UserModels;
using My_Store.Services.UserServices;
using My_Store.Shared.Helper;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using My_Store.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace My_Store.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase        
    {
        private readonly IUserService _userService;

        public UserController(IUserService UserService)
        {
            _userService = UserService;
        }



        //GET api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll(int Page,int PageSize)
        {
            try
            {
                if((Page < 0)||(PageSize<0))
                {
                    return BadRequest("The page or the page size is not valid");
                }

                IEnumerable<UserResponseDTO> Users = await _userService.GetAll(Page,PageSize);

                if (!Users.Any())
                {
                    return NotFound("Not users found");
                }

                ActionResult Status = StatusCode(200, Users);
                return Status;

            }catch (Exception Ex)
            {
                ActionResult Status = StatusCode(500, $"An error has occoured: {Ex.Message}");
                return Status;
            }

        }

        //GET api/<UserController>
        [HttpGet("{Id}")]
        public async Task<ActionResult<UserResponseDTO>> GetById(int Id) 
        { 
            if( Id<=0)
            {
                ActionResult Status = BadRequest("The Id is null or not valid");
                return Status;  
            }

            try
            {
                UserResponseDTO? User = await _userService.GetById(Id);

                if(User == null)
                {
                    return  NotFound($"The user with the ID: {Id} was not found"); 
                }
                
                ActionResult Status = StatusCode(200, User);
                return Status;
            }catch(Exception Ex)
            {
                ActionResult Status = StatusCode(500, $"Internal Error: {Ex.Message}");
                return Status;  
            }


        }


        //GET api/<UserController>
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>>EmailExists([FromQuery] string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ActionResult Status = BadRequest("The Email is null or empty");
                return Status;
            }
            try
            {
                bool ItExists = await _userService.EmailExists(Email);

                ActionResult Status = null;

                if (!ItExists)
                {
                    Status = StatusCode(404,"The Email does not exists");
                    return Status;
                }

                Status = StatusCode(200, ItExists);
                return Status;
            }
            catch (Exception Ex)
            {
                ActionResult Status = StatusCode(500, $"Internal Error: {Ex.Message}");
                return Status;
            }
            
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Create([FromBody] UserCreateDTO User)
        {
            if(User== null)
            {
                ActionResult Status = BadRequest("The user is null");
                return Status;
            }

            if( (string.IsNullOrWhiteSpace(User.Email)) || (string.IsNullOrWhiteSpace(User.Password)) )
            {
                ActionResult Status = BadRequest("Missing required user fields");
                return Status;
            }

            try
            {
                UserResponseDTO NewUser = await _userService.Create(User);
                if (NewUser == null)
                {
                    return BadRequest("The user couldn't be created");
                }

                ActionResult Status = StatusCode(201,NewUser);
                return Status;
            }
            catch (Exception ex)
            {
                ActionResult Status = StatusCode(500, "Internal server error: " + ex.Message);
                return Status;
            }
        }

        // POST api/<UserController>
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseDTO>> Login([FromBody] UserCreateDTO User)
        {
            if (User == null)
            {
                ActionResult Status = BadRequest("The user is null");
                return Status;
            }


            try
            {           
                UserResponseDTO? LoggedUser=await _userService.Login(User);

                if(LoggedUser == null)
                {
                    return Unauthorized("One of the credentials is invalid");
                }

                ActionResult Status = StatusCode(200, LoggedUser);
                return Status;  



            }catch(Exception ex)
            {
                ActionResult Status= StatusCode(500, "Internal server error: " + ex.Message);
                return Status;
            }

        }


        //PATCH /api/<UserController>
        [HttpPatch]
        public async Task<ActionResult<UserResponseDTO?>> Patch([FromBody]UserUpdateDTO User)
        {
            if(User == null)
            {
                ActionResult Status = BadRequest("The user is null");
                return Status;
            }

            try
            {
                UserResponseDTO? UpdatedUser = await _userService.Patch(User); 
                
                if(UpdatedUser == null)
                {
                    return BadRequest("The user is not updated");
                }

                ActionResult Status= StatusCode(200, UpdatedUser);
                return Status;

            }
            catch(Exception Ex)
            {
                ActionResult Status = StatusCode(500, $"Internal server error: {Ex.Message}");
                return Status;
            }

        }


        //Tengo que agregar un procedimiento para modificar el email, y otro para modificar el tipo de usuario.

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
