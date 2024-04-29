namespace WebApp.Services;

public class AuthService 
{
    private Dictionary<string, uint?> _usersData = new();
    private readonly ILogger<AuthService> _logger;
    private readonly ApiService _apiService;

    public AuthService(ILogger<AuthService> logger, ApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var userId = await _apiService.AuthUser(username, password);
        if (userId != null)
        {
            await SyncAccessAsync(userId);
        }
        return userId;
    }

    private async Task<bool> SyncAccessAsync(string userId)
    {
        var access = await _apiService.GetUserAccessById(userId);
        if (access == null) return false;
        _usersData[userId] = access;
        return true;
    }

    public async Task<bool> CheckExistsAsync(string? userId)
    {
        if (userId == null)
        {
            return false;
        }

        if (_usersData.ContainsKey(userId)) return true;
        
        return await SyncAccessAsync(userId);
        
    }
    

}