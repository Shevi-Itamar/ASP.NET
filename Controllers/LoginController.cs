using Microsoft.AspNetCore.Mvc;
using lessson1.Models;
using lessson1.Interfaces;
using lessson1.Services;


namespace lessson1.Controllers
{
    [ApiController]
[Route("Login")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public LoginController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {   
        var user = _userService.Authenticate(request.Name, request.Password);
        if (user == null)
            return StatusCode(403, "Forbidden: User not found or incorrect credentials");

        var token = UserTokenService.GenerateToken(user.Id, user.Name, user.Permission, _configuration);
        
        return Ok(new { token });
    }
}

}
