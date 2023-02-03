using ChileafSDK;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace ChileafDemo.iOS.Chileaf
{
    public class CustomBLECentralDelegate : BLECentralDelegate
    {
        public override void FitScanDeviceArray(NSMutableArray DeviceArr)
        {
            Console.WriteLine(DeviceArr.ToString());
        }
    }
}