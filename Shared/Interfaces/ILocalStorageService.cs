namespace Shared.Interfaces;

public interface ILocalStorageService
{
    public Task SetItemAsync(string name, string value, int days = 7);
    public Task<string?> GetItemAsync(string name);
}