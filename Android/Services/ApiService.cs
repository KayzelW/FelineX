using System.Net;
using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Soap;
using Android.Content;
using Android.Content.Res;

namespace Android.Services;

internal class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer = new();
    public IEnumerable<ClaimRecord>? Claims { get; private set; }

    public ApiService()
    {
        var handler = new HttpClientHandler()
        {
            CookieContainer = _cookieContainer
        };
#if DEBUG

        handler.ServerCertificateCustomValidationCallback = delegate { return true; };

#endif

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(Application.Context.Resources!.GetString(Resource.String.base_url_https))
        };
    }

    #region Cookie

    /// <summary>
    /// Использовать после запроса
    /// </summary>
    internal void SaveCookies()
    {
        var formatter = new SoapFormatter();
        var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "cookies.dat");

        using (Stream s = File.Create(file))
            formatter.Serialize(s, _cookieContainer.GetAllCookies());
    }

    /// <summary>
    /// Вызывается при регистрации сервиса
    /// </summary>
    internal void LoadCookies()
    {
        var formatter = new SoapFormatter();
        var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "cookies.dat");

        if (!File.Exists(file))
        {
            return;
        }

        CookieCollection retrievedCookies = null;
        using (Stream s = File.OpenRead(file))
            retrievedCookies = (CookieCollection)formatter.Deserialize(s);

        _cookieContainer.Add(retrievedCookies);
        Toast.MakeText(Application.Context, $"Загружено {retrievedCookies.Count} Cookie", ToastLength.Long)?.Show();
    }

    #endregion

    #region Auth

    internal async Task<bool> AuthAsync(string login, string password)
    {
        var form = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("login", login),
            new KeyValuePair<string, string>("password", password)
        ]);
        var response = await _httpClient.PostAsync("/Account/MobileLogin", form);
        if (!response.IsSuccessStatusCode)
        {
            Toast.MakeText(Application.Context, $"Error: {response.ReasonPhrase}", ToastLength.Long)?.Show();
            return false;
        }

        SaveCookies();
        Toast.MakeText(Application.Context, $"Logged in as {login}", ToastLength.Long)?.Show();

        return await CheckAuth();
    }

    internal async Task<bool> CheckAuth()
    {
        var response = await _httpClient.GetAsync("/Account/MobileClaims");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        Claims = await response.Content.ReadFromJsonAsync<IEnumerable<ClaimRecord>>();
        SaveCookies();

        return true;
    }

    internal async Task<bool> LogoutAsync()
    {
        var response = await _httpClient.GetAsync("/Account/MobileLogout");
        foreach (Cookie cookie in _cookieContainer.GetAllCookies())
        {
            cookie.Expired = true;
        }

        Toast.MakeText(Application.Context, $"Кол-во кукисов после логаута {_cookieContainer.GetAllCookies().Count}",
            ToastLength.Long)?.Show();
        SaveCookies();
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        Claims = null;
        return true;
    }

    #endregion

    #region Tests

    internal async Task<bool> GetTestsAsync()
    {
        var response = await _httpClient.GetAsync("/Test/api/Test/get_tests");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var text = await response.Content.ReadAsStringAsync();
        Toast.MakeText(Application.Context, text, ToastLength.Long)?.Show();
        return true;
    }

    #endregion
}

internal class ClaimRecord
{
    public string Type { get; set; }
    public string Value { get; set; }
}