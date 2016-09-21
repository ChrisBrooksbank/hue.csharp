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
            Dictionary<string, ILightDescription> lightDescriptions = await LightQuery.GetLightDescriptionsAsync();
            foreach(var lightDescriptionAndID in lightDescriptions)
            {
                Gamut gamut = GetGamut(lightDescriptionAndID.Value);
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

                if (!string.IsNullOrEmpty(xyString))
                {
                    string lightID = await LightQuery.GetLightIDAsync(lightDescriptionAndID.Value.Name);
                    Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/lights/" + lightID + "/state");
                    StringContent requestContent = new StringContent("{\"xy\" : " + xyString + "}");
                    HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };

                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.SendAsync(request);
                    }
                }

            }

        }

        public async Task SetColourLightAsync(NamedColour namedColour, string lightName)
        {
            string lightID = await LightQuery.GetLightIDAsync(lightName);

            Dictionary<string, ILightDescription> lightDescriptions = await LightQuery.GetLightDescriptionsAsync();
            ILightDescription lightDescription = lightDescriptions[lightName];

            Gamut gamut = GetGamut(lightDescription);
            INamedColourDetail namedColourDetail = ColourQuery.GetNamedColourDetail(namedColour);

            string xyString = string.Empty;

            switch (gamut)
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

            if (!string.IsNullOrEmpty(xyString))
            {
                Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/lights/" + lightID + "/state");
                StringContent requestContent = new StringContent("{\"xy\" : " + xyString + "}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                }
            }

        }

        private Gamut GetGamut(ILightDescription lightDescription )
        {
            Gamut gamut = Gamut.none;

            switch (lightDescription.ModelID)
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
