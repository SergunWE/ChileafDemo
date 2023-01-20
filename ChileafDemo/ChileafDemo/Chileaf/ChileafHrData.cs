using System;
using System.Collections.Generic;
using System.Text;

namespace ChileafBleXamarin.DataTypes
{
    public struct ChileafHrData
    {
        public int HeartRate { get; set; }
        public bool? ContactDetected { get; set; }
        public int? EnergyExpanded { get; set; }
        public List<int> RrIntervals { get; set; }
    }
}
