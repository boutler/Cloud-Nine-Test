using TechTestBackend.DataBaseContext;

namespace TechTestBackend.Services.Spotify.Responses
{
    public class GetTracksResponse
    {
        public GetTracksResponse() { }

        public Soptifysong[] tracks { get; set; }
    }
}
