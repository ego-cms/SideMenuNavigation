using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace SideMenuNavigation
{
    internal class NavigationViewHolder : RecyclerView.ViewHolder
    {
        public TextView TitleView { get; private set; }
        public ImageView IconView { get; private set; }
        public LinearLayout Container { get; set; }

        public NavigationViewHolder(View itemView, Action<int> clickAction) : base(itemView)
        {
            TitleView = itemView.FindViewById<TextView>(Resource.Id.title_view);
            IconView = itemView.FindViewById<ImageView>(Resource.Id.icon_view);
            Container = itemView.FindViewById<LinearLayout>(Resource.Id.container);

            ItemView.Click += delegate
            {
                clickAction?.Invoke(Position);
            };
        }
    }
}