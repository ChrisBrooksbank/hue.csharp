using ChrisBrooksbank.Hue.Interfaces;
using System;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightSwitch : ILightSwitch
    {
        IHueDotNetConfigurationReader HueConfiguration;
        ILightQuery LightQuery;

        public LightSwitch( IHueDotNetConfigurationReader hueConfiguration, ILightQuery lightQuery)
        {
            HueConfiguration = hueConfiguration;
            LightQuery = lightQuery;
        }

        public void TurnOffAll()
        {
            throw new NotImplementedException();
        }

        public void TurnOffGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TurnOffLIght(string lightName)
        {
            throw new NotImplementedException();
        }

        public void TurnOnAll()
        {
            throw new NotImplementedException();
        }

        public void TurnOnGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public async void TurnOnLight(string lightName)
        {
            throw new NotImplementedException();
        }

    }
}
