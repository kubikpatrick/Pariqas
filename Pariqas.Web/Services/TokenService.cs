using Blazored.LocalStorage;

namespace Pariqas.Web.Services;

public sealed class TokenService
{
    private readonly ILocalStorageService _localStorage;
    
    public TokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(Constants.AccessToken);
    }

    public async Task SetTokenAsync(string token)
    {
        await _localStorage.SetItemAsStringAsync(Constants.AccessToken, token);
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorage.RemoveItemAsync(Constants.AccessToken);
    }
}