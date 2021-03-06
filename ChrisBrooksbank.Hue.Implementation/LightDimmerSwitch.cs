﻿using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightDimmerSwitch : ILightDimmerSwitch
    {

        IHueDotNetConfigurationReader HueConfiguration;
        ILightQuery LightQuery;

        public LightDimmerSwitch(IHueDotNetConfigurationReader hueConfiguration, ILightQuery lightQuery)
        {
            HueConfiguration = hueConfiguration;
            LightQuery = lightQuery;
        }

        public async Task SetMaxBrightnessAllASync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"bri\" : 254}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }

        public async Task SetMinBrightnessAllAsync()
        {
            Uri RequestUri = new Uri("http://" + HueConfiguration.BridgeAddress + "/api/" + HueConfiguration.UserName + "/groups/0/action");
            StringContent requestContent = new StringContent("{\"bri\" : 1}");

            HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Put, RequestUri = RequestUri, Content = requestContent };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
            }

            return;
        }
    }
}
