using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.Core.Location;
using ChileafDemo.Chileaf;
using ChileafDemo.Droid;
using Java.Util.Concurrent.Atomic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static AndroidX.Activity.Result.Contract.ActivityResultContracts;

[assembly: Dependency(typeof(SensorHelper))]
namespace ChileafDemo.Droid
{
    public class SensorHelper : ISensorHelper
    {
        private static BroadcastReceiver _receiverBound;
        private static Action<bool> boundedAction;

        private readonly int REQUEST_PERMISSIONS = 222;

        private static readonly AtomicBoolean ready = new AtomicBoolean(false);
        private static Action<bool> sensorReadyAction;

        public void EnableSensor(Action<bool> enabled)
        {
            sensorReadyAction = enabled;
            RequestPermissions();
        }

        private void RequestPermissions()
        {
            //ActivityCompat.requestPermissions(ctx, new String[] { Manifest.permission.BLUETOOTH_SCAN, Manifest.permission.BLUETOOTH_CONNECT }, 2);


            bool permissionsGranted = Build.VERSION.SdkInt >= BuildVersionCodes.S ?
                (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothScan) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothConnect) == (int)Permission.Granted) :
                (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.Bluetooth) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessCoarseLocation) == (int)Permission.Granted);

            if (permissionsGranted)
            {
                TurnOnBT();
            }
            else
            {
                MessagingCenter.Subscribe<MainActivity, bool>(this, "RequestPermissions", (sender, granted) =>
                {
                    if (granted)
                    {
                        TurnOnBT();
                    }
                    else
                    {
                        ready.Set(false);
                        InvokeSensorsReady();
                    }
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "RequestPermissions");
                });
                String[] permissions = Build.VERSION.SdkInt >= BuildVersionCodes.S ?
                    new String[] { Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothConnect } :
                    new String[] { Manifest.Permission.Bluetooth, Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };
                ActivityCompat.RequestPermissions((Activity)MainActivity.Context, permissions, REQUEST_PERMISSIONS);
            }
        }

        private void TurnOnBT()
        {
            BluetoothManager BTManager = MainActivity.Context.GetSystemService(Context.BluetoothService) as BluetoothManager;
            if (!BTManager.Adapter.IsEnabled)
            {
                MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableBT", (sender, granted) =>
                {
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableBT");
                    if (granted)
                    {
                        TurnOnGps();
                    }
                    else
                    {
                        ready.Set(false);
                        InvokeSensorsReady();
                    }
                });
                Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                ((MainActivity)MainActivity.Context).StartActivityForResult(enableBT, 555);


            }
            else
            {
                TurnOnGps();
            }

        }

        public async void TurnOnGps()
        {
            try
            {
                LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
                if (!LocationManagerCompat.IsLocationEnabled(locationManager))
                {
                    if (IsGooglePlayServicesAvailable(MainActivity.Context))
                    {
                        GoogleApiClient googleApiClient = new GoogleApiClient.Builder((MainActivity)MainActivity.Context).AddApi(LocationServices.API).Build();
                        googleApiClient.Connect();
                        Android.Gms.Location.LocationRequest locationRequest = Android.Gms.Location.LocationRequest.Create();
                        locationRequest.SetPriority(Android.Gms.Location.LocationRequest.PriorityLowPower);
                        locationRequest.SetInterval(5);
                        locationRequest.SetFastestInterval(5);

                        LocationSettingsRequest.Builder
                            locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                            .AddLocationRequest(locationRequest);
                        locationSettingsRequestBuilder.SetAlwaysShow(false);

                        LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                            googleApiClient, locationSettingsRequestBuilder.Build());

                        MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableGPS", (sender, granted) =>
                        {
                            MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                            if (granted)
                            {
                                ready.Set(true);
                                InvokeSensorsReady();
                            }
                            else
                            {
                                ready.Set(false);
                                InvokeSensorsReady();
                            }
                        });

                        if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                        {
                            MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableGPS", (sender, granted) =>
                            {
                                MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                                if (granted)
                                {
                                    ready.Set(true);
                                    InvokeSensorsReady();
                                }
                                else
                                {
                                    ready.Set(false);
                                    InvokeSensorsReady();
                                }
                            });
                            locationSettingsResult.Status.StartResolutionForResult((MainActivity)MainActivity.Context, 666);
                        }
                        else
                        {
                            ready.Set(true);
                            InvokeSensorsReady();
                        }
                    }
                    else
                    {
                        Intent intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                        ((MainActivity)MainActivity.Context).StartActivity(intent);
                    }
                }
                else
                {
                    ready.Set(true);
                    InvokeSensorsReady();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        private static void InvokeSensorsReady()
        {
            sensorReadyAction?.Invoke(ready.Get());
        }

        public bool IsGooglePlayServicesAvailable(Context context)
        {
            GoogleApiAvailability googleApiAvailability = GoogleApiAvailability.Instance;
            int resultCode = googleApiAvailability.IsGooglePlayServicesAvailable(context);
            return resultCode == ConnectionResult.Success;
        }
    }
}