using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        IColourQuery ColourQuery;

        public LightColourSwitch(IHueDotNetConfigurationReader hueConfiguration, ILightQuery lightQuery, IColourQuery colourQuery)
        {
            HueConfiguration = hueConfiguration;
            LightQuery = lightQuery;
            ColourQuery = colourQuery;
        }

        public async Task SetColourAllAsync(NamedColour namedColour)
        {
            Dictionary<string,ILight> lights = await LightQuery.GetLightsAsync();
            foreach(var lightAndName in lights)
            {
                ILight light = lightAndName.Value;
                Gamut gamut = GetGamut(light);
                INamedColourDetail namedColourDetail = ColourQuery.GetNamedColourDetail(namedColour);

                string xyString = string.Empty;
                switch(gamut)
                {
                    case Gamut.A:
                        xyString = namedColourDetail.GamutA;
                        break;
                    case Gamut.B:
                        xyString = namedColourDetail.GamutB;
                        break;
                    case Gamut.C:
                        xyString = namedColourDetail.GamutC;
                        break;
                }

                Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/lights/" + light.ID + "/state");
                StringContent requestContent = new StringContent("{\"xy\" : " + xyString + "}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                }

            }

        }

        public async Task SetColourGroupAsync(NamedColour namedColour)
        {
            throw new NotImplementedException();
        }

        public async Task SetColourLightAsync(NamedColour namedColour)
        {
            throw new NotImplementedException();
        }

        private Gamut GetGamut( ILight light )
        {
            Gamut gamut = Gamut.none;

            switch (light.ModelID)
            {
                case "LST001":
                case "LLC010":
                case "LLC011":
                case "LLC012":
                case "LLC006":
                case "LLC007":
                case "LLC013":
                    gamut = Gamut.A;
                    break;

                case "LCT001":
                case "LCT002":
                case "LCT003":
                case "LCT007" :
                case "LLM001":
                    gamut = Gamut.B;
                    break;

                case "LCT010" :
                case "LCT014" :
                case "LCT011" :
                case "LST002" :
                    gamut = Gamut.C;
                    break;
            }

            return gamut;
        }

        public enum Gamut { none, A, B, C};
    }
}
