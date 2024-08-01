using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Shared.Models;
using MyTest = Shared.DB.Classes.Test.Test;

namespace WebAssembly.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly LocalStorageService _localStorageService;

    public ApiService(HttpClient httpClient, IMemoryCache memoryCache, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _localStorageService = localStorageService;
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

    public async Task<MyTest?> GetTest(string testId)
    {
        MyTest? test = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"Test/get_test/{testId}");
        if (responseMessage.IsSuccessStatusCode)
        {
            test = await responseMessage.Content.ReadFromJsonAsync<MyTest>();
        }

        return test;
    }

    public async Task<TestAnswer?> GetTestResult(Guid testAnswerId)
    {
        var responseMessage = await _httpClient.GetAsync($"Test/get_test_result/{testAnswerId}");
        TestAnswer? testAnswer = null;
        if (responseMessage.IsSuccessStatusCode)
        {
            testAnswer = await responseMessage.Content.ReadFromJsonAsync<TestAnswer>();
        }

        return testAnswer;
    }

    public async Task<User?> GetUser(Guid userId)
    {
        
        var responseMessage = await _httpClient.GetAsync($"User/get_user/{userId}");
        responseMessage.EnsureSuccessStatusCode();

        var user = await responseMessage.Content.ReadFromJsonAsync<User>();

        return user;
    }

    public async Task<bool> PostTest(MyTest test)
    {
        var userId = await _localStorageService.GetUserIdAsync();

        test.CreatorId = userId;
        //TODO: Failed if user == null && can't add a new test with multiply exception like problems with Foreign key
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

    public async Task<Guid> SubmitTest(MyTest? test)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("Test/submit_test", test);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<Guid>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns>Token as a string</returns>
    public async Task<string?> AuthUser(string login, string password)
    {
        var hash = await UserExtensions.HashPasswordAsync(password);
        
        Console.WriteLine($"User: |{login}| - |{hash}|"); //TODO: REMOVE
        var responseMessage = await _httpClient.PostAsJsonAsync("User/auth", new AuthData
        {
            Login = login,
            HashedPassword = WebUtility.UrlEncode(hash)
        });
        if (!responseMessage.IsSuccessStatusCode) return null;

        var token = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine($"Got |{token}| from User/auth");
        return token;
    }

    public async Task<uint?> GetUserAccessById(Guid? id)
    {
        if (id == null || id == Guid.Empty) return null;
        if (_memoryCache.TryGetValue(id, out uint access)) return access;
        
        var responseMessage = await _httpClient.GetAsync($"User/get_user_access_by_id/{id}");
        if (!responseMessage.IsSuccessStatusCode) return null;

        access = await responseMessage.Content.ReadFromJsonAsync<uint>();
        _memoryCache.Set(id, access);

        return access;
    }

    public async Task<List<TestAnswer>?> GetListStudentsTestAnswers(string testId)
    {
        var responseMessage = await _httpClient.GetAsync($"Test/get_list_students_testanswers/{testId}");
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<List<TestAnswer>>();
    }
}