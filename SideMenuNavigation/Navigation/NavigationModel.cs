using Android.Graphics.Drawables;
using System;

namespace SideMenuNavigation
{
    public class NavigationModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Drawable Icon { get; set; }
        public bool Selected { get; set; }

        public NavigationModel(int id, string title, Drawable icon)
        {
            Id = id;
            Title = title;
            Icon = icon;
        }
    }
}