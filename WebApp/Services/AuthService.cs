namespace WebApp.Services;

public class AuthService
{
    private Dictionary<Guid?, uint?> _usersData = new();
    private readonly ILogger<AuthService> _logger;
    private readonly ApiService _apiService;

    public AuthService(ILogger<AuthService> logger, ApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public async Task<Guid?> AuthenticateAsync(string username, string password)
    {
        var userId = await _apiService.AuthUser(username, password);
        _logger.LogInformation($"Get {userId} from {nameof(_apiService.AuthUser)} at {nameof(this.AuthenticateAsync)}");

        if (userId != null)
        {
            await SyncAccessAsync(userId);
        }

        return userId;
    }

    private async Task<bool> SyncAccessAsync(Guid? userId)
    {
        var access = await _apiService.GetUserAccessById(userId);
        _logger.LogInformation($"Access for {userId} is {access}");

        if (access == null) return false;
        _usersData[userId] = access;

        _logger.LogInformation($"Access for {userId} add in {nameof(this._usersData)}");
        return true;
    }

    public async Task<bool> CheckExistsAsync(Guid? userId)
    {
        if (userId is null || userId == Guid.Empty)
        {
            return false;
        }

        if (_usersData.ContainsKey(userId)) return true;

        _logger.LogInformation($"{userId} doesn't exist in {nameof(this._usersData)}");
        return await SyncAccessAsync(userId);
    }
}