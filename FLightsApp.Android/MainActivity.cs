using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using FFImageLoading.Forms.Droid;
using static Android.Provider.SyncStateContract;

namespace FLightsApp.Droid
{
    [Activity(Label = "FLightsApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#ff0000"));
            base.OnCreate(bundle);
			Rg.Plugins.Popup.Popup.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
			CachedImageRenderer.Init();
			UserDialogs.Init(this);
            LoadApplication(new App());
        }
    }
}

