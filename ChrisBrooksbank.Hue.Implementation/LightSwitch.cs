using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task TurnOffAllAsync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"on\" : false}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }

        public async Task TurnOffLightAsync(string lightName)
        {
            string lightID = await LightQuery.GetLightIDAsync(lightName);

            if (!string.IsNullOrEmpty(lightID))
            {
                Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/lights/" + lightID + "/state");
                StringContent requestContent = new StringContent("{\"on\" : false}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                }

            }

            return;
        }

        public async Task TurnOnAllAsync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"on\" : true}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
            
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }

        public async Task TurnOnLightAsync(string lightName)
        {
            string lightID = await LightQuery.GetLightIDAsync(lightName);

            if (!string.IsNullOrEmpty(lightID))
            {
                Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/lights/" + lightID + "/state");
                StringContent requestContent = new StringContent("{\"on\" : true}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                }

            }

            return;
        }

    }
}
