using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AspFromScratch.WebApi.User;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;

    public UserController(DatabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    [HttpPost("register")]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserDto registerDto)
    {
        var usernameTaken = await _context.Users.AnyAsync(user => user.Username == registerDto.Username);


        if (usernameTaken)
        {
            return Conflict("Username already taken.");
        }

        var newUser = new UserEntity
        {
            Id = Guid.NewGuid(),
            DisplayName = registerDto.DisplayName,
            Username = registerDto.Username,
            Password = registerDto.Password
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var readDto = new ReadUserDto
        {
            Id = newUser.Id,
            DisplayName = newUser.DisplayName,
            Username = newUser.Username
        };
        return Ok(readDto);
    }

    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserDto authenticateDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == authenticateDto.Username);
        if (user is null)
        {
            return NotFound();
        }

        if (BCrypt.Net.BCrypt.Verify(authenticateDto.Password, user.Password))
        {
            return Conflict("Incorrect password");
        }

        // Prepare token info
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecret = Encoding.ASCII.GetBytes(_configuration["JwtAuth:Secret"]);
        var accessTokenLifetime = int.Parse(_configuration["JwtAuth:AccessTokenLifetime"]);
        var refreshTokenLifetime = int.Parse(_configuration["JwtAuth:RefreshTokenLifetime"]);

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sub", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(accessTokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }
}
