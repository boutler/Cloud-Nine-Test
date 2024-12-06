
namespace TechTestBackend.Helpers;

public static class SpotifyHelper
{
    public static bool IsValidSpotifyId(string id)
    {
        return id?.Length == 22;
    }
}