using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{

    public class LightQuery: ILightQuery
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public LightQuery(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public async Task<ILight> GetLightAsync(string lightName)
        {
            Light light = new Light();
            Dictionary<string, ILight> lights = await this.GetLightsAsync();

            foreach (Light candidateLight in lights.Values)
            {
                if (candidateLight.Name.Equals(lightName, StringComparison.CurrentCultureIgnoreCase))
                {
                    light = candidateLight;
                    break;
                }
            }

            return light;
        }

        public async Task<Dictionary<string, ILight>> GetLightsAsync()
        {
            Dictionary<string, ILight> lights = new Dictionary<string, ILight>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/lights");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();


                    Dictionary<string, Light> jsonLights = new Dictionary<string, Light>();
                    jsonLights = JsonConvert.DeserializeObject<Dictionary<string, Light>>(responseString);
                    foreach(var light in jsonLights)
                    {
                        lights.Add(light.Key, light.Value);
                    }

                    // assign id property to each light
                    foreach (var keyvalue in lights)
                    {
                        ((ILight)(keyvalue.Value)).ID = keyvalue.Key;
                    }
                }

            }


            return lights;
        }
    }
}
