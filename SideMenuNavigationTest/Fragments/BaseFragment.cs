using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;

namespace SideMenuNavigationTest.Fragments
{
    public class BaseFragment : Fragment
    {
        private LayoutInflater _inflater;
        private ViewGroup _container;
        private Bundle _savedInstanceState;

        private Orientation _orientation;
        protected Orientation Orientation
        {
            get
            {
                if(_orientation == Orientation.Undefined)
                    _orientation = Activity.Resources.Configuration.Orientation;
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _inflater = inflater;
            _container = container;
            _savedInstanceState = savedInstanceState;
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            Orientation = newConfig.Orientation;
            var viewGroup = (ViewGroup)View;
            viewGroup.RemoveAllViewsInLayout();
            var inflater = LayoutInflater.From(Activity);
            var view = OnCreateView(inflater, viewGroup, null);
            viewGroup.AddView(view);
        }
    }
}