using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Contracts.Services;
using Android.Graphics;

namespace ThinMPm.Platforms.Android.Services;

public class ArtworkService : IArtworkService
{
    public async Task<byte[]?> GetArtwork(string id)
    {
        try
        {
            var context = Platform.AppContext;
            var albumArtUri = Uri.Parse($"{MediaConstants.ALBUM_ART}/{id}");
            var source = ImageDecoder.CreateSource(context.ContentResolver, albumArtUri);

            return await Task.Run(() =>
            {
                var bitmap = ImageDecoder.DecodeBitmap(source);
                using var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 90, stream);

                return stream.ToArray();
            });
        }
        catch
        {
            return null;
        }
    }
}