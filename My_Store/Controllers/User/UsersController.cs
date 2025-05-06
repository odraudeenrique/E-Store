using Microsoft.AspNetCore.Mvc;
using My_Store.Models.UserModels;
using My_Store.Services.UserServices;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace My_Store.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IService<UserDTO> _userService;

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
        public async Task<IActionResult> Post()
        {
            using var Reader = new StreamReader(Request.Body);
            var Body = await Reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(Body))
            {
                return BadRequest("The user is null");
            }


            UserDTO Aux = new UserDTO();

            try
            {
                Aux = JsonSerializer.Deserialize<UserDTO>(Body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }catch(JsonException ex)
            {
                return BadRequest("Invalid JSON format");
            }

            if((Aux==null)|| (string.IsNullOrEmpty(Aux.Email)) || (string.IsNullOrEmpty(Aux.Password)))
            {
                return BadRequest("Missing required user fields");
            }



            try
            {
                await _userService.Create(Aux);
                return StatusCode(201, "User created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
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
