using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Com.Baidu.Mapapi.Map.BaiduMap;

namespace BaiduMap.Demo.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnMapLoadedCallback
    {
        MapView Map;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SDKInitializer.Initialize(ApplicationContext);

            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            Map = FindViewById<MapView>(Resource.Id.map);
            Map.OnCreate(this, savedInstanceState);
            Map.Map.SetOnMapLoadedCallback(this);
        }

        public void OnMapLoaded()
        {
            var marker = new MarkerOptions()
                    .InvokePosition(new LatLng(40.023537, 116.289429))
                    .InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.ic_launcher))
                    .Visible(true)
                    .Draggable(true);

            var circle = new CircleOptions()
                .InvokeCenter(new LatLng(43.023637, 116.287429))
                .InvokeFillColor(Color.Coral.ToArgb())
                .InvokeStroke(new Stroke(20, Color.Green.ToArgb()))
                .InvokeRadius(2000)
                .Visible(true);
            var points = new List<LatLng> {
                new LatLng(40.123537, 119.289429),
                new LatLng(42.124537, 116.289429),
                new LatLng(40.123537, 116.288429),
                new LatLng(41.123737, 117.289489)
            };
            var polygon = new PolygonOptions()
                .InvokeFillColor(Color.Red.ToArgb())
                .InvokePoints(points)
                .Visible(true);
            var pointsLine = new List<LatLng> {
                new LatLng(40.223537, 116.289429),
                new LatLng(40.124537, 116.489429),
                new LatLng(41.123537, 116.288429),
                new LatLng(41.123737, 117.289489)
            };
            var line = new PolylineOptions()
                .InvokePoints(pointsLine)
                .InvokeColor(Color.Blue.ToArgb())
                .InvokeWidth(20)
                .KeepScale(true)
                .Visible(true);
            Map.Map.AddOverlays(new List<OverlayOptions> { marker, circle, polygon, line });
            //var q = Map.Map.AddOverlay(marker);
            //var w = Map.Map.AddOverlay(circle);
            //var a = Map.Map.AddOverlay(polygon);
            //var r = Map.Map.AddOverlay(line);

            Map.Map.MarkerClick += (_, e) =>
            {
                e.Handled = true;
                var m = e.P0;
                ShowSnackbar("Marker " + m.Position.ToString());
            };

            Map.Map.PolylineClick += (s, e) =>
            {
                e.Handled = true;
                var latlng = e.P0.Points.Last();
                var random = new Random();
                var x = random.Next(1);
                var y = random.NextDouble();
                if (x % 2 == 0)
                    latlng.Latitude += y;
                else
                    latlng.Longitude += y;
                e.P0.Points.Add(latlng);
            };

            Map.Map.MapClick += (s, e) => ShowSnackbar("Map " + e.P0.ToString());
            Map.Map.MapDoubleClick += (s, e) => ShowSnackbar("MapDouble " + e.P0.ToString());
            Map.Map.MapLongClick += (s, e) => ShowSnackbar("MapLong " + e.P0.ToString());
            Map.Map.MapPoiClick += (s, e) => ShowSnackbar("MapPoi " + e.P0.ToString());
        }

        private void ShowSnackbar(string msg)
        {
            Snackbar.Make(Map, msg, Snackbar.LengthShort).Show();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Map?.OnResume();
        }

        protected override void OnPause()
        {
            Map?.OnPause();
            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            Map?.OnDestroy();
            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}

