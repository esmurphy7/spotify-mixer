using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpotifyMixerApi.Controllers
{
    [ApiController]
    [Route("auth/spotify")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        // In-memory storage for user tokens (for demo purposes only)
        private static readonly Dictionary<string, (string accessToken, string refreshToken)> _userTokens = new();
        private static readonly HttpClient _httpClient = new();

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Starts the Spotify OAuth login flow for a given user.
        /// Redirects the user to Spotify's authorization page.
        /// </summary>
        /// <param name="userId">A unique identifier for the user (used as state).</param>
        /// <returns>Redirects to Spotify's authorization URL.</returns>
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string userId)
        {
            var clientId = _config["Spotify:ClientId"] ?? "";
            var redirectUri = _config["Spotify:RedirectUri"] ?? "";
            var scopes = "user-read-email playlist-modify-public playlist-modify-private";
            var state = userId; // In production, use a secure random state
            var url = $"https://accounts.spotify.com/authorize?response_type=code&client_id={clientId}&scope={Uri.EscapeDataString(scopes)}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}";
            return Redirect(url);
        }

        /// <summary>
        /// Handles the Spotify OAuth callback, exchanges the code for access and refresh tokens, and stores them.
        /// </summary>
        /// <param name="code">The authorization code returned by Spotify.</param>
        /// <param name="state">The state parameter (userId) sent in the login step.</param>
        /// <returns>Returns a success message and the userId if successful, or an error if the exchange fails.</returns>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            var clientId = _config["Spotify:ClientId"] ?? "";
            var clientSecret = _config["Spotify:ClientSecret"] ?? "";
            var redirectUri = _config["Spotify:RedirectUri"] ?? "";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            var body = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "authorization_code"),
                new("code", code),
                new("redirect_uri", redirectUri),
                new("client_id", clientId),
                new("client_secret", clientSecret)
            };
            request.Content = new FormUrlEncodedContent(body);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(content);
            }
            using var doc = JsonDocument.Parse(content);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString() ?? "";
            var refreshToken = doc.RootElement.GetProperty("refresh_token").GetString() ?? "";
            // Store tokens in memory for demo; use DB in production
            _userTokens[state] = (accessToken, refreshToken);
            return Ok(new { message = "Spotify authentication successful", userId = state });
        }

        /// <summary>
        /// (Demo) Retrieves the access and refresh tokens for a given userId.
        /// </summary>
        /// <param name="userId">The user identifier used during login.</param>
        /// <returns>The access and refresh tokens if found, otherwise 404.</returns>
        [HttpGet("token/{userId}")]
        public IActionResult GetToken(string userId)
        {
            if (_userTokens.TryGetValue(userId, out var tokens))
            {
                return Ok(new { tokens.accessToken, tokens.refreshToken });
            }
            return NotFound();
        }
    }
} 