using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NearToMe.Core.ViewModels;
using Android.Provider;
using Android.Gms.Maps.Model;
using NearToMe.Droid.Helpers;

namespace NearToMe.Droid.Views
{
    [Activity(Label = "MainView", Theme = "@style/NTMTheme")]
    public class MainView : BaseView<MainViewModel>,IOnMapReadyCallback, ILocationListener
    {
        LocationManager _locationManager;
        string _locationProvider;
        Boolean enabled;
        Location _currentLocation;
        private GoogleMap gmap;
        public Marker _marker;
        protected override int LayoutResource => Resource.Layout.MainView;

        public void OnMapReady(GoogleMap googleMap)
        {
            this.gmap = googleMap;
            gmap.UiSettings.ZoomControlsEnabled = true;
            gmap.UiSettings.ScrollGesturesEnabled = true;
            gmap.UiSettings.ZoomGesturesEnabled = true;
            gmap.MyLocationEnabled = true;
   
        }
        protected override void OnResume()
        {
            base.OnResume();
            enabled = _locationManager.IsProviderEnabled(_locationProvider);
             _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);
      
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetUI();
            InitializeLocationManager();
            EnableGPS();
            SetUpMap();

        }

        private void SetUpMap()
        {
            if(gmap==null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);
            }
        }
        public void InitializeLocationManager()
        {
            _locationManager = (LocationManager)this.GetSystemService(Service.LocationService);

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        public Boolean EnableGPS()
        {
            enabled = _locationManager.IsProviderEnabled(_locationProvider);

            if (!enabled)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Location Services Not Active");

                alert.SetMessage("Please enable Location Services and GPS");
                alert.SetPositiveButton("Ok", (c, ev) =>
                {
                    StartActivity(new Intent(Settings.ActionLocationSourceSettings));
                });
                alert.Show();
            }
            return enabled;
        }

        private void SetUI()
        {
            
        }

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            GlobalConst.lat = _currentLocation.Latitude;
            GlobalConst.lang = _currentLocation.Longitude;
            AddMarker();
        }

        private void AddMarker()
        {
            if (_currentLocation != null)
            {
                RunOnUiThread(() =>
                {
                    if (_marker != null)
                    {
                        _marker.Remove();
                    }
                    LatLng latlngnew = new LatLng(_currentLocation.Latitude, _currentLocation.Longitude);
                    MarkerOptions markerOption = new MarkerOptions()
                             .SetPosition(latlngnew)
                             .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                    _marker = gmap.AddMarker(markerOption);
                
                    gmap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(latlngnew, 13.0f));
                });
            }


        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
           // throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
           // throw new NotImplementedException();
        }
    }
}