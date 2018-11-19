using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using SideMenuNavigation;
using SideMenuNavigationTest.Fragments;

namespace SideMenuNavigationTest
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var navigation = FindViewById<NavigationFragmentLayout>(Resource.Id.navigation_container);

            navigation.SetupNavigation(GenerateMenuFragmentModels(), FragmentManager);

            navigation.FragmentActionBarColor = Color.ParseColor("#3DC0C6");
            navigation.FragmentBackgroundColor = Color.ParseColor("#3DC0C6");
            navigation.LeftIconColor = Color.ParseColor("#3D3D45");
            navigation.RightIconColor = Color.ParseColor("#3D3D45");
            navigation.TitleTypeface = Typeface.CreateFromAsset(Assets, "Fonts/dincondensedc.otf");
            navigation.ItemTypeface = Typeface.CreateFromAsset(Assets, "Fonts/BodoniBook.ttf");
            navigation.AccountTextColor = Color.White;
        }

        private List<NavigationFragmentModel> GenerateMenuFragmentModels()
        {
            return new List<NavigationFragmentModel>()
            {
                new NavigationFragmentModel(new NavigationModel(1, "Search", BaseContext.GetDrawable(Resource.Drawable.search)), new SearchFragment()),
                new NavigationFragmentModel(new NavigationModel(2, "Home", BaseContext.GetDrawable(Resource.Drawable.home)), new HomeFragment()),
                new NavigationFragmentModel(new NavigationModel(3, "Sale", BaseContext.GetDrawable(Resource.Drawable.percent)), new SaleFragment()),
                new NavigationFragmentModel(new NavigationModel(4, "Catalog", BaseContext.GetDrawable(Resource.Drawable.grid)), new CatalogFragment()),
                new NavigationFragmentModel(new NavigationModel(5, "Orders", BaseContext.GetDrawable(Resource.Drawable.wallet)), new OrdersFragment()),
                new NavigationFragmentModel(new NavigationModel(6, "Info", BaseContext.GetDrawable(Resource.Drawable.information)), new InformationFragment()),
                new NavigationFragmentModel(new NavigationModel(7, "Settings", BaseContext.GetDrawable(Resource.Drawable.settings)), new SettingsFragment()),
            };
        }
    }
}