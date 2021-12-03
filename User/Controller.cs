using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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


    /// <summary>Get information about the user</summary>
    /// <response code="200">Information about the user</response>
    [HttpPost("who-am-i")]
    [Authorize]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var currentUserId = Guid.Parse(subClaim.Value);

        var currentUser = await _context.Users.SingleAsync(user => user.Id == currentUserId);

        var readDto = new ReadUserDto
        {
            Id = currentUser.Id,
            DisplayName = currentUser.DisplayName,
            Username = currentUser.Username
        };

        return Ok(readDto);
    }


    /// <summary>Register a new user</summary>
    /// <param name="registerDto">User registration information.</param>
    /// <response code="200">Registration was successful.</response>
    /// <response code="409">Failed to register a new user: username was already taken.</response>
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

    /// <summary>Authenticate the user</summary>
    /// <param name="authenticateDto">User authentication information</param>
    /// <response code="200">Authentication token pair for specified user credentials.</response>
    /// <response code="404">User with specified username does not exists.</response>
    /// <response code="409">Incorrect password was specified.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
        var encodeAccessToken = tokenHandler.WriteToken(accessToken);

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationTime = DateTime.UtcNow.AddDays(refreshTokenLifetime)
        };

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("jti", refreshTokenEntity.Id.ToString())
            }),
            Expires = refreshTokenEntity.ExpirationTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
        var encodedRefreshToken = tokenHandler.WriteToken(refreshToken);

        var tokenPairDto = new TokenPairDto
        {
            AccessToken = encodeAccessToken,
            RefreshToken = encodedRefreshToken
        };

        return Ok(tokenPairDto);
    }
}
