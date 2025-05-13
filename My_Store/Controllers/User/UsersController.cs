using Microsoft.AspNetCore.Mvc;
using My_Store.Models.UserModels;
using My_Store.Services.UserServices;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace My_Store.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IService<UserCreateDTO, UserResponseDTO> _userService;

        public UsersController()
        {
            _userService = new UserService();
        }


        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Post()
        {
            using var Reader = new StreamReader(Request.Body);
            var Body = await Reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(Body))
            {
                ActionResult Status = BadRequest("The user is null");
                return Status;
            }


            UserCreateDTO Aux = new UserCreateDTO();

            try
            {
                Aux = JsonSerializer.Deserialize<UserCreateDTO>(Body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                ActionResult Status = BadRequest("Invalid JSON format");
                return Status;
            }

            if ((Aux == null) || (string.IsNullOrEmpty(Aux.Email)) || (string.IsNullOrEmpty(Aux.Password)))
            {
                ActionResult Status = BadRequest("Missing required user fields");
                return Status;
            }



            try
            {
                UserResponseDTO NewUser = await _userService.Create(Aux);
                ActionResult Status = StatusCode(201, "User created successfully");

                return Status;
            }
            catch (Exception ex)
            {
                ActionResult Status = StatusCode(500, "Internal server error: " + ex.Message);
                return Status;
            }
        }





        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
