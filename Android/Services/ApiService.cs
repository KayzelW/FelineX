using System.Net;
using System.Runtime.Serialization.Formatters.Soap;
using Android.Content;
using Android.Content.Res;

namespace Android.Services;

internal class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer = new();

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
        Toast.MakeText(Application.Context, $"Загружено {retrievedCookies.Count} Cookie", ToastLength.Long)!.Show();
    }

    #endregion

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

        return true;
    }

    internal async Task<bool> CheckAuth()
    {
        var response = await _httpClient.GetAsync("/Account/MobileClaims");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        return true;
    }
}