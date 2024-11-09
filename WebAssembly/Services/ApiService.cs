using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Shared.DB.Test.Answers;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models;
using MyTest = Shared.DB.Test.Test;
using MyTask = Shared.DB.Test.Task.Task;

namespace WebAssembly.Services;

public class ApiService
{
    private HttpClient httpClient { get; }
    private ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        this.httpClient = httpClient;
        _logger = logger;
    }

    #region ForTests


    public async Task<bool> PostTest(MyTest test, bool forEdit = false)
    {
        if (forEdit)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("Test/edit_test", test);
            return responseMessage.IsSuccessStatusCode;
        }
        else
        {
            var responseMessage = await httpClient.PostAsJsonAsync("Test/create_test", test);
            return responseMessage.IsSuccessStatusCode;
        }
        
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

    public async Task<TestDTO?> GetTest(string testId, bool AsOriginal = false)
    {
        TestDTO? test = null;
        if (AsOriginal)
        {
            var responseMessage = await httpClient.GetAsync($"Test/get_original_test/{testId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                test = await responseMessage.Content.ReadFromJsonAsync<TestDTO>();
            }
        }
        else
        {
            var responseMessage = await httpClient.GetAsync($"Test/get_test_for_solving/{testId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                test = await responseMessage.Content.ReadFromJsonAsync<TestDTO>();
            }
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
        }
        return testAnswer;
    }

    public async Task<double?> GetTestScore(Guid testAnswerId)
    {
        var responseMessage = await httpClient.GetAsync($"Test/get_test_score/{testAnswerId}");
        double? testAnswerScore = null;
        if (responseMessage.IsSuccessStatusCode)
        {
            testAnswerScore = await responseMessage.Content.ReadFromJsonAsync<double>();
        }
        return testAnswerScore;
        
    }

    public async Task<Guid> SubmitTest(TestDTO? test)
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

    public async Task<bool> AddGroup(UserGroup? group)
    {
        var responseMessage = await httpClient.PostAsJsonAsync("Class/add_group", group);
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<List<User>?> GetStudents()
    {
        var responseMessage = await httpClient.GetAsync("Class/get_students");
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<User>>();
        }

        return null;
    }
    
    #endregion

    #region Other

    public async void SendMessage(string msg)
    {
        await httpClient.PatchAsJsonAsync("User", msg);
    }

    #endregion
    
    
    
}
