using Android.App;

namespace SideMenuNavigation
{
    public class NavigationFragmentModel
    {
        public NavigationModel NavigationModel { get; set; }
        public Fragment Fragment { get; set; }

        public NavigationFragmentModel(NavigationModel navigationModel, Fragment fragment)
        {
            NavigationModel = navigationModel;
            Fragment = fragment;
        }
    }
}