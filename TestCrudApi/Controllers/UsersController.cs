using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCrudApi.DTOs;
using TestCrudApi.Services;

namespace TestCrudApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        { 
            _userService = userService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(UsuariosDTO usuario)
        {
            return Ok(await _userService.Create(usuario));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            return Ok(await _userService.Login(email, password));
        }

        [HttpPut]
        public async Task<IActionResult> Put(UsuariosDTO usuario)
        {
            return Ok(await _userService.Update(usuario));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _userService.DeleteById(id));
        }
    }
}
