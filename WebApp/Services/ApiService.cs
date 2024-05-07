using Shared.DB.Classes.User;
using Shared.Extensions;
using MyTest = Shared.DB.Classes.Test.Test;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace WebApp.Services;

public class ApiService
{
    private HttpClient _httpClient;

    public ApiService(IConfiguration configuration)
    {
        var baseUrl = configuration?.GetConnectionString("ApiUrl");

        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl!)
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
        test.CreatorId =
            user!.Id; //TODO: Failed if user == null && can't add a new test with multiply exception like problems with Foreign key
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

    public async Task<Guid?> AuthUser(string login, string password)
    {
        var hash = UserExtensions.HashPassword(password);
        var responseMessage = await _httpClient.PostAsJsonAsync("User/auth", new AuthData
        {
            Login = login,
            HashedPassword = WebUtility.UrlEncode(hash)
        });
        if (!responseMessage.IsSuccessStatusCode) return null;

        var userId = await responseMessage.Content.ReadFromJsonAsync<string>();
        if (!Guid.TryParse(userId, out var userGuid)) return null;

        return userGuid;
    }

    public async Task<uint?> GetUserAccessById(Guid? id)
    {
        if (id is null || id == Guid.Empty) return null;

        var responseMessage = await _httpClient.GetAsync($"User/get_user_access_by_id/{id}");
        if (!responseMessage.IsSuccessStatusCode) return null;

        var access = await responseMessage.Content.ReadFromJsonAsync<uint>();
        return access;
    }
}