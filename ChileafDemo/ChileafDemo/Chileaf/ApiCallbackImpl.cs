using ChileafBleXamarin.DataTypes;
using ChileafBleXamarin.Interfaces;
using System;

namespace ChileafDemo.Chileaf
{
    public class ApiCallbackImpl : IApiCallback
    {
        public Action<int> BatteryAction { get; set; }
        public Action<bool> ConnectedAction { get; set; }
        public Action<ChileafHrData> HeartRateDataAction { get; set; }
        public Action<ChileafSportData> SportDataAction { get; set; }

        public void OnBatteryLevelChanged(int level)
        {
            BatteryAction?.Invoke(level);
        }

        public void OnDeviceConnected()
        {
            ConnectedAction?.Invoke(true);
        }

        public void OnDeviceDisconnected()
        {
            ConnectedAction?.Invoke(false);
        }

        public void OnHeartRateMeasurementReceived(ChileafHrData data)
        {
            HeartRateDataAction?.Invoke(data);
        }

        public void OnSportReceived(ChileafSportData data)
        {
            SportDataAction?.Invoke(data);
        }
    }
}
