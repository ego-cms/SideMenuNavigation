using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using SideMenuNavigation.Helpers;

namespace SideMenuNavigation
{
    public delegate void ItemClickEventHandler(int id);
    public delegate void MenuItemsEventHandler(ButtonType buttonType);

    public class NavigationFragmentLayout : LinearLayout
    {
        #region fields

        private const int ShadowSize = 15;
        private const int DefaultXPosition = 0;
        private const float OpenedRegardingXPosition = 0.5f;
        private const float DefaultScale = 1f;
        private const float OpenedScale = 0.8f;
        private const string StartBackgroundColor = "#4CA1A3";
        private const string EndBackgroundColor = "#4F4BA2";

        private const float StartScale = 1f;
        private const float EndScale = 0.9f;
        private const float PivotScale = 0.5f;
        private const int ScaleDuration = 100;
        private const int ToggleStateDuration = 400;

        private LinearLayout _container;
        private RelativeLayout _containerActionBar;
        private RecyclerView _menusRecyclerView;
        private ImageView _leftButton;
        private ImageView _rightButton;
        private ImageView _accountImageView;
        private TextView _accountTextView;

        private SwipeListener _swipeListener = new SwipeListener();

        private bool _menuitemsClickSubscribed;
        private bool _toggleFragmentsSubscribed;

        private List<NavigationFragmentModel> _navigationFragmentMenus;
        private FragmentManager _fragmentManager;

        #endregion

        #region properties

        public bool IsMenuVisible { get; private set; }
        private NavigationAdapter NavigationAdapter => _menusRecyclerView?.GetAdapter() as NavigationAdapter;
        private DisplayMetrics Metrics => Context.Resources.DisplayMetrics;
        public bool IsBusy { get; private set; }

        #endregion

        #region events

        public event ItemClickEventHandler ItemClicked;
        public event MenuItemsEventHandler MenuItemClicked;

        #endregion

        #region constructors

        public NavigationFragmentLayout(Context context) : base(context)
        {
            Initialize();
        }

        public NavigationFragmentLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public NavigationFragmentLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public NavigationFragmentLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected NavigationFragmentLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        #region customization

        //TODO: Customize RecycleView holders

        public Color NormalTextColor
        {
            get => NavigationAdapter.NormalTextColor;
            set => NavigationAdapter.NormalTextColor = value;
        }

        public Color SelectedTextColor
        {
            get => NavigationAdapter.SelectedTextColor;
            set => NavigationAdapter.SelectedTextColor = value;
        }

        public Color NormalIconColor
        {
            get => NavigationAdapter.NormalIconColor;
            set => NavigationAdapter.NormalIconColor = value;
        }

        public Color SelectedIconColor
        {
            get => NavigationAdapter.SelectedIconColor;
            set => NavigationAdapter.SelectedIconColor = value;
        }

        public Color SelectedBackgroundColor
        {
            get => NavigationAdapter.SelectedBackgroundColor;
            set => NavigationAdapter.SelectedBackgroundColor = value;
        }

        public List<NavigationModel> NavigationModels
        {
            get => NavigationAdapter.Items;
        }

        public int FragmentContainerId
        {
            get => Resource.Id.fragment_container;
        }

        private void SetFullscreenBackground(Color startColor, Color endColor)
        {
            (Context as Activity).Window.SetBackgroundDrawable(new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { startColor, endColor }));
        }

        private Color _fragmentActionBarColor;
        public Color FragmentActionBarColor
        {
            get => _fragmentActionBarColor;
            set
            {
                _fragmentActionBarColor = value;
                _containerActionBar.SetBackgroundColor(_fragmentActionBarColor);
            }
        }

        private Color _fragmentBackgroundColor;
        public Color FragmentBackgroundColor
        {
            get => _fragmentBackgroundColor;
            set
            {
                _fragmentBackgroundColor = value;
                _container.SetBackgroundColor(_fragmentBackgroundColor);
            }
        }

        private Color _leftIconColor;
        public Color LeftIconColor
        {
            get => _leftIconColor;
            set
            {
                _leftIconColor = value;
                _leftButton.Drawable.SetColorFilter(_leftIconColor, PorterDuff.Mode.SrcIn);
            }
        }

        private Color _rightIconColor;
        public Color RightIconColor
        {
            get => _rightIconColor;
            set
            {
                _rightIconColor = value;
                _rightButton.Drawable.SetColorFilter(_rightIconColor, PorterDuff.Mode.SrcIn);
            }
        }

        private Drawable _leftIcon;
        public Drawable LeftIcon
        {
            get => _leftIcon;
            set
            {
                _leftIcon = value;
                _leftButton.SetImageDrawable(_leftIcon);
            }
        }

        private Drawable _rightIcon;
        public Drawable RightIcon
        {
            get => _rightIcon;
            set
            {
                _rightIcon = value;
                _rightButton.SetImageDrawable(_rightIcon);
            }
        }

        private Drawable _accountIcon;
        public Drawable AccountIcon
        {
            get => _accountIcon;
            set
            {
                _accountIcon = value;
                _accountImageView.SetImageDrawable(_accountIcon);
            }
        }

        private Color _accountIconColor;
        public Color AccountIconColor
        {
            get => _accountIconColor;
            set
            {
                _accountIconColor = value;
                _accountImageView.SetColorFilter(_accountIconColor, PorterDuff.Mode.SrcIn);
            }
        }

        public string AccountTitle
        {
            get => _accountTextView.Text;
            set => _accountTextView.Text = value;
        }

        private Color _accountTextColor;
        public Color AccountTextColor
        {
            get => _accountTextColor;
            set
            {
                _accountTextColor = value;
                _accountTextView.SetTextColor(_accountTextColor);
            }
        }

        private Typeface _titleTypeface;
        public Typeface TitleTypeface
        {
            get => _titleTypeface;
            set
            {
                _titleTypeface = value;
                _accountTextView.Typeface = value;
            }
        }

        public Typeface ItemTypeface
        {
            get => NavigationAdapter.Typeface;
            set => NavigationAdapter.Typeface = value;
        }

        #endregion

        #region overrides

        public override ViewGroup.LayoutParams LayoutParameters => base.LayoutParameters;

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            var forward = false;
            if(_swipeListener.IsSwipedHorizontal(ref forward, ev) && CanToggleMenuState(forward))
            {
                ToggleMenuState();
                return true;
            }

            return base.OnInterceptTouchEvent(ev);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            //If device was rotate fragment container position should be recalculate
            if(IsMenuVisible && _container.GetX() != 0)
                NormalizeFragmentContainerPosition();
        }

        #endregion

        #region public api

        /// <summary>
        /// Setup navigation layout by data of selectable items
        /// </summary>
        /// <param name="navigationMenus">Model which contains data for selectable items</param>
        public void SetupNavigation(List<NavigationModel> navigationMenus)
        {
            var adapter = new NavigationAdapter(navigationMenus);

            if(!_menuitemsClickSubscribed)
            {
                adapter.ItemClicked += AdapterItemClicked;
                _menuitemsClickSubscribed = true;
            }

            if(_toggleFragmentsSubscribed)
            {
                adapter.ItemClicked -= ToggleMenuWithFragments;
                _toggleFragmentsSubscribed = false;
            }
            _navigationFragmentMenus = null;
            _fragmentManager = null;

            _menusRecyclerView.SetAdapter(adapter);
        }


        /// <summary>
        /// Setup navigation layout by data of selectable items and fragments
        /// </summary>
        /// <param name="navigationMenus">Model which contains data for selectable items and fragments</param>
        /// <param name="fragmentManager">FragmentManager of parent activity</param>
        public void SetupNavigation(List<NavigationFragmentModel> navigationFragmentMenus, FragmentManager fragmentManager)
        {
            var navigationMenus = navigationFragmentMenus.Select(item => item.NavigationModel).ToList();
            var first = navigationMenus.FirstOrDefault();
            first.Selected = true;
            var adapter = new NavigationAdapter(navigationMenus);

            _navigationFragmentMenus = navigationFragmentMenus;
            _fragmentManager = fragmentManager;

            if(!_toggleFragmentsSubscribed)
            {
                adapter.ItemClicked += ToggleMenuWithFragments;
                _toggleFragmentsSubscribed = false;
            }

            if(_menuitemsClickSubscribed)
            {
                adapter.ItemClicked -= AdapterItemClicked;
                _menuitemsClickSubscribed = false;
            }

            _menusRecyclerView.SetAdapter(adapter);

            ToggleFragments(first.Id);
        }

        #endregion

        #region utility methods

        private void Initialize()
        {
            //hide toolbar if it's exist
            (Context as AppCompatActivity)?.SupportActionBar?.Hide();
            (Context as Activity).ActionBar?.Hide();

            //setup statusbar
            (Context as Activity).Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            (Context as Activity).Window.AddFlags(WindowManagerFlags.TranslucentStatus);

            //set default fuullscreen background
            (Context as Activity).Window.SetBackgroundDrawable(new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { Color.ParseColor(StartBackgroundColor), Color.ParseColor(EndBackgroundColor) }));

            SetupControlSize();

            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.navigation_layout, this);

            _container = view.FindViewById<LinearLayout>(Resource.Id.container);
            _containerActionBar = view.FindViewById<RelativeLayout>(Resource.Id.actionBarContainer);
            _leftButton = view.FindViewById<ImageView>(Resource.Id.left_button);
            _rightButton = view.FindViewById<ImageView>(Resource.Id.right_button);
            _accountImageView = view.FindViewById<ImageView>(Resource.Id.account_icon_view);
            _accountTextView = view.FindViewById<TextView>(Resource.Id.account_name_view);

            _container.Elevation = ShadowSize * Metrics.Density;

            _container.SetPadding(PaddingLeft, PaddingTop + GetStatusBarHeight(), PaddingRight, PaddingBottom);

            _menusRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_menus);
            _menusRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            _menusRecyclerView.SetAdapter(new NavigationAdapter(new List<NavigationModel>()));

            _leftButton.Touch += (s, e) =>
            {
                TouchButtonHeandler(_leftButton, e, () => 
                {
                    MenuItemClicked?.Invoke(ButtonType.Left);
                    ToggleMenuState();
                });
            };

            _rightButton.Touch += (s, e) =>
            {
                TouchButtonHeandler(_rightButton, e, () =>
                {
                    MenuItemClicked?.Invoke(ButtonType.Right);
                });
            };
        }

        private void TouchButtonHeandler(View view, TouchEventArgs e, Action postAnimationAction)
        {
            if(e.Event.Action == MotionEventActions.Down)
            {
                view.StartAnimation(StartTouchAnimation(StartScale, EndScale));
            }
            else if(e.Event.Action == MotionEventActions.Cancel || e.Event.Action == MotionEventActions.Up)
            {
                var animation = StartTouchAnimation(EndScale, StartScale);
                animation.AnimationEnd += delegate
                {
                    postAnimationAction?.Invoke();
                };
                view.StartAnimation(animation);
            }
        }

        private bool CanToggleMenuState(bool forward)
        {
            if(IsBusy || (forward && IsMenuVisible) || (!forward && !IsMenuVisible))
                return false;

            return true;
        }

        private ScaleAnimation StartTouchAnimation(float startScale, float endScale)
        {
            var scaleInAnimation = new ScaleAnimation(startScale, endScale, startScale, endScale, Dimension.RelativeToSelf, PivotScale, Dimension.RelativeToSelf, PivotScale);
            scaleInAnimation.Duration = ScaleDuration;
            scaleInAnimation.FillAfter = true;
            return scaleInAnimation;
        }

        private void AdapterItemClicked(int id)
        {
            ItemClicked?.Invoke(id);
            ToggleMenuState();
        }

        private void ToggleMenuWithFragments(int id)
        {
            ToggleFragments(id);
            AdapterItemClicked(id);
        }

        private void ToggleFragments(int id)
        {
            var fragment = _navigationFragmentMenus.FirstOrDefault(item => item.NavigationModel.Id == id).Fragment;
            var transition = _fragmentManager.BeginTransaction();
            transition.Replace(FragmentContainerId, fragment);
            transition.Commit();
        }

        private void StartAnimation(int x, float scale)
        {
            var xProp = PropertyValuesHolder.OfFloat("X", x);
            var scaleXProp = PropertyValuesHolder.OfFloat("ScaleX", scale);
            var scaleYProp = PropertyValuesHolder.OfFloat("ScaleY", scale);

            var animator = ObjectAnimator.OfPropertyValuesHolder(_container, xProp, scaleXProp, scaleYProp);
            animator.SetDuration(ToggleStateDuration);
            animator.SetInterpolator(new LinearInterpolator());

            animator.AnimationEnd += delegate
            {
                IsMenuVisible = !IsMenuVisible;
                IsBusy = false;
            };

            animator.Start();
        }

        private void NormalizeFragmentContainerPosition()
        {
            var xProp = PropertyValuesHolder.OfFloat("X", (int)(Metrics.WidthPixels * OpenedRegardingXPosition));
            var animator = ObjectAnimator.OfPropertyValuesHolder(_container, xProp);
            animator.SetDuration(0);
            animator.SetInterpolator(new LinearInterpolator());
            animator.Start();
        }

        private int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = Context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if(resourceId > 0)
                result = Context.Resources.GetDimensionPixelSize(resourceId);

            return result;
        }

        private void ToggleMenuState()
        {
            IsBusy = true;
            if(IsMenuVisible)
                StartAnimation(DefaultXPosition, DefaultScale);
            else
                StartAnimation((int)(Metrics.WidthPixels * OpenedRegardingXPosition), OpenedScale);
        }

        //control shold be always fullscreen without margin and padding
        private void SetupControlSize()
        {
            var parameters = new MarginLayoutParams(Metrics.WidthPixels, Metrics.HeightPixels);
            parameters.SetMargins(0, 0, 0, 0);
            SetPadding(0, 0, 0, 0);
            base.LayoutParameters = parameters;
        }

        #endregion
    }
}