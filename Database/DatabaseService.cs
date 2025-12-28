using SQLite;
using ThinMPm.Database.Entities;

namespace ThinMPm.Database;

public class DatabaseService
{
    private const string DatabaseFilename = "thinmpm.db";
    private static SQLiteAsyncConnection? _database;
    private static bool _isInitialized;
    private static readonly SemaphoreSlim _initLock = new(1, 1);

    public static SQLiteAsyncConnection Database
    {
        get
        {
            if (_database == null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
                _database = new SQLiteAsyncConnection(dbPath);
            }
            return _database;
        }
    }

    public static async Task InitializeAsync()
    {
        if (_isInitialized) return;

        await _initLock.WaitAsync();
        try
        {
            if (_isInitialized) return;

            await Database.CreateTableAsync<FavoriteSongEntity>();
            await Database.CreateTableAsync<FavoriteArtistEntity>();
            await Database.CreateTableAsync<PlaylistEntity>();
            await Database.CreateTableAsync<PlaylistSongEntity>();
            await Database.CreateTableAsync<ShortcutEntity>();
            _isInitialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }
}
