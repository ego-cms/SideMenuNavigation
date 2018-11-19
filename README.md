[![eGo-CMS](https://rawgithub.com/ego-cms/Resources/master/Badges_by_EGO/by_EGO.svg)](http://ego-cms.com/?utm_source=github)

# NavigationFragmentLayout
======================================================

## Table of contents
* [Introduction](../..#introduction)
* [Requirements](../..#requirements)
* [Documentation](../..#documentation)
* [Sample](../..#sample)

<img src="Resources/animation.gif" width="400"/>&nbsp;&nbsp;&nbsp;

Introduction
-----------

NavigationFragmentLayout is layout which containes logic for contain and navigate fragments. It contains list of fragment with icon and title, account icon and title and fragments container with navigation bar. Navigation bar contains 2 buttons: left and right side. Left side button open and close fragments list visibility. Right button does not have any action, it's only do call event by click.

Requirements
-----------
- Xamarin.Android
- Android API 21+

Documentation
-----------

- event ItemClickEventHandler ItemClicked - fragment list item clicked event;
- delegate void ItemClickEventHandler(int id) - delegate for handle fragment list item click;
- event MenuItemsEventHandler MenuItemClicked - navigation button clicked event;
- delegate void MenuItemsEventHandler(ButtonType buttonType) - delegate for handle navigation button click;
- Color NormalTextColor - color of fragment list item text for normal state;
- Color SelectedTextColor - color of fragment list item text for selected state;
- Color NormalIconColor - color of fragment list item icon for normal state;
- Color SelectedIconColor - color of fragment list item icon for selected state;
- Color SelectedBackgroundColor - color of fragment list item background for selected state (for normal state background always transparent);
- List<NavigationModel> NavigationModels - models for customization fragments list items;
- int FragmentContainerId - id of fragments container;
- void SetFullscreenBackground(Color startColor, Color endColor) - set left to right linear gradient color for background;
- Color FragmentActionBarColor - color of navigation bar;
- Color FragmentBackgroundColor - color of status bar and background under fragment container;
- Color LeftIconColor - color of left side navigation bar button;
- Color RightIconColor - color of right side navigation bar button;
- Drawable LeftIcon - icon of left side navigation bar button;
- Drawable RightIcon - icon of right side navigation bar button;
- Color AccountIconColor - color of account picture;
- Drawable AccountIcon - icon of account picture;
- string AccountTitle - text of account title;
- Color AccountTextColor - text color of account title;
- Typeface TitleTypeface - text typeface of account title;
- Typeface ItemTypeface - text typeface of fragments list item;

- void SetupNavigation(List<NavigationModel> navigationMenus) - method of setup customization fragments list without auto change fragments;
- void SetupNavigation(List<NavigationFragmentModel> navigationFragmentMenus, FragmentManager fragmentManager) - method of setup customization fragments list with auto change fragments;

#Models
```c#
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
```

Sample
-----------

Sample of NavigationFragmentLayout customization in main Activity
```c#
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
        new NavigationFragmentModel(new NavigationModel(3, "Scale", BaseContext.GetDrawable(Resource.Drawable.percent)), new ScaleFragment()),
        new NavigationFragmentModel(new NavigationModel(4, "Catalog", BaseContext.GetDrawable(Resource.Drawable.grid)), new CatalogFragment()),
        new NavigationFragmentModel(new NavigationModel(5, "Orders", BaseContext.GetDrawable(Resource.Drawable.wallet)), new OrdersFragment()),
        new NavigationFragmentModel(new NavigationModel(6, "Info", BaseContext.GetDrawable(Resource.Drawable.information)), new InformationFragment()),
        new NavigationFragmentModel(new NavigationModel(7, "Settings", BaseContext.GetDrawable(Resource.Drawable.settings)), new SettingsFragment()),
    };
}
```

Sample of activity_main.axml of main Activity
```xml
<?xml version="1.0" encoding="utf-8"?>
<EOSNavigation.NavigationFragmentLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:id="@+id/navigation_container"
    android:layout_width="match_parent"
    android:layout_height="match_parent"/>
```


[![License](https://rawgit.com/ego-cms/Resources/master/License/license.svg)](LICENSE)