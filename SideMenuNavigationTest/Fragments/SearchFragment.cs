using Android.Content.Res;
using Android.OS;
using Android.Views;

namespace SideMenuNavigationTest.Fragments
{
    public class SearchFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Orientation == Orientation.Portrait ? Resource.Layout.search_fragment_layout : Resource.Layout.search_fragment_layout_land, container, false);
        }
    }
}