using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class GroupQuery : IGroupQuery
    {
        IHueDotNetConfigurationReader HueConfiguration;

        public GroupQuery(IHueDotNetConfigurationReader hueConfiguration)
        {
            HueConfiguration = hueConfiguration;
        }

        public async Task<IGroup> GetGroupAsync(string groupName)
        {
            string groupID = await this.GetGroupIDAsync(groupName);

            IGroup group = new Group();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/groups/" + groupID);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();


                    group = JsonConvert.DeserializeObject<Group>(responseString);
                    group.ID = groupID;
                }
            }

            return group;
        }

        public async Task<string> GetGroupIDAsync(string groupName)
        {
            ObjectCache cache = MemoryCache.Default;
            Dictionary<string, string> groupCache = cache["GroupCache"] as Dictionary<string, string>;

            if (groupCache == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(HueConfiguration.LightCacheExpiryMinutes));

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);
                    HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/groups");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        Dictionary<string, Group> jsonGroups = new Dictionary<string, Group>();
                        jsonGroups = JsonConvert.DeserializeObject<Dictionary<string, Group>>(responseString);

                        groupCache = new Dictionary<string, string>();
                        foreach (var group in jsonGroups)
                        {
                            groupCache.Add(group.Value.Name, group.Key);
                        }

                        cache.Set("GroupCache", groupCache, policy);
                    }

                }

            }

            string groupID = groupCache[groupName];
            return groupID;
        }

        public async Task<Dictionary<string, IGroup>> GetGroupsAsync()
        {
            Dictionary<string, IGroup> groups = new Dictionary<string, IGroup>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://" + HueConfiguration.BridgeAddress);

                HttpResponseMessage response = await client.GetAsync("/api/" + HueConfiguration.UserName + "/groups");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();


                    Dictionary<string, Group> jsonGroups = new Dictionary<string, Group>();
                    jsonGroups = JsonConvert.DeserializeObject<Dictionary<string, Group>>(responseString);
                    foreach (var group in jsonGroups)
                    {
                        groups.Add(group.Key, group.Value);
                    }

                    // assign id property to each group
                    foreach (var keyvalue in groups)
                    {
                        ((IGroup)(keyvalue.Value)).ID = keyvalue.Key;
                    }
                }
            }

            return groups;
        }
    }
}
