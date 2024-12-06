using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TechTestBackend.DataBaseContext;
using TechTestBackend.Services.Spotify.Responses;

namespace TechTestBackend.Services.Spotify;

public class SpotifyService : IApiMarker
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId = "996d0037680544c987287a9b0470fdbb";
    private readonly string _clientSecret = "5a3c92099a324b8f9e45d77e919fec13";
    
    public SpotifyService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        Task.Run(() => SetAuthToken()).Wait();
    }

    #region Public Methods
    public async Task<Soptifysong[]> GetTracksByName(string name)
    {
        var httpResponse = await _httpClient.GetAsync("https://api.spotify.com/v1/search?q=" + name + "&type=track");
        ValidateHttpResponse(httpResponse);
        var stringResponse = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<SearchTracksResponse>(stringResponse).tracks.items;
    }

    public async Task<Soptifysong[]> GetTracksByIds(List<string> ids)
    {
        var httpResponse = await _httpClient.GetAsync("https://api.spotify.com/v1/tracks?ids=" + string.Join(",", ids));
        ValidateHttpResponse(httpResponse);
        var stringResponse = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<GetTracksResponse>(stringResponse)?.tracks;
    }

    public async Task<Soptifysong> GetTrack(string id)
    {
        var httpResponse = await _httpClient.GetAsync("https://api.spotify.com/v1/tracks/" + id + "/");
        ValidateHttpResponse(httpResponse);
        var stringResponse = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Soptifysong>(stringResponse);
    }
    #endregion

    #region Private Methods
    private async Task SetAuthToken()
    {
        byte[] encoding = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        var base64 = Convert.ToBase64String(encoding);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

        HttpResponseMessage httpResponse = await _httpClient.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }));

        ValidateHttpResponse(httpResponse);

        TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await httpResponse.Content.ReadAsStringAsync());

        if (string.IsNullOrEmpty(tokenResponse?.access_token))
            throw new Exception("Token null or empty");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.access_token);
    }

    private void ValidateHttpResponse(HttpResponseMessage httpResponse)
    {
        if (httpResponse?.IsSuccessStatusCode == false)
            throw new HttpRequestException(httpResponse.ReasonPhrase, null, httpResponse.StatusCode);
    }
    #endregion
}