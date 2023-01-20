using ChileafBleXamarin.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChileafBleXamarin.Interfaces
{
    public interface IApiProvider
    {
        void SetApiCallback(IApiCallback callback);
        void StartSearch(Action<List<IBTDevice>> deviceList, TimeSpan searchTime);
        void StopSearch();
        void Connect(IBTDevice device);
        void Disconnect();
        void SetDebug(bool debug);
        ChileafDeviceInfo GetDeviceInfo();
        Task<ChileafDeviceInfo> GetDeviceInfoAsync();
        int GetConnectionState();
        Task<string> GetSerialWithoutConnect(IBTDevice device);
    }
}
