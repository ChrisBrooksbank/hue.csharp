using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class BridgeCommand : IBridgeCommand
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public BridgeCommand(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public async Task<string> CreateNewUser()
        {
            string newUser = string.Empty;

            using (var client = new HttpClient())
            {
                Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api");
                StringContent requestContent = new StringContent("{\"devicetype\":\"" + HueConfiguration.ApplicationName + "\"}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = RequestUri, Content = requestContent };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.Contains("link button not pressed"))
                    {
                        throw new ELinkButtonNotPressed();
                    }

                    // TODO need to parse out newUser
                    newUser = responseString;
                }
            }

            return newUser;
        }

        public async Task<bool> Ping()
        {
            bool bridgeChecked = false;

            string signature = "<modelDescription>Philips hue Personal Wireless Lighting</modelDescription>";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("description.xml");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    bridgeChecked = responseString.Contains(signature);
                }

            }


            return bridgeChecked;
        }
    }
}
