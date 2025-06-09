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
        private readonly IService<UserCreateDTO, UserResponseDTO> _userService;

        public UserController()
        {
            _userService = new UserService();
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
                IUserService _IUserService = new UserService();
               

                UserResponseDTO LoggedUser=await  _IUserService.Login(User);

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
                IUserService _IUserService=new UserService();
                UserResponseDTO? UpdatedUser = await _IUserService.Patch(User); 
                
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




        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
