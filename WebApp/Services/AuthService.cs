using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Shared.Extensions;
using LoginRequest = Shared.SupportClasses.LoginRequest;

namespace WebApp.Services;

public class AuthService
{
    private Dictionary<Guid, uint> _usersData = new();
    private readonly ILogger<AuthService> _logger;
    private readonly ApiService _apiService;
    private readonly HttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly JSRuntime _jsRuntime;

    public AuthService(ILogger<AuthService> logger, ApiService apiService, HttpContextAccessor httpContextAccessor,
        IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler, JSRuntime jsRuntime)
    {
        _logger = logger;
        _apiService = apiService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        _jsRuntime = jsRuntime;
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

            var token = _jwtSecurityTokenHandler.ReadJwtToken(
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

    public async Task RegisterJwtToken(JwtSecurityToken token)
    {
        await SetJwtToken(token);
        await CheckExistsAsync(token);
    }

    private async Task SetJwtToken(JwtSecurityToken token)
    {
        await CookieInterop.SetJwtToken(_jsRuntime, _jwtSecurityTokenHandler.WriteToken(token));
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

    public async Task<bool> CheckExistsAsync(JwtSecurityToken token)
    {
        var userId = token.GetGuidFromToken();

        if (_usersData.ContainsKey(userId)) return true;

        _logger.LogInformation($"{userId} doesn't exist in {nameof(this._usersData)}");
        return await SyncAccessAsync(token);
    }

    private string GenerateJwtToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}