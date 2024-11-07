using Microsoft.AspNetCore.Mvc;
using Services;
using Core;
using Microsoft.AspNetCore.Identity.Data;

namespace Api.Controllers{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase{
        private readonly AuthService _authService;
        private readonly UserService _userService;
        public AuthController(AuthService authService, UserService userService){
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request){
            var user = _userService.AuthenticateUser(request.Name, request.Password);
            if (user == null){
                return Unauthorized("Invalid credentials");
            }
            var token = _authService.GenerateJwtToken(user);
            return Ok(new {Token = token});
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] int id){
            if(!_authService.Logout(id)) return NotFound("User not found");
            return Ok("User logged out.");
        }
    }
}