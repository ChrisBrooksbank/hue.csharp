using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class BridgeQuery : IBridgeQuery
    {
        class upnpResponse
        {
            public string id { get; set; }
            public string internalipaddress { get; set; }
        }

        public async Task<IPAddress> GetBridgeAddress()
        {
            IPAddress ipAddress = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://www.meethue.com");

                HttpResponseMessage response = await client.GetAsync("api/nupnp");

                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(contentString))
                    {
                        upnpResponse[] upnpResponse = JsonConvert.DeserializeObject<upnpResponse[]>(contentString);
                        ipAddress = IPAddress.Parse(upnpResponse[0].internalipaddress);
                    }
                }

            }

            return ipAddress;
        }

    }
}
