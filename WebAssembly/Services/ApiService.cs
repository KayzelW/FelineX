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

public class ApiService
{
    private HttpClient httpClient { get; }
    private ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime, ILogger<ApiService> logger)
    {
        this.httpClient = httpClient;
        _logger = logger;
        // SetupHttpClient(jsRuntime);
    }


    private async void SetupHttpClient(IJSRuntime jsRuntime)
    {
        var userAgent = httpClient.DefaultRequestHeaders.UserAgent;
        userAgent.TryParseAdd(await jsRuntime.InvokeAsync<string>("navigator.userAgent"));
        userAgent.TryParseAdd(await jsRuntime.InvokeAsync<string>("navigator.platform"));
        userAgent.TryParseAdd(await jsRuntime.InvokeAsync<string>("navigator.language"));
        userAgent.TryParseAdd(await jsRuntime.InvokeAsync<string>("Intl.DateTimeFormat().resolvedOptions().timeZone"));
        userAgent.Add(new ProductInfoHeaderValue($"WebAssembly Blazor"));
    }

    #region ForTests


    public async Task<bool> PostTest(MyTest test)
    {
        _logger.LogInformation("Got test in APIservice");
        var responseMessage = await httpClient.PostAsJsonAsync("Test/create_test", test);
        _logger.LogInformation("Send test to API");
        return responseMessage.IsSuccessStatusCode;
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

    #endregion


    #region ForTestAnswers

    public async Task<TestAnswer?> GetTestResult(Guid testAnswerId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_test_result/{testAnswerId}");
        TestAnswer? testAnswer = null;
        if (responseMessage.IsSuccessStatusCode)
        {
            testAnswer = await responseMessage.Content.ReadFromJsonAsync<TestAnswer>();
            var g = responseMessage.Content.ReadFromJsonAsAsyncEnumerable<TestAnswer>();
            Console.WriteLine(g);
        }
        
        return testAnswer;
    }

    public async Task<Guid> SubmitTest(MyTest? test)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("Test/submit_test", test);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task<List<TestAnswer>?> GetListStudentsTestAnswers(string testId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_list_students_test_answers/{testId}");
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<List<TestAnswer>>();

    }

    #endregion


    #region ForAuth
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
    
    #endregion
    
    #region ForClasses

    public async Task<List<UserGroup>?> GetClasses()
    {
        var responseMessage = await httpClient.GetAsync("Class/get_classes");
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<UserGroup>>();
        }

        return null;
    }

    #endregion
    public async void SendMessage(string msg)
    {
        await httpClient.PatchAsJsonAsync("User", msg);
    }
}