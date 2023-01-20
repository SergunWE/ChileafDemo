using System;
using System.Collections.Generic;
using System.Text;

namespace ChileafBleXamarin.DataTypes
{
    public struct ChileafSportData
    {
        public int Step { get; set; }
        public int Distance { get; set; }
        public int Calorie { get; set; }

        public ChileafSportData(int step, int distance, int calorie)
        {
            Step = step;
            Distance = distance;
            Calorie = calorie;
        }
    }
}
