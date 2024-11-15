using System.Net.Http.Json;

namespace WebAssembly.Services
{
    public class SearchService<T>
    {
        private readonly HttpClient _httpClient;
        private string _searchQuery = string.Empty;
        private Timer _debounceTimer;
        
        public event Action<List<T>>? SuggestionsUpdated;

        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnSearchInput(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 4)
            {
                NotifySuggestionsUpdated([]);
                return;
            }

            _searchQuery = input;
            _debounceTimer?.Dispose();
            _debounceTimer = new Timer(async _ =>
            {
                var suggestions = await GetSuggestions(_searchQuery);
                NotifySuggestionsUpdated(suggestions);
            }, null, 300, Timeout.Infinite);
        }
        
        private async Task<List<T>> GetSuggestions(string query)
        {
            switch (typeof(T))
            {
                case { } t when t == typeof(User):
                    var userSuggestions = await GetUsersSuggestions(query);
                    return userSuggestions as List<T> ?? [];
                case { } t when t == typeof(UserGroup):
                    var groupSuggestions = await GetGroupsSuggestions(query);
                    return groupSuggestions as List<T> ?? [];
                default:
                    throw new InvalidOperationException("Unsupported type for suggestions.");
            }
        }

        private async Task<List<User>> GetUsersSuggestions(string query)
        {
            var response = await _httpClient.GetAsync($"/Search/search_users?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var suggestions = await response.Content.ReadFromJsonAsync<List<User>>();
                return suggestions ?? [];
            }

            return [];
        }

        private async Task<List<UserGroup>> GetGroupsSuggestions(string query)
        {
            var response = await _httpClient.GetAsync($"/Search/search_groups?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var suggestions = await response.Content.ReadFromJsonAsync<List<UserGroup>>();
                return suggestions ?? [];
            }

            return [];
        }
        
        private void NotifySuggestionsUpdated(List<T> suggestions)
        {
            SuggestionsUpdated?.Invoke(suggestions);
        }
    }
}