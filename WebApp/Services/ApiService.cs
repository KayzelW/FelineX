using WebApp.Components.Pages.tests;
using MyTest = Shared.DB.Classes.Test.Test;

namespace WebApp.Services;

public class ApiService
{
    private HttpClient _httpClient;

    public ApiService()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var BaseUrl = config.GetConnectionString("ApiUrl");
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(BaseUrl ?? "http://localhost:5071/")
        };
    }

    public async Task<MyTest> GetRandomTest()
    {
        MyTest? _test = null;
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("Test");
        if (responseMessage.IsSuccessStatusCode)
        {
            _test = await responseMessage.Content.ReadFromJsonAsync<MyTest>();
        }

        return _test;
    }
}