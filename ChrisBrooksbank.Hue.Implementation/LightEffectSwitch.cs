using ChrisBrooksbank.Hue.Interfaces;
using System;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightEffectSwitch : ILightEffectSwitch
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public LightEffectSwitch(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public void SetEffectAll(ILightEffect effect)
        {
            throw new NotImplementedException();
        }

        public void SetEffectGroup(string groupName, ILightEffect effect)
        {
            throw new NotImplementedException();
        }

        public void SetEffectLight(string lightName, ILightEffect effect)
        {
            throw new NotImplementedException();
        }
    }
}
