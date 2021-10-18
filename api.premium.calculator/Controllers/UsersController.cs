using api.premium.calculator.Common;
using api.premium.calculator.Models;
using api.premium.calculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.premium.calculator.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
