﻿using Shared.DB.Classes.User;
using Shared.Extensions;
using MyTest = Shared.DB.Classes.Test.Test;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.Models;

namespace Desktop.Services;

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
        User? user = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"User/get_user/{userId}");
        if (responseMessage.IsSuccessStatusCode)
        {
            user = await responseMessage.Content.ReadFromJsonAsync<User>();
        }

        return user;
    }

    public async Task<bool> PostTest(MyTest test)
    {
        if (test.Tasks is not null && test.CreatorId is not null)
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
        var testAnswerId = await responseMessage.Content.ReadFromJsonAsync<string>();
        return testAnswerId.ToGuid();
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

    public async Task<List<TestAnswer>> GetListStudentsTestAnswers(string testId)
    {
        var responseMessage = await _httpClient.GetAsync($"Test/get_list_students_testanswers/{testId}");
        if (!responseMessage.IsSuccessStatusCode) return null;

        var testAnswers = await responseMessage.Content.ReadFromJsonAsync<List<TestAnswer>>();

        return testAnswers;
    }
}