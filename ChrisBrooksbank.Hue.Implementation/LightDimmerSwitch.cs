using ChrisBrooksbank.Hue.Interfaces;
using System;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightDimmerSwitch : ILightDimmerSwitch
    {

        IHueDotNetConfigurationReader HueConfiguration;

        public LightDimmerSwitch(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public void BrightenAll(ushort TransitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void BrightenGroup(string groupName, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void BrightenLight(string lightName, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void DimAll(ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void DimGroup(string groupName, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void DimLight(string lightName, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void SetBrightnessAll(byte brightness, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void SetBrightnessGroup(string groupName, byte brightness, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void SetBrightnessLight(string lightName, byte brightness, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }
    }
}
