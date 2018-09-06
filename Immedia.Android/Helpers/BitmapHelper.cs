using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics;

namespace Immedia.Android.Helpers
{
    public static class BitmapHelper
    {
       public static async Task<Bitmap> GetImageFromUrl(string url)
        {
            using (var client = new HttpClient())
            {
                var msg = await client.GetAsync(url);
                if (msg.IsSuccessStatusCode)
                {
                    using (var stream = await msg.Content.ReadAsStreamAsync())
                    {
                        var bitmap = await BitmapFactory.DecodeStreamAsync(stream);
                        return bitmap;
                    }
                }
            }
            return null;
        }
    }
}