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
    private Dictionary<Guid, uint> _usersData = new();
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

    public bool HasAccess(Guid userId, AccessLevel accessLevel)
    {
        if (!HasUser(userId)) return false;
        var access = (AccessLevel)_usersData[userId];
        return access.HasFlag(accessLevel) || access.HasFlag(AccessLevel.AllAccess);
    }

    private bool HasUser(Guid userId)
    {
        return _usersData.ContainsKey(userId);
    }

    public uint? GetAccess(JwtSecurityToken token)
        => _usersData.ContainsKey(token.GetGuidFromToken())
            ? _usersData[token.GetGuidFromToken()]
            : null;

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
        await CheckExistsAsync(token);
    }
    
    private async Task<bool> CheckExistsAsync(JwtSecurityToken? token)
    {
        if (token is null) return false;
        var userId = token.GetGuidFromToken();

        if (_usersData.ContainsKey(userId)) return true;

        _logger.LogInformation($"{userId} doesn't exist in {nameof(this._usersData)}");
        return await SyncAccessAsync(token);
    }

    private async Task SetJwtToken(IJSRuntime _jsRuntime, JwtSecurityToken token)
    {
        await CookieInterop.SetJwtToken(_jsRuntime, _jwtTokenHandler.WriteToken(token));
    }

    private async Task<Guid?> AuthorizeAsync(string username, string password)
    {
        var userId = await _apiService.AuthUser(username, password);
        _logger.LogInformation($"Get {userId} from {nameof(_apiService.AuthUser)}");
        return userId;
    }

    private async Task<bool> SyncAccessAsync(JwtSecurityToken token)
    {
        var userId = token.GetGuidFromToken();
        try
        {
            var access = await _apiService.GetUserAccessById(userId);
            if (access == null) return false;
            _usersData[userId] = access.Value;
            _logger.LogInformation($"Access for {userId} is {access}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception while add access to _usersData in {nameof(SyncAccessAsync)}");
        }

        return true;
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