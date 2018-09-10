using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Immedia.Core.Models;

namespace Immedia.Android.Adapters
{
    public class PhotoAdapter : RecyclerView.Adapter
    {
        #region Properties
        
        LayoutInflater Inflater { get; set; }
        public IList<FlickrPhoto> Items { get; set; }

        #endregion

        #region View Holders

        public class FlickrImageViewHolder : RecyclerView.ViewHolder
        {
            public ImageView Image { get; }
            public TextView Title { get; }

            public FlickrImageViewHolder(View v) : base(v)
            {
                Image = (ImageView)v.FindViewById(Resource.Id.flickrImage);
                Title = (TextView)v.FindViewById(Resource.Id.imageTitle);
            }
        }

        #endregion

        #region Constructor

        public PhotoAdapter(List<FlickrPhoto> items)
        {
            Items = items;
        }

        #endregion

        #region Bind Views to View Holder

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FlickrImageViewHolder viewHolder = holder as FlickrImageViewHolder;

            viewHolder.Image.SetImageBitmap(Items[position].Image);
            viewHolder.Title.Text = Items[position].Title;
        }

        #endregion

        public override int ItemCount
        {
            get { return Items.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.FlickrImageView, parent, false);

            return new FlickrImageViewHolder(view);
        }
    }
}
