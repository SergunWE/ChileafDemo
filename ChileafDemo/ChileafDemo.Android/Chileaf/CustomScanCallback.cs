using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChileafBleXamarin.Interfaces;
using Com.Android.Chileaf.Bluetooth.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChileafDemo.Droid.Chileaf
{
    public class CustomScanCallback : ScanCallback
    {
        private Dictionary<string, IBTDevice> _foundedDevices;
        public List<IBTDevice> FoundedDevices => _foundedDevices.Values.ToList();

        public CustomScanCallback()
        {
            _foundedDevices = new Dictionary<string, IBTDevice>();
        }

        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            base.OnBatchScanResults(results);
            foreach (var result in results)
            {
                System.Console.WriteLine(result.Device.Name);
                _foundedDevices[result.Device.Address] = new BTDeviceWrapper(result.Device);
            }
        }

        public override void OnScanResult(int callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);
            _foundedDevices[result.Device.Address] = new BTDeviceWrapper(result.Device);
            System.Console.WriteLine(result.Device.Name);
        }
    }
}