using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Shared.DB.User;
using Shared.DB.Test.Answers;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models;
using MyTest = Shared.DB.Test.Test;

namespace WebAssembly.Services;

public class ApiService(HttpClient httpClient)
{
    // private HttpClient httpClient { get; } = httpClient;

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

    public async Task<bool> DeleteTest(Guid testId)
    {
        var responseMessage = await httpClient.DeleteAsync($"Test/delete_test/{testId}");
        return responseMessage.IsSuccessStatusCode;
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
    /// <param name="password">will hash</param>
    /// <returns><see cref="AuthAnswer"/></returns>
    public async Task<AuthAnswer?> AuthUser(string login, string password)
    {
        var hash = await UserExtensions.HashPasswordAsync(password);

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
        var responseMessage = await httpClient.PostAsJsonAsync("Auth/auth_token", token);
        if (!responseMessage.IsSuccessStatusCode) return null;
        var data = await responseMessage.Content.ReadFromJsonAsync<AuthAnswer>();
        Console.WriteLine($"Got |{data?.UserToken} - {data?.Access} - {data?.UserName}| from Auth/auth_token");
        return data;
    }

    public async Task<List<TestAnswer>?> GetListStudentsTestAnswers(string testId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_list_students_test_answers/{testId}");
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<List<TestAnswer>>();
    }
}