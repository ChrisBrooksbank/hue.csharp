using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{

    // TODO , this class needs splitting up in a sensible way
    public class Bridge : IBridgeQuery, IBridgeCommand, ILightQuery, ILightStateCommand, ILightCommand
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

        public async Task<bool> Ping(IPAddress bridgeAddress)
        {
            bool bridgeChecked = false;

            string signature = "<modelDescription>Philips hue Personal Wireless Lighting</modelDescription>";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + bridgeAddress);

                HttpResponseMessage response = await client.GetAsync("description.xml");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    bridgeChecked = responseString.Contains(signature);
                }

            }


            return bridgeChecked;
        }

        public async Task<string> CreateNewUser(IPAddress bridgeAddress)
        {
            // http://www.developers.meethue.com/documentation/getting-started
            string newUser = string.Empty;

            using (var client = new HttpClient())
            {
                Uri RequestUri = new Uri("http://" + bridgeAddress + "/api");
                StringContent requestContent = new StringContent("{\"devicetype\":\"my_hue_app#iphone peter\"}");
                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = RequestUri, Content = requestContent };
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    // responseString = "[{\"success\":{\"username\":\"mEifRt9dRMIlKu4c1qNrl6GZ1ky92NAtwUyTdxpU\"}}]";

                    if (responseString.Contains("link button not pressed"))
                    {
                        throw new ELinkButtonNotPressed();
                    }

                    newUser = responseString;
                }
            }

            return newUser;
        }


        public async Task<Light> GetLight(IPAddress bridgeAddress, string userName, string lightName)
        {
            Light light = new Light();
            Dictionary<string, Light> lights = await this.GetLights(bridgeAddress, userName);

            foreach (Light candidateLight in lights.Values)
            {
                if (candidateLight.name.Equals(lightName, StringComparison.CurrentCultureIgnoreCase))
                {
                    light = candidateLight;
                    break;
                }
            }

            return light;
        }

        public async Task<Dictionary<string, Light>> GetLights(IPAddress bridgeAddress, string userName)
        {
            Dictionary<string, Light> lights = new Dictionary<string, Light>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + bridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + userName + "/lights");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    lights = JsonConvert.DeserializeObject<Dictionary<string, Light>>(responseString);

                    // assign id property to each light
                    foreach (var keyvalue in lights)
                    {
                        ((Light)(keyvalue.Value)).id = keyvalue.Key;
                    }
                }

            }


            return lights;
        }

        public async Task<bool> TurnOn(IPAddress bridgeAddress, string userName, string lightName)
        {
            bool isOn = false;

            Light light = await this.GetLight(bridgeAddress, userName, lightName);

            if (light != null)
            {
                if (light.state.on)
                {
                    return true;
                }

                light.state.on = true;


                using (var client = new HttpClient())
                {
                    Uri RequestUri = new Uri("http://" + bridgeAddress + "/api/" + userName + "/lights/" + light.id + "/state");

                    StringContent requestContent = new StringContent("{\"on\" : true}");

                    HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        light = await this.GetLight(bridgeAddress, userName, lightName);
                        if (light != null)
                        {
                            isOn = light.state.on;
                        }
                    }
                }


            }

            return isOn;
        }

        public async Task<bool> TurnOff(IPAddress bridgeAddress, string userName, string lightName)
        {
            bool isOff = false;

            Light light = await this.GetLight(bridgeAddress, userName, lightName);

            if (light != null)
            {
                if (!light.state.on)
                {
                    return true;
                }

                using (var client = new HttpClient())
                {
                    Uri RequestUri = new Uri("http://" + bridgeAddress + "/api/" + userName + "/lights/" + light.id + "/state");

                    StringContent requestContent = new StringContent("{\"on\" : false}");

                    HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        light = await this.GetLight(bridgeAddress, userName, lightName);
                        if (light != null)
                        {
                            isOff = !light.state.on;
                        }
                    }
                }
            }

            return isOff;
        }

        public Task<bool> SetState(IPAddress bridgeAddress, string userName, string lightName, LightStateChangeCommand stateChangeCommand)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLight(IPAddress bridgeAddress, string userName, string lightName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TurnAllOn(IPAddress bridgeAddress, string userName)
        {
            using (var client = new HttpClient())
            {
                Uri RequestUri = new Uri("http://" + bridgeAddress + "/api/" + userName + "/groups/0/action");

                StringContent requestContent = new StringContent("{\"on\" : true}");

                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return true;
        }

        public async Task<bool> TurnAllOff(IPAddress bridgeAddress, string userName)
        {
            using (var client = new HttpClient())
            {
                Uri RequestUri = new Uri("http://" + bridgeAddress + "/api/" + userName + "/groups/0/action");

                StringContent requestContent = new StringContent("{\"on\" : false}");

                HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return true;
        }

        public Task<bool> RenameLight(IPAddress bridgeAddress, string userName, string oldLightName, string newLightName)
        {
            throw new NotImplementedException();
        }

        public List<Light> FindNewLights(IPAddress bridgeAddress, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
