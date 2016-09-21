using System;
using System.Threading.Tasks;
using ChrisBrooksbank.Hue.Interfaces;
using System.Net.Http;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightFlashSwitch : ILightFlashSwitch
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public LightFlashSwitch(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public async Task FlashAllOnceAsync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"alert\" : \"select\"}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }

        public async Task FlashAllSeveralTimesASync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"alert\" : \"lselect\"}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }
    }
}
