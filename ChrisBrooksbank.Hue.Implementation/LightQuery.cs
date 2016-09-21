using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Caching;
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
            string lightID = await this.GetLightIDAsync(lightName);
            ILight light = new Light();

            Dictionary<string, ILight> lights = new Dictionary<string, ILight>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/lights/" + lightID);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    light = JsonConvert.DeserializeObject<Light>(responseString);
                    light.ID = lightID;
                }
            }

            return light;
        }

        public async Task<Dictionary<string, ILightDescription>> GetLightDescriptionsAsync()
        {
            ObjectCache cache = MemoryCache.Default;
            Dictionary<string, ILightDescription> lightDescriptionCache = cache["LightDescriptionCache"] as Dictionary<string, ILightDescription>;

            if (lightDescriptionCache == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(HueConfiguration.LightCacheExpiryMinutes));

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);
                    HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/lights");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        Dictionary<string, Light> jsonLights = new Dictionary<string, Light>();
                        jsonLights = JsonConvert.DeserializeObject<Dictionary<string, Light>>(responseString);

                        lightDescriptionCache = new Dictionary<string, ILightDescription>();

                        foreach (var light in jsonLights)
                        {
                            ILightDescription lightDescription = new LightDescription
                            {
                                ModelID = light.Value.ModelID,
                                ManufacturerName = light.Value.ManufacturerName,
                                Name = light.Value.Name,
                                Type = light.Value.Type,
                                SWVersion = light.Value.SWVersion,
                                Uniqueid = light.Value.Uniqueid
                            };

                            lightDescriptionCache[light.Value.Name] = lightDescription;
                        }

                        cache.Set("LightDescriptionCache", lightDescriptionCache, policy);
                    }

                }
            }

            return lightDescriptionCache;
        }

        public async Task<string> GetLightIDAsync(string lightName)
        {
            ObjectCache cache = MemoryCache.Default;
            Dictionary<string, string> lightCache = cache["LightIDCache"] as Dictionary<string, string>;

            if (lightCache == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset( DateTime.UtcNow.AddMinutes(HueConfiguration.LightCacheExpiryMinutes) );

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);
                    HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/lights");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        Dictionary<string, Light> jsonLights = new Dictionary<string, Light>();
                        jsonLights = JsonConvert.DeserializeObject<Dictionary<string, Light>>(responseString);

                        lightCache = new Dictionary<string, string>();

                        foreach (var light in jsonLights)
                        {
                            lightCache[light.Value.Name] = light.Key;
                        }

                        cache.Set("LightIDCache", lightCache, policy);
                    }

                }
                    
            }

            string lightID = lightCache[lightName];
            return lightID;
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
