using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hue.Csharp.BridgeTests
{
    [TestClass]
    public class BridgeTests
    {
        [TestMethod]
        public async Task GetBridgeAddress()
        {
            IBridgeConnectivity bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();

            Assert.IsNotNull(bridgeAddress);
        }

        [TestMethod]
        public async Task BridgeFoundAtAddress()
        {
            IBridgeConnectivity bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            bool bridgeFound = await bridge.BridgeFoundAtAddress(bridgeAddress);

            Assert.IsTrue(bridgeFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ELinkButtonNotPressed))]
        public async Task CreateNewUserFailsWithoutPressingLinkButton()
        {
            IBridgeConnectivity bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            string newUser = await bridge.CreateNewUser(bridgeAddress);
        }

        [TestMethod]
        public async Task GetLandingLight()
        {
            Bridge bridge = new Bridge();
           
            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            string userName = "hS582W-AhSdUEE7Tfjll2xslcgFOTOEglDTOZTpA";

            Light light = await bridge.GetLight(bridgeAddress, userName, "landing");

            Assert.IsTrue(light != null);
        }

        [TestMethod]
        public async Task AtLeastOneLight()
        {
            Bridge bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            string userName = "hS582W-AhSdUEE7Tfjll2xslcgFOTOEglDTOZTpA";

            Dictionary<string, Light> lights = await bridge.GetLights(bridgeAddress, userName);

            Assert.IsTrue(lights.Count > 0);
        }

        [TestMethod]
        public async Task TurnLandingLightOn()
        {
            Bridge bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            string userName = "hS582W-AhSdUEE7Tfjll2xslcgFOTOEglDTOZTpA";

            bool turnedOn = await bridge.TurnOn(bridgeAddress, userName, "landing");
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnLandingLightOff()
        {
            Bridge bridge = new Bridge();

            IPAddress bridgeAddress = await bridge.GetBridgeAddress();
            string userName = "hS582W-AhSdUEE7Tfjll2xslcgFOTOEglDTOZTpA";

            bool turnedOff = await bridge.TurnOff(bridgeAddress, userName, "landing");
            Assert.IsTrue(turnedOff);
        }



    }


    public class ELinkButtonNotPressed : ApplicationException
    {

    }

 
    internal class Bridge : IBridgeConnectivity, ILightsReader, ILightsWriter
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

        public async Task<bool> BridgeFoundAtAddress(IPAddress bridgeAddress)
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

                if ( response.IsSuccessStatusCode)
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


        public async Task<Light>  GetLight(IPAddress bridgeAddress, string userName, string lightName)
        {
            Light light = new Light();
            Dictionary<string, Light> lights = await this.GetLights(bridgeAddress, userName);

            foreach (Light candidateLight in lights.Values)
            {
                if ( candidateLight.name.Equals(lightName, StringComparison.CurrentCultureIgnoreCase))
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

                    lights = JsonConvert.DeserializeObject<Dictionary<string,Light>>(responseString);

                    // assign id property to each light
                    foreach(var keyvalue in lights)
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

            Light light = await this.GetLight( bridgeAddress, userName, lightName);

            if ( light != null )
            {
                if ( light.state.on )
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

    }


    public class LightState
    {
        public bool on { get; set; }
        public byte bri { get; set; }
        public int hue { get; set; }
        public byte sat { get; set; }
        public string effect { get; set; }
        public float[] xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }
    }


    public class Light {
        public string id { get; set; }
        public LightState state { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string manufacturername { get; set; }
        public string uniqueid { get; set; }
        public string swversion { get; set; }
    }

    internal interface ILightsReader
    {
        Task<Light> GetLight(IPAddress bridgeAddress, string userName, string lightName);
        Task<Dictionary<string, Light>> GetLights(IPAddress bridgeAddress, string userName);
    }

    internal interface ILightsWriter
    {
        Task<bool> TurnOn(IPAddress bridgeAddress, string userName, string lightName);
        Task<bool> TurnOff(IPAddress bridgeAddress, string userName, string lightName);
    }


    internal interface IBridgeConnectivity
    {
        Task<IPAddress> GetBridgeAddress();
        Task<bool> BridgeFoundAtAddress(IPAddress bridgeAddress);
        Task<string> CreateNewUser(IPAddress bridgeAddress);
    }
}
