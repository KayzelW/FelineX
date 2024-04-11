using WebApp.Components.Pages.tests;
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

    public async Task<MyTest> GetRandomTest()
    {
        MyTest? test = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("Test");
        if (responseMessage.IsSuccessStatusCode)
        {
            test = await responseMessage.Content.ReadFromJsonAsync<MyTest>();
        }

        return test;
    }
}