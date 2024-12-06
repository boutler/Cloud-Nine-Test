using TechTestBackend.DataBaseContext;

namespace TechTestBackend.Services.Spotify.Responses
{
    public class SearchTracksResponse

    {
        public SearchTracksResponse() { }

        public Tracks tracks { get; set; }
       
    }

    public class Tracks
    {
        public Soptifysong[] items { get; set; }
    }
}
