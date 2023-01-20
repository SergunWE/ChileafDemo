
using ChileafBleXamarin.Interfaces;
using Com.Android.Chileaf.Wear;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using static Android.Bluetooth.BluetoothClass;
using System.Threading.Tasks;
using ChileafBleXamarin.DataTypes;
using Android.Content;
using System.Threading;

using System.Diagnostics;
using ChileafDemo.Droid;
using ChileafDemo.Droid.Chileaf;

[assembly: Dependency(typeof(ApiProvider))]
namespace ChileafDemo.Droid.Chileaf
{
    public class ApiProvider : IApiProvider
    {
        private static WearManager _wearManager;
        private readonly ChileafApiCallback _chileafApiCallback;
        private bool _searchRunning;

        public ApiProvider()
        {
            _wearManager = WearManager.GetInstance(ChileafSDK.Context);
            _chileafApiCallback = new ChileafApiCallback();
            _wearManager.SetManagerCallbacks(_chileafApiCallback);
        }

        public void Connect(IBTDevice device)
        {
            _wearManager.Connect((Android.Bluetooth.BluetoothDevice)device.Native, true);
        }

        public void Disconnect()
        {
            _wearManager.DisConnect();

        }

        public int GetConnectionState()
        {
            return _wearManager.ConnectionState;
        }

        public ChileafDeviceInfo GetDeviceInfo()
        {
            return _chileafApiCallback.DeviceInfo;
        }

        public async Task<ChileafDeviceInfo> GetDeviceInfoAsync()
        {
            if(await WaitingToConnect() && await WaitingForReadiness())
            {
                return _chileafApiCallback.DeviceInfo;
            }
            return null;
        }

        public async Task<string> GetSerialWithoutConnect(IBTDevice device)
        {
            Stopwatch stopwatch;
            Connect(device);

            //Waiting for connection to the device
            stopwatch = Stopwatch.StartNew();
            while (!_wearManager.IsConnected)
            {
                if(stopwatch.ElapsedMilliseconds > 3000)
                {
                    Disconnect();
                    return null;
                }
                await Task.Delay(300);
            }

            //wait until the device is ready (all information is loaded)
            stopwatch = Stopwatch.StartNew();
            while (_wearManager.IsConnected && !_wearManager.IsReady)
            {
                if (stopwatch.ElapsedMilliseconds > 7000)
                {
                    Disconnect();
                    return null;
                }
                await Task.Delay(300);
            }

            string serial = _wearManager.SerialNumber;
            Disconnect();
            return string.IsNullOrEmpty(serial) ? "Serial number not received" : serial;
        }

        public void SetApiCallback(IApiCallback callback)
        {
            _chileafApiCallback.Callback = callback;
        }

        public void SetDebug(bool debug)
        {
            _wearManager.SetDebug(debug);
        }

        public async void StartSearch(Action<List<IBTDevice>> deviceList, TimeSpan searchTime)
        {
            if (_searchRunning) throw new System.Exception("Search already running");
            _searchRunning = true;
            CustomScanCallback callback = new CustomScanCallback();
            _wearManager.StartScan(callback);
            await Task.Delay(searchTime);
            StopSearch();
            deviceList?.Invoke(callback.FoundedDevices);
        }

        public void StopSearch()
        {
            _wearManager.StopScan();
            _searchRunning = false;
        }

        private async Task<bool> WaitingToConnect()
        {
            var stopwatch = Stopwatch.StartNew();
            while (!_wearManager.IsConnected)
            {
                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    return false;
                }
                await Task.Delay(300);
            }
            return true;
        }

        private async Task<bool> WaitingForReadiness()
        {
            var stopwatch = Stopwatch.StartNew();
            while (_wearManager.IsConnected && !_wearManager.IsReady)
            {
                if (stopwatch.ElapsedMilliseconds > 7000)
                {
                    return false;
                }
                await Task.Delay(300);
            }
            return true;
        }
    }
}