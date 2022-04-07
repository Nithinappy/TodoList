
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOLIST.DTOs;
using TODOLIST.Models;
using TODOLIST.Repositories;

namespace TODOLIST.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _User;
    private readonly ITodoRepository _todo;

    public UserController(ILogger<UserController> logger, IUserRepository User, ITodoRepository todo)
    {
        _logger = logger;
        _User = User;
        _todo = todo;

    }


    [HttpGet("{email}")]
    // [Authorize]
    public async Task<ActionResult<UserDTO>> GetUserByEmail([FromRoute] string email)
    {
        UserDTO UserDTO = new UserDTO();
        var User = await _User.FindEmail(email);

        if (User is null)
            return NotFound("No User found with given User Email");

        UserDTO = User.asDto;
        UserDTO.MyTodos = (await _todo.GetUserTodoById(User.Id)).Select(x => x.asDto).ToList();
        return Ok(UserDTO);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {
        var toCreateUser = new User
        {
            UserName = Data.UserName.Trim(),
            Email = Data.Email.Trim(),
            Passcode = Data.Passcode,
            Mobile = Data.Mobile
        };

        var createdUser = await _User.CreateUser(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDto);
    }


    private User GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaims = identity.Claims;

            return new User
            {
                UserName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                // Mobile = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value,
            };
        }
        return null;
    }

}
