using Android.Graphics;

namespace Immedia.Core.Models
{
    public class FlickrPhoto
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }

        //Not Ideal model should be platform agnostic. TODO Change this
        public Bitmap Image { get; set; }
    }
}