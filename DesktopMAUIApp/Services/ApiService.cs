﻿using System.Net;
using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Claims;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Data.Test;

namespace DesktopMAUIApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer = new();
    public Dictionary<string, string>? Claims { get; private set; }
    public bool IsAuthroized => Claims?.Count > 0;
    public string? GetRole => Claims?[ClaimTypes.Role];
    public string? GetId => Claims?[ClaimTypes.NameIdentifier];
    public string? GetName => Claims?[ClaimTypes.Name];

    private ILogger<ApiService> Logger { get; }

    public ApiService(ILogger<ApiService> logger)
    {
        Logger = logger;
        var handler = new HttpClientHandler()
        {
            CookieContainer = _cookieContainer
        };

#if DEBUG
        handler.ServerCertificateCustomValidationCallback = delegate { return true; };
#endif

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(Constants.API_URL)
        };
    }

    #region Cookie

    /// <summary>
    /// Использовать после запроса
    /// </summary>
    public void SaveCookies()
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
    public void LoadCookies()
    {
        var formatter = new SoapFormatter();
        var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "cookies.dat");

        if (!File.Exists(file))
        {
            return;
        }

        CookieCollection retrievedCookies;
        using (Stream s = File.OpenRead(file))
            retrievedCookies = (CookieCollection)formatter.Deserialize(s);

        _cookieContainer.Add(retrievedCookies);
        Logger.LogInformation($"Загружено {retrievedCookies.Count} Cookie");
    }

    #endregion

    #region Auth

    public async Task<bool> LoginAsync(string login, string password)
    {
        try
        {
            var form = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("login", login),
                new KeyValuePair<string, string>("password", password)
            ]);
            var response = await _httpClient.PostAsync("/Account/MobileLogin", form);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogInformation($"Error: {response.ReasonPhrase}");
                return false;
            }

            SaveCookies();
            Logger.LogInformation($"Logged in successfully");
            return await CheckAuth();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{login} : {password}");
            return false;
        }
    }

    public async Task<bool> CheckAuth()
    {
        var response = await _httpClient.GetAsync("/Account/MobileClaims");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        Claims = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (Claims == null || Claims.Count == 0)
        {
            Logger.LogCritical("На запрос Claims пришёл пустой словарь");
            return false;
        }

        SaveCookies();
        Logger.LogInformation($"Logged in as {Claims![ClaimTypes.Name]}:{Claims[ClaimTypes.Role]}");
        
        return true;
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await _httpClient.PostAsync("/Account/MobileLogout", null);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        foreach (Cookie cookie in _cookieContainer.GetAllCookies())
        {
            cookie.Expired = true;
        }

        Logger.LogInformation($"Кол-во кукисов после логаута {_cookieContainer.GetAllCookies().Count}");
        SaveCookies();

        Claims = null;
        return true;
    }

    #endregion

    #region Tests

    public async Task<List<UniqueTest>?> GetTestsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/Test/get_tests");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var tests = await response.Content.ReadFromJsonAsync<List<UniqueTest>>();
            Toast.Make("Тесты получены", ToastDuration.Long)?.Show();
            return tests;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return null;
        }
        
    }

    public async Task<UniqueTest> GetTest(Guid testId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/Test/get_test_for_solving/{testId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var test = await response.Content.ReadFromJsonAsync<UniqueTest>();
            Toast.Make($"Тест {test.TestName} загружен", ToastDuration.Long)?.Show();
            return test;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return null;
        }
        
        
    }

    #endregion
}