using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightNamedColour : ILightNamedColour
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

        public IEnumerable<ILightNamedColour> GetNamedColours()
        {
            string namedColourFileName = "NamedColours.json";

            if ( !File.Exists(namedColourFileName))
            {
                throw new ApplicationException("missing named colours file " + namedColourFileName );
            }

            string namedColoursJSON = File.ReadAllText(namedColourFileName);

            List<LightNamedColour> namedColours = JsonConvert.DeserializeObject<List<LightNamedColour>>(namedColoursJSON);

            return namedColours;
        }



        public async Task SetColourAllAsync(string namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public async Task SetColourGroupAsync(string namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }

        public async Task SetColourLightAsync(string namedColour, ushort transitionTimeIn100MS = 1)
        {
            throw new NotImplementedException();
        }
    }
}
