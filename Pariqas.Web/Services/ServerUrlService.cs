using Blazored.LocalStorage;

namespace Pariqas.Web.Services;

public sealed class ServerUrlService
{
    private readonly ILocalStorageService _localStorage;
    
    public ServerUrlService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
    public async Task<string?> GetServerUrlAsync()
    {
        return await _localStorage.GetItemAsStringAsync(Constants.ServerUrl);
    }

    public async Task SetServerUrlAsync(string url)
    {
        await _localStorage.SetItemAsStringAsync(Constants.ServerUrl, url);
    }
    
    public async Task RemoveServerUrlAsync()
    {
        await _localStorage.RemoveItemAsync(Constants.ServerUrl);
    }
}