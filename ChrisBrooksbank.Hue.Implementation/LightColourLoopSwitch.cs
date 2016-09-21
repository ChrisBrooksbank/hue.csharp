using System;
using System.Threading.Tasks;
using ChrisBrooksbank.Hue.Interfaces;
using System.Net.Http;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightColourLoopSwitch : ILightColourLoopSwitch
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public LightColourLoopSwitch(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public async Task TurnOffAllASync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"effect\" : \"none\"}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }

        public async Task TurnOnAllASync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"effect\" : \"colorloop\"}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }
    }
}
