using Android.OS;
using Android.Views;
using Android.Content.Res;

namespace SideMenuNavigationTest.Fragments
{
    public class OrdersFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Orientation == Orientation.Portrait ? Resource.Layout.orders_fragment_layout : Resource.Layout.orders_fragment_layout_land, container, false);
        }
    }
}