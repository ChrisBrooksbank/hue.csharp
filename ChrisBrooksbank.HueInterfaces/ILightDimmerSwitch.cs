using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightDimmerSwitch
    {
        void DimAll(UInt16 transitionTimeIn100MS = 1);
        void DimGroup(string groupName, UInt16 transitionTimeIn100MS= 1);
        void DimLight(string lightName, UInt16 transitionTimeIn100MS = 1);
        void BrightenAll(UInt16 TransitionTimeIn100MS = 1);
        void BrightenGroup(string groupName, UInt16 transitionTimeIn100MS = 1);
        void BrightenLight(string lightName, UInt16 transitionTimeIn100MS = 1);
        void SetBrightnessAll(byte brightness, UInt16 transitionTimeIn100MS = 1);
        void SetBrightnessLight(string lightName, byte brightness, UInt16 transitionTimeIn100MS = 1);
        void SetBrightnessGroup(string groupName, byte brightness, UInt16 transitionTimeIn100MS = 1);
    }
}
