using Android.App;
using Android.OS;
using Immedia.Core.Helpers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Immedia.Core.Models;
using Android.Views;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Widget;

namespace Immedia.Android
{
    [Activity(Label = "Immedia", Icon = "@mipmap/iclauncher")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            ActionBar.Title = "Immedia";

            getData();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Home, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var result = false;

            switch (item.ItemId)
            {
                case Resource.Id.ActionAbout:
                    HandleAbout();
                    result = true;
                    break;
            }

            if (!result)
            {
                result = base.OnOptionsItemSelected(item);
            }

            return result;
        }

        #region Private Methods

        void HandleAbout()
        {
            var packageInfo = PackageManager.GetPackageInfo(PackageName, 0);
            var version = packageInfo.VersionName;

            var message = string.Format("Immedia {0}", version);

            var alertBuilder = new  AlertDialog.Builder(this);
            var alertDialog = alertBuilder.SetTitle(Resource.String.ActionAboutTitle)
                                          .SetMessage(message)
                                          .SetPositiveButton(Resource.String.AboutDialogClose, (sender, e) =>
                                          {
                                          })
                                          .Show();     
        }

        #endregion



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

            var apidata = JsonConvert.DeserializeObject<FlickerData>(json);

            if(apidata.stat == "ok")
            {
                foreach (Photo data in apidata.photos.photo)
                {
                    //To retrieve photo use this format: 
                    //http://farm{farmid}.staticFlickr.com/{server-id}/{id}_{secret}{size}.jpg

                    string photoUrl = "http://farm{0}.staticFlickr.com/{1}/{2}_{3}_n.jpg";

                    string baseFlickUrl = string.Format(photoUrl, data.farm, data.server, data.id, data.secret);
                
                
                    //flickerImage will be image for now to be replaced later with list of images
                
                }
            }
        }


    }
}

