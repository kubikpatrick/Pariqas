using System.Net.Http.Headers;
using System.Net.Http.Json;

using Pariqas.Http.Requests;
using Pariqas.Http.Responses;

namespace Pariqas.Daemon.Services;

public sealed class JwtService
{
    private readonly HttpClient _http;
    
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;

    private string _refreshToken;

    public JwtService(HttpClient http, IConfiguration configuration, ILogger<JwtService> logger)
    {
        _http = http;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("[{timestamp}] - Initializing JWT tokens.", DateTime.UtcNow);
        await GetTokensAsync();
    }
    
    public async Task RefreshAccessTokenAsync()
    {
        var response = await _http.PostAsJsonAsync("authentication/refresh", new RefreshRequest(_refreshToken));
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("[{timestamp}] - Token refresh failed with status {status}. Attempting full re-authentication...", DateTime.UtcNow, response.StatusCode);

            await GetTokensAsync();
            return;
        }
        
        var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? throw new InvalidOperationException("Invalid refresh token response.");

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        _logger.LogInformation("[{timestamp}] - Access token successfully refreshed.", DateTime.UtcNow);
    }
    
    private async Task GetTokensAsync()
    {
        var response = await _http.PostAsJsonAsync("authentication/login", new LoginRequest
        {
            Email = _configuration["Credentials:Email"],
            Password = _configuration["Credentials:Password"]
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Login failed with status {response.StatusCode}: {response.ReasonPhrase}" );
        }

        var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? throw new InvalidOperationException("Invalid login token response.");

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        _refreshToken = tokens.RefreshToken;
        _logger.LogInformation("[{timestamp}] - Obtained new tokens successfully.", DateTime.UtcNow);
    }
}
