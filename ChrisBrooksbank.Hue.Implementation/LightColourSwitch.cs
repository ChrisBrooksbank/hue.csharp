using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class NamedColourDetail : INamedColourDetail
    {
        public string Colour { get; set; }
        public string RGB { get; set; }
        public string GamutA { get; set; }
        public string GamutB { get; set; }
        public string GamutC { get; set; }
    }


    public class LightColourSwitch : ILightColourSwitch
    {
        IHueDotNetConfigurationReader HueConfiguration;
        ILightQuery LightQuery;

        public LightColourSwitch(IHueDotNetConfigurationReader hueConfiguration, ILightQuery lightQuery)
        {
            HueConfiguration = hueConfiguration;
            LightQuery = lightQuery;
        }

      


        public async Task SetColourAllAsync(NamedColour namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public async Task SetColourGroupAsync(NamedColour namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public async Task SetColourLightAsync(NamedColour namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }
    }
}
