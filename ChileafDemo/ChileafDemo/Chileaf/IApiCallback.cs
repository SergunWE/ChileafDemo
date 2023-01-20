using ChileafBleXamarin.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChileafBleXamarin.Interfaces
{
    public interface IApiCallback
    {
        void OnBatteryLevelChanged(int level);
        void OnDeviceConnected();
        void OnDeviceDisconnected();
        void OnHeartRateMeasurementReceived(ChileafHrData data);
        void OnSportReceived(ChileafSportData data);
    }
}
