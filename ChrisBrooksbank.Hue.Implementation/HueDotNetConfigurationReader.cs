using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class HueDotNetConfigurationReader : IHueDotNetConfigurationReader
    {

        private class HueDontNetConfiguration
        {
            public string BridgeAddress { get; set; }
            public string UserName { get; set; }
            public string ApplicationName { get; set; }
            public string LightCacheKeyName { get; set; }
            public int LightCacheExpiryMinutes { get; set; }
        }

        HueDontNetConfiguration config;

        public HueDotNetConfigurationReader()
        {
            var filestream = new System.IO.FileStream("HueDotNetConfiguration.json",
                                       System.IO.FileMode.Open,
                                       System.IO.FileAccess.Read,
                                       System.IO.FileShare.ReadWrite);

            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

            string json = file.ReadToEnd();

            config = JsonConvert.DeserializeObject<HueDontNetConfiguration>(json);

        }

        public IPAddress BridgeAddress
        {
            get
            {

                return IPAddress.Parse(config.BridgeAddress);
            }
        }

        public string UserName
        {
            get
            {
                return config.UserName;
            }
        }

        public string ApplicationName
        {
            get
            {
                return config.ApplicationName;
            }
        }

        public int LightCacheExpiryMinutes
        {
            get
            {
                return config.LightCacheExpiryMinutes;
            }
        }
    }
}
