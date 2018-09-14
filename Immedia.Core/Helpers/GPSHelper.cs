using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Immedia.Core.Helpers
{
    public static class GPSHelper
    {
        public static  async Task<Position> GetPosition()
        {
            //TODO Write code to check permissions first 

            var locator = CrossGeolocator.Current;

            if (!locator.IsGeolocationAvailable || locator.IsListening)
            {
                return null;
            }

            locator.DesiredAccuracy = 20;

            return await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
        }
    }
}

