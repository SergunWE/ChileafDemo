using System;
using System.Collections.Generic;
using System.Text;

namespace ChileafDemo.Chileaf
{
    public interface ISensorHelper
    {
        void EnableSensor(Action<bool> enabled);
    }
}
