using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Shared.DB.Classes.User;
using Shared.Extensions;

namespace WebApp.Services;

public class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly ApiService _apiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _jwtTokenHandler;

    public AuthService(ILogger<AuthService> logger, ApiService apiService, IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _logger = logger;
        _apiService = apiService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _jwtTokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<bool> HasAccess(Guid userId, AccessLevel accessLevel)
    {
        var access = await CheckExistsAsync(userId);
        if (access is null)
        {
            return false;
        }

        return ((AccessLevel)access).HasFlag(accessLevel);
    }


    /// <summary>
    /// This func will authorize user and return the JwtToken
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<JwtSecurityToken?> GetJwtToken(string login, string password)
    {
        try
        {
            var userId = await AuthorizeAsync(login, password);
            if (userId == null || userId == Guid.Empty)
            {
                return null;
            }

            var token = _jwtTokenHandler.ReadJwtToken(
                GenerateJwtToken(userId.Value)
            );
            return token;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while get token in {this.GetJwtToken}");
            return null;
        }
    }

    public async Task RegisterJwtToken(IJSRuntime _jsRuntime, JwtSecurityToken token)
    {
        await SetJwtToken(_jsRuntime, token);
    }

    private async Task<uint?> CheckExistsAsync(Guid userId)
    {
        _logger.LogInformation($"Try get access for {userId} from {nameof(_apiService.GetUserAccessById)}");
        return await _apiService.GetUserAccessById(userId);
    }

    private async Task SetJwtToken(IJSRuntime _jsRuntime, JwtSecurityToken token)
    {
        await CookieInterop.SetJwtToken(_jsRuntime, _jwtTokenHandler.WriteToken(token));
        // Task.Delay(1500).Wait();
    }

    private async Task<Guid?> AuthorizeAsync(string username, string password)
    {
        var userId = await _apiService.AuthUser(username, password);
        _logger.LogInformation($"Get {userId} from {nameof(_apiService.AuthUser)}");
        return userId;
    }


    private string GenerateJwtToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JWTClaimNames.UserId, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}