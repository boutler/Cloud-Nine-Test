using TechTestBackend.DataBaseContext;

namespace TechTestBackend.Repositories
{
    public class SongStorageRepository : IApiMarker
    {
        SongstorageContext _songStorageContext;
        ILogger<SongStorageRepository> _logger;
        public SongStorageRepository(SongstorageContext songStorageContext, ILogger<SongStorageRepository> logger)
        {
            _songStorageContext = songStorageContext;
            _logger = logger;
        }

        #region Public Methods
        public bool SongExists(string id)
        {
            try
            {
                return _songStorageContext.Songs.Any(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error checking if song exists");
                throw e;
            }
        }

        public bool Save(Soptifysong song)
        {
            try
            {
                if (SongExists(song.Id))
                    return false;

                _songStorageContext.Songs.Add(song);
                return _songStorageContext.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving song");
                throw e;
            }
        }

        public bool RemoveById(string songId)
        {
            try
            {
                var song = GetSongById(songId);

                if (song == null)
                    return false;

                _songStorageContext.Songs.Remove(song);
                return _songStorageContext.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing song by song id: " + songId);
                throw e;
            }
        }

        public List<string> RemoveByIds(List<string> songIds)
        {
            try
            {
                var songs = GetSongsByIds(songIds);

                if (songs.Any() == false)
                    return new List<string>();

                _songStorageContext.Songs.RemoveRange(songs);
                _songStorageContext.SaveChanges();

                return songs.Select(s => s.Id).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing song by song ids: " + string.Join(", ", songIds));
                throw e;
            }
        }

        public Soptifysong? GetSongById(string id)
        {
            try
            {
                return _songStorageContext.Songs.SingleOrDefault(s => s.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting song by id: " + id);
                throw e;
            }
        }

        public List<Soptifysong> GetSongsByIds(List<string> songIds)
        {
            try
            {
                return _songStorageContext.Songs.Where(s => songIds.Contains(s.Id)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting songs by ids: " + string.Join(", ", songIds));
                throw e;
            }
        }

        public List<Soptifysong> GetAll()
        {
            try
            {
                return _songStorageContext.Songs.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all songs");
                throw e;
            }
        }

        #endregion

    }
}
