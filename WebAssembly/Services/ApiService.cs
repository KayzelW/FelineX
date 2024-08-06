using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models;
using MyTest = Shared.DB.Classes.Test.Test;

namespace WebAssembly.Services;

public class ApiService
{
    private HttpClient httpClient { get; }
    
    public ApiService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        this.httpClient = httpClient;
        var token = localStorageService.GetItemAsync(JwtExtensions.JwtCookieName).ConfigureAwait(false).ToString();
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        Console.WriteLine(token);
    }
    
    public async Task<List<MyTest>> GetTests()
    {
        List<MyTest>? tests = null;
        var responseMessage = await httpClient.GetAsync("Test/get_tests");
        if (responseMessage.IsSuccessStatusCode)
        {
            tests = await responseMessage.Content.ReadFromJsonAsync<List<MyTest>>();
        }

        return tests;
    }

    public async Task<TestDTO?> GetTest(string testId)
    {
        TestDTO? test = null;
        var responseMessage = await httpClient.GetAsync($"Test/get_test/{testId}");
        if (responseMessage.IsSuccessStatusCode)
        {
            test = await responseMessage.Content.ReadFromJsonAsync<TestDTO>();
        }

        return test;
    }

    public async Task<TestAnswer?> GetTestResult(Guid testAnswerId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_test_result/{testAnswerId}");
        TestAnswer? testAnswer = null;
        if (responseMessage.IsSuccessStatusCode)
        {
            testAnswer = await responseMessage.Content.ReadFromJsonAsync<TestAnswer>();
        }

        return testAnswer;
    }

    public async Task<bool> PostTest(MyTest test)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("Test/create_test", test);
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<Guid> SubmitTest(MyTest? test)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("Test/submit_test", test);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<Guid>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns>Token as a string</returns>
    public async Task<AuthAnswer?> AuthUser(string login, string password)
    {
        var hash = await UserExtensions.HashPasswordAsync(password);

        Console.WriteLine($"User: |{login}| - |{hash}|"); //TODO: REMOVE
        var responseMessage = await httpClient.PostAsJsonAsync("Auth/auth", new AuthData
        {
            Login = login,
            HashedPassword = WebUtility.UrlEncode(hash)
        });
        if (!responseMessage.IsSuccessStatusCode) return null;

        var data = await responseMessage.Content.ReadFromJsonAsync<AuthAnswer>();
        Console.WriteLine($"Got |{data?.UserToken} - {data?.Access} - {data?.UserName}| from Auth/auth");
        return data;
    }

    public async Task<AuthAnswer?> AuthUserByToken(string token)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("Auth/authtoken", token);
        if (!responseMessage.IsSuccessStatusCode) return null;
        var data = await responseMessage.Content.ReadFromJsonAsync<AuthAnswer>();
        Console.WriteLine($"Got |{data?.UserToken} - {data?.Access} - {data?.UserName}| from Auth/auth");
        return data;
    }

    public async void SendMessage(string msg)
    {
        await httpClient.PatchAsJsonAsync("User", msg);
    }

    public async Task<List<TestAnswer>?> GetListStudentsTestAnswers(string testId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_list_students_testanswers/{testId}");
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<List<TestAnswer>>();
    }
}