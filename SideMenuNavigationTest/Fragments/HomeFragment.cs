﻿using Android.OS;
using Android.Views;
using Android.Content.Res;

namespace SideMenuNavigationTest.Fragments
{
    public class HomeFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Orientation == Orientation.Portrait ? Resource.Layout.home_fragment_layout : Resource.Layout.home_fragment_layout_land, container, false);
        }
    }
}