using Android.Content;
using Android.Database;
using Android.OS;
using Uri = Android.Net.Uri;

namespace ThinMPm.Platforms.Android.Repositories;

public abstract class MediaStoreRepository<T>
{
    protected readonly Context Context;
    protected readonly Uri Uri;
    protected readonly string[] Projection;

    protected ICursor? Cursor { get; set; }
    public string? Selection { get; set; }
    public string[]? SelectionArgs { get; set; }
    public string? SortOrder { get; set; }
    public Bundle? Bundle { get; set; }

    protected MediaStoreRepository(Uri uri, string[] projection)
    {
        Context = Platform.CurrentActivity?.ApplicationContext 
            ?? throw new InvalidOperationException("Platform.CurrentActivity is null. Cannot obtain ApplicationContext.");
        Uri = uri;
        Projection = projection;
    }

    private void Initialize()
    {
        // Cursor = CreateCursor();
    }

    public abstract T Fetch();

    protected T? Get()
    {
        throw new NotImplementedException("Get method is not implemented yet.");
        // Initialize();

        // if (Cursor == null || !Cursor.MoveToNext())
        // {
        //     Destroy();
        //     return default;
        // }

        // var item = Fetch();

        // Destroy();

        // return item;
    }

    protected IList<T> GetList()
    {
        throw new NotImplementedException("GetList method is not implemented yet.");
        // Initialize();

        // var list = new List<T>();

        // if (Cursor != null)
        // {
        //     while (Cursor.MoveToNext())
        //     {
        //         list.Add(Fetch());
        //     }
        // }

        // Destroy();

        // return list;
    }

    protected string[] ToStringArray(IEnumerable<string> list)
    {
        throw new NotImplementedException("ToStringArray method is not implemented yet.");
        // return list.ToArray();
    }

    protected string MakePlaceholders(int size)
    {
        throw new NotImplementedException("MakePlaceholders method is not implemented yet.");
        // return string.Join(",", Enumerable.Repeat("?", size));
    }

    private ICursor? CreateCursor()
    {
        throw new NotImplementedException("CreateCursor method is not implemented yet.");
        // if (Bundle != null)
        // {
        //     return Context.ContentResolver.Query(
        //         Uri,
        //         Projection,
        //         Bundle,
        //         null
        //     );
        // }
        // else
        // {
        //     return Context.ContentResolver.Query(
        //         Uri,
        //         Projection,
        //         Selection,
        //         SelectionArgs,
        //         SortOrder
        //     );
        // }
    }

    private void Destroy()
    {
        // Cursor?.Close();
        // Cursor = null;
    }
}