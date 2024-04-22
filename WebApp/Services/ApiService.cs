using System.Diagnostics;
using System.Text.Json;
using Shared.DB.Classes.User;
using MyTest = Shared.DB.Classes.Test.Test;

namespace WebApp.Services;

public class ApiService
{
    private HttpClient _httpClient;
    

    public ApiService(IConfiguration configuration)
    {
        
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
        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"User/get_user");
        if (responseMessage.IsSuccessStatusCode)
        {
            user = await responseMessage.Content.ReadFromJsonAsync<User>();
        }
        return user;
    } 
    
    public async Task<bool> PostTest(MyTest test)
    {
        var user = await GetUser();
        test.Creator = user!; //TODO: Failed if user == null && can't add a new test with multiply exception like problems with Foreign key
        var responseMessage = await _httpClient.PostAsJsonAsync("Test/create_test", test);
        return responseMessage.IsSuccessStatusCode;
    }

}