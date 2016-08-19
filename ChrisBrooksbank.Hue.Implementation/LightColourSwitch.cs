using ChrisBrooksbank.Hue.Interfaces;
using System;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightColourSwitch : ILightColourSwitch
    {

        IHueDotNetConfigurationReader HueConfiguration;

        public LightColourSwitch(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public void SetColourAll(ILightColour colour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void SetColourGroup(ILightColour colour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public void SetColourLight(ILightColour colour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }
    }
}
