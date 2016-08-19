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

        public async Task<Light> GetLight(string lightName)
        {
            Light light = new Light();
            Dictionary<string, Light> lights = await this.GetLights();

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

        public async Task<Dictionary<string, Light>> GetLights()
        {
            Dictionary<string, Light> lights = new Dictionary<string, Light>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/lights");

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
    }
}
