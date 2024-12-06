using Microsoft.AspNetCore.Mvc;
using TechTestBackend.DataBaseContext;
using TechTestBackend.Helpers;
using TechTestBackend.Repositories;
using TechTestBackend.Services.Spotify;

namespace TechTestBackend.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyController : ControllerBase, IApiMarker
{
    SpotifyService _spotifyService;
    SongStorageRepository _songStorageRepository;
    ILogger<SpotifyController> _logger;

    public SpotifyController(SongStorageRepository songStorageRepository, SpotifyService spotifyService, ILogger<SpotifyController> logger)
    {
        _songStorageRepository = songStorageRepository;
        _spotifyService = spotifyService;
        _logger = logger;
    }

    [HttpGet]
    [Route("searchTracks")]
    public async Task<IActionResult> SearchTracks(string name)
    {
        try
        {
            return Ok(await _spotifyService.GetTracksByName(name));
        }
        catch (HttpRequestException re)
        {
            _logger.LogError(re, "searchTracks - HttpRequestException");
            return StatusCode((int)re.StatusCode.GetValueOrDefault(), re.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "searchTracks - Exception");
            throw e;
        }
    }

    [HttpPost]
    [Route("like")]
    public async Task<IActionResult> Like(string id)
    {
        try
        {
            if (SpotifyHelper.IsValidSpotifyId(id) == false)
                return BadRequest("Invalid Spotify Id");

            Soptifysong track = await _spotifyService.GetTrack(id);

            return Ok(_songStorageRepository.Save(track));           
        }
        catch (HttpRequestException re)
        {
            _logger.LogError(re, "like - HttpRequestException");
            return StatusCode((int)re.StatusCode.GetValueOrDefault(), re.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "like - Exception");
            throw e;
        }
    }

    [HttpPost]
    [Route("removeLike")]
    public async Task<IActionResult> RemoveLike(string id)
    {
        try
        {
            if (SpotifyHelper.IsValidSpotifyId(id) == false)
                return BadRequest("Invalid Spotify Id");

            Soptifysong track = await _spotifyService.GetTrack(id);

            return Ok(_songStorageRepository.RemoveById(track.Id));
        }
        catch (HttpRequestException re)
        {
            _logger.LogError(re, "removeLike - HttpRequestException");
            return StatusCode((int)re.StatusCode.GetValueOrDefault(), re.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "removeLike - Exception");
            throw e;
        }
    }

    [HttpGet]
    [Route("listLiked")]
    public async Task<IActionResult> ListLiked()
    {
        try
        {
            var localSongs = _songStorageRepository.GetAll();

            List<string> likedSongIds = localSongs.Select(s => s.Id).ToList();
            

            Soptifysong[] spotifySongs = await _spotifyService.GetTracksByIds(likedSongIds);
            List<string> songIds = spotifySongs.Select(s => s.Id).ToList();
            List<string> removedSongIds = new List<string>();

            // Remove from local songs if not exists in Spotify
            localSongs.RemoveAll((ls) => 
            {
                bool exists = songIds.Contains(ls.Id);

                if (exists == false)
                    removedSongIds.Add(ls.Id);

                return !exists;
            });

            if(removedSongIds.Any())
                _songStorageRepository.RemoveByIds(removedSongIds);

            return Ok(localSongs);
        }
        catch (HttpRequestException re)
        {
            _logger.LogError(re, "listLiked - HttpRequestException");
            return StatusCode((int)re.StatusCode.GetValueOrDefault(), re.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "listLiked - Exception");
            throw e;
        }
    }
}