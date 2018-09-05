using Android.App;
using Android.OS;
using Immedia.Core.Helpers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Immedia.Core.Models;

namespace Immedia.Android
{
    [Activity(Label = "Immedia", MainLauncher = true, Icon = "@mipmap/iclauncher")]
    public class MainActivity : Activity
    {
       protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            getData();
        }

       async void getData()
        {
            var url = WebRequestHelper.BaseUrl + "?method=flickr.photos.search";

            var content = new Dictionary<string, string>
            {
                ["api_key"] = "3d9fd8ad0eede887e3427a7984990f4a",
                ["lat"] = "-26.0821681",
                ["lon"] = "28.0213506",
                ["format"] = "json",
                ["nojsoncallback"] = "1",
                ["api_sig"] = "06c99bc9148becfc6fc832a003812b18"
            };

            var response = await WebRequestHelper.MakeAsyncRequest(url, content);
            var json = response.Content.ReadAsStringAsync().Result;

            var verification = JsonConvert.DeserializeObject<Photo>(json);

        }
    }
}

