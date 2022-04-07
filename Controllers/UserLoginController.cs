using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TODOLIST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TODOLIST.Repositories;

namespace JwtApp.Controllers;

[ApiController]
[Route("api/userlogin")]
public class UserLoginController : ControllerBase
{
    private IConfiguration _configuration;
    private readonly IUserRepository _User;
    public UserLoginController(IConfiguration configuration, IUserRepository User)
    {
        _configuration = configuration;
        _User = User;
    }

    [AllowAnonymous]
    [HttpPost]

    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        var User = await _User.FindEmail(userLogin.Email);
        if (User == null)
            return Unauthorized("Email  is Incorrect");
        if (User.Passcode != userLogin.Passcode)
            return Unauthorized("Password is Incorrect");
        var token = Generate(User);
        return Ok(token);

    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.Mobile.ToString()),

        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
