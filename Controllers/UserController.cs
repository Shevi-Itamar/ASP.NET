using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lessson1.Models;
using lessson1.Interfaces;

namespace lessson1.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{



    private IUserService UserService;

    public UserController(IUserService userService)
    {
          this.UserService = userService;
    }

    private bool IsAdmin(HttpContext context)
    {
        return context.User.Claims.Any(c => c.Type =="Role" && c.Value == "Admin");
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<List<User>> GetAll() =>
        UserService.GetAll();

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    public ActionResult<User> Get(int id)
    {
        var user = UserService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public IActionResult Create(User newUser)
    { 
        UserService.Add(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    public IActionResult Update(int id, User newUser)
    {
        if (!IsAdmin(HttpContext) &&( newUser == null || newUser.Id != id) )
            return BadRequest("Invalid user data."); 
        var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdFromToken == null || userIdFromToken != id.ToString() && !IsAdmin(HttpContext))
        {
            return Forbid();
        }

        var existingUser = UserService.Get(id);
        if (existingUser is null)
            return NotFound();

        UserService.Update(newUser);
        return NoContent();
    }
[HttpDelete("{id}")]
[Authorize(Policy = "UserOrAdmin")]
public IActionResult Delete(int id)
{
    var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userRole = User.FindFirst("Role")?.Value;

    if (userIdFromToken == null || (userIdFromToken != id.ToString() && userRole != "Admin"))
    {
        return Forbid(); 
    }

    var oldUser = UserService.Get(id);
    if (oldUser is null)
        return NotFound();

    UserService.Delete(id);

    return Content(UserService.Count.ToString());
}


}
