using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Contracts.Services;
using Android.Graphics;

namespace ThinMPm.Platforms.Android.Services;

public class AlbumArtService : IAlbumArtService
{
    public async Task<string?> GetArtwork(string id)
    {
        try
        {
            var context = Platform.AppContext;
            var albumArtUri = Uri.Parse($"{MediaConstant.ALBUM_ART}/{id}");
            var source = ImageDecoder.CreateSource(context.ContentResolver, albumArtUri);
            var bitmap = await Task.Run(() => ImageDecoder.DecodeBitmap(source));
            var stream = new MemoryStream();

            bitmap.Compress(Bitmap.CompressFormat.Png, 90, stream);

            return Convert.ToBase64String(stream.ToArray());
        }
        catch
        {
            return null;
        }
    }
}