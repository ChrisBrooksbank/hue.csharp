using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            Group group = new Group();
            Dictionary<string, IGroup> groups = await this.GetGroupsAsync();

            foreach (Group candidateGroup in groups.Values)
            {
                if (candidateGroup.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase))
                {
                    group = candidateGroup;
                    break;
                }
            }

            return group;
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
