using ChileafDemo.Chileaf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChileafBleXamarin.DataTypes
{
    public class ChileafDeviceInfo
    {
        public int Rssi { get; set; }
        public int BatteryLevel { get; set; }
        public string SystemId { get; set; }
        public string ModelName { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string HardwareVersion { get; set; }
        public string SoftwareVersion { get; set; }
        public string VendorName { get; set; }
        public SensorLocation SensorLocation { get; set; }
    }
}
