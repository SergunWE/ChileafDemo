using ChileafBleXamarin.DataTypes;
using ChileafBleXamarin.Interfaces;
using ChileafDemo.iOS.Chileaf;
using ChileafSDK;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApiProvider))]
namespace ChileafDemo.iOS.Chileaf
{
    public class ApiProvider : IApiProvider
    {
        private bool _searchRunning;
        private FitBLECentralManager _manager;
        private CustomBLECentralDelegate _searchCallback;

        public ApiProvider()
        {
            _manager = FitBLECentralManager.ShareInstance;
            _searchCallback = new CustomBLECentralDelegate();
            _manager._BLECentralDelegate = _searchCallback;
        }

        public void Connect(IBTDevice device)
        {
            
        }

        public void Disconnect()
        {
            
        }

        public int GetConnectionState()
        {
            return 0;
        }

        public ChileafDeviceInfo GetDeviceInfo()
        {
            return null;
        }

        public Task<ChileafDeviceInfo> GetDeviceInfoAsync()
        {
            return null;
        }

        public Task<string> GetSerialWithoutConnect(IBTDevice device)
        {
            return null;
        }

        public void SetApiCallback(IApiCallback callback)
        {
            
        }

        public void SetDebug(bool debug)
        {
            
        }

        public async void StartSearch(Action<List<IBTDevice>> deviceList, TimeSpan searchTime)
        {
            if (_searchRunning) throw new Exception("Search already running");
            _manager.CentralStartSaomiao();
            await Task.Delay(searchTime);
            StopSearch();
        }

        public void StopSearch()
        {
            _manager.BLESaomiaoStop();
            _searchRunning = false;
        }
    }
}