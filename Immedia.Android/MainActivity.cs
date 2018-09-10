using Android.App;
using Android.OS;
using Immedia.Core.Helpers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Immedia.Core.Models;
using Android.Views;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Widget.Toolbar;
using Android.Support.V7.Widget;
using Immedia.Android.Adapters;

namespace Immedia.Android
{
    [Activity(Label = "Immedia", Icon = "@mipmap/iclauncher")]
    public class MainActivity : Activity
    {
        List<FlickrPhoto> _photos = new List<FlickrPhoto>();

        RecyclerView _recyclerView;
        public RecyclerView.LayoutManager layoutManager;
        public PhotoAdapter Adapter { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
             base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            ActionBar.Title = "Immedia";

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

             GetData();

            layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(layoutManager);
            Adapter = new PhotoAdapter(_photos);
            _recyclerView.SetAdapter(Adapter);
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

        //TODO Move out to a service
        //Create Card View Layout

        async void GetData()
        {
            var progress = new ProgressDialog(this);
            progress.SetTitle("Loading");
            progress.SetCanceledOnTouchOutside(false);
            
            progress.Show();

            var url = WebRequestHelper.BaseUrl + "?method=flickr.photos.search";

            var content = new Dictionary<string, string>
            {
                ["api_key"] = "c0f8adc7bf4991dca92da91db98d41f0",
                ["lat"] = "-26.0821681",
                ["lon"] = "28.0213506",
                ["format"] = "json",
                ["nojsoncallback"] = "1",
            };

            var response = await WebRequestHelper.MakeAsyncRequest(url, content);
            var json = response.Content.ReadAsStringAsync().Result;

            var apidata = JsonConvert.DeserializeObject<FlickerData>(json);

            if(apidata.stat == "ok")
            {
                for (int i = 1; i < 10; i++)
                {
                    i++;

                    var photo = apidata.photos.photo[i];

                    long id = long.Parse(photo.id);

                    string photoUrl = "http://farm{0}.staticFlickr.com/{1}/{2}_{3}_n.jpg";
                    string baseFlickUrl = string.Format(photoUrl, photo.farm, photo.server, photo.id, photo.secret);

                    var bitmap = await Helpers.BitmapHelper.GetImageFromUrl(baseFlickUrl);

                    var flickrPhoto = new FlickrPhoto
                    {
                        ID = id,
                        ImageURL = baseFlickUrl,
                        Title = photo.title,
                        Image = bitmap
                    };

                    _photos.Add(flickrPhoto);
                }


                //Foreach replaced by for loop for now until Image download is moved to a service.
                //foreach (Photo data in apidata.photos.photo)
                //{
                //}

                Adapter.NotifyDataSetChanged();
                progress.Hide();
            }
        }
    }
}

