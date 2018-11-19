using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;

namespace SideMenuNavigation
{
    internal class NavigationAdapter : RecyclerView.Adapter
    {
        #region properties

        //TODO: Remove default colors when will added Theme
        public Color NormalTextColor { get; set; } = Color.White;
        public Color SelectedTextColor { get; set; } = Color.White;
        public Color NormalIconColor { get; set; } = Color.White;
        public Color SelectedIconColor { get; set; } = Color.White;
        public Color SelectedBackgroundColor { get; set; } = Color.ParseColor("#50E1F5FE");
        public Typeface Typeface { get; set; }

        public List<NavigationModel> Items { get; private set; }

        #endregion

        #region events

        public event ItemClickEventHandler ItemClicked;

        #endregion

        #region constructor

        public NavigationAdapter(List<NavigationModel> navigationModels)
        {
            Items = navigationModels;
        }

        #endregion

        #region RecyclerView>adapter implementation

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var navigationHolder = holder as NavigationViewHolder;
            var model = Items[position];

            navigationHolder.IconView.SetImageDrawable(model.Icon);
            navigationHolder.TitleView.Text = model.Title;

            if(Typeface != null)
                navigationHolder.TitleView.Typeface = Typeface;

            if(model.Selected)
            {
                navigationHolder.IconView.SetColorFilter(SelectedIconColor, PorterDuff.Mode.SrcIn);
                navigationHolder.TitleView.SetTextColor(SelectedTextColor);
                navigationHolder.Container.SetBackgroundColor(SelectedBackgroundColor);
            }
            else
            {
                navigationHolder.IconView.SetColorFilter(NormalIconColor, PorterDuff.Mode.SrcIn);
                navigationHolder.TitleView.SetTextColor(NormalTextColor);
                navigationHolder.Container.SetBackgroundColor(Color.Transparent);
            }

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.navigation_cell_layout, parent, false);
            var viewHolder = new NavigationViewHolder(itemView, OnClick);
            return viewHolder;

        }

        #endregion

        #region utility methods

        private void OnClick(int position)
        {
            var menu = Items[position];
            if(!menu.Selected)
            {
                foreach(var item in Items)
                    item.Selected = false;

                menu.Selected = true;

                NotifyDataSetChanged();
            }

            ItemClicked?.Invoke(menu.Id);
        }

        #endregion
    }
}