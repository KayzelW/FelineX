using System.Diagnostics;
using System.Text.Json;
using Shared.DB.Classes.User;
using Shared.Extensions;
using MyTest = Shared.DB.Classes.Test.Test;

namespace WebApp.Services;

public class ApiService
{
    private HttpClient _httpClient;
    private CookieManagerService _cookieManager;
    HttpContext httpContext = new DefaultHttpContext();

    public ApiService(IConfiguration configuration, CookieManagerService cookieManager)
    {
        _cookieManager = cookieManager;
        var baseUrl = configuration?.GetConnectionString("ApiUrl");

        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl ?? "http://26.96.214.129:5071/")
        };
    }

    public async Task<List<MyTest>> GetTests()
    {
        List<MyTest>? tests = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("Test/get_tests");
        if (responseMessage.IsSuccessStatusCode)
        {
            tests = await responseMessage.Content.ReadFromJsonAsync<List<MyTest>>();
        }
        return tests;
    }
    
    public async Task<MyTest> GetTest(string testId)
    {
        MyTest? test = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"Test/get_test/{testId}");
        if (responseMessage.IsSuccessStatusCode)
        {
            test = await responseMessage.Content.ReadFromJsonAsync<MyTest>();
        }
        return test;
    }

    public async Task<User?> GetUser()
    {
        User? user = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("User/get_user");
        if (responseMessage.IsSuccessStatusCode)
        {
            user = await responseMessage.Content.ReadFromJsonAsync<User>();
        }
        return user;
    } 
    
    public async Task<bool> PostTest(MyTest test)
    {
        var user = await GetUser();
        test.CreatorId = user!.Id; //TODO: Failed if user == null && can't add a new test with multiply exception like problems with Foreign key
        if (test.Tasks is not null)
        {
            foreach (var task in test.Tasks)
            {
                task.CreatorId = test.CreatorId;
            }
        }
        
        var responseMessage = await _httpClient.PostAsJsonAsync("Test/create_test", test);
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> Auth_user(string login, string password)
    {
        var hash = UserExtensions.HashPassword(password);
        var responseMessage = await _httpClient.PostAsJsonAsync("User/auth", (login, hash));
        var user = await responseMessage.Content.ReadFromJsonAsync<(bool success, string userId)>();
        if (!user.success) return false;
        _cookieManager.SetUserIdCookie(httpContext, user.userId);
        return true;

    }

}