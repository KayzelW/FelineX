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
            BaseAddress = new Uri(baseUrl ?? "http://localhost:5071/")
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
    
    public async Task<MyTest> GetTest()
    {
        MyTest? test = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("Test/get_test");
        if (responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            test = await responseMessage.Content.ReadFromJsonAsync<MyTest>();
        }
        return test;
    }
    
    
}