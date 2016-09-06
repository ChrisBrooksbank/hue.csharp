using ChrisBrooksbank.Hue.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.BridgeTests
{
    [TestClass]
    public class BridgeTests {

        private readonly string testLightName = "landing";

        IHueDotNetConfigurationReader hueDotNetconfigurationReader;

        IBridgeQuery bridgeQuery;
        IBridgeCommand bridgeCommand;

        ILightQuery lightQuery;

        ILightSwitch lightSwitch;

        IGroupQuery groupQuery;

        public BridgeTests()
        {
            hueDotNetconfigurationReader = new Implementation.HueDotNetConfigurationReader();

            bridgeQuery = new Implementation.BridgeQuery();
            bridgeCommand = new Implementation.BridgeCommand(hueDotNetconfigurationReader);

            lightQuery = new Implementation.LightQuery(hueDotNetconfigurationReader);
            lightSwitch = new Implementation.LightSwitch(hueDotNetconfigurationReader, lightQuery);

            groupQuery = new Implementation.GroupQuery(hueDotNetconfigurationReader);
        }

        private async Task<IPAddress> GetTestsBridgeAddress()
        {
            return await bridgeQuery.GetBridgeAddress();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void CanReadHueDotNetConfigurationBridgeAddress()
        {
            Assert.IsTrue(hueDotNetconfigurationReader != null && hueDotNetconfigurationReader.BridgeAddress != null);
        }

        [TestMethod]
        public void CanReadHueDotNetConfigurationUserName()
        {
            Assert.IsTrue(hueDotNetconfigurationReader != null && !string.IsNullOrEmpty(hueDotNetconfigurationReader.UserName));
        }

        [TestMethod]
        public async Task BridgeFoundAtAddress()
        {
            bool bridgeFound = await bridgeCommand.Ping();

            Assert.IsTrue(bridgeFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ELinkButtonNotPressed))]
        public async Task CreateNewUserFailsWithoutPressingLinkButton()
        {
            string newUser = await bridgeCommand.CreateNewUser();
        }

        [TestMethod]
        public async Task GetLight()
        {
            ILight light = await lightQuery.GetLightAsync(testLightName);

            Assert.IsTrue(light != null);
        }

        [TestMethod]
        public async Task AtLeastOneLight()
        {
            Dictionary<string, ILight> lights = await lightQuery.GetLightsAsync();
            Assert.IsTrue(lights.Count > 0);
        }


        [TestMethod]
        public async Task CanTurnLightOn()
        {
            await lightSwitch.TurnOnLightAsync("landing");
        }


        [TestMethod]
        public async Task CanTurnLightOff()
        {
            await lightSwitch.TurnOffLightAsync("landing");
        }

        [TestMethod]
        public async Task GetGroupsAsync()
        {
            Dictionary<string, IGroup> groups = await groupQuery.GetGroupsAsync();
            Assert.IsTrue(groups.Count > 0);
        }

        [TestMethod]
        public async Task GetGroupAsync()
        {
            IGroup group = await groupQuery.GetGroupAsync("living");
            Assert.IsTrue(group != null && !string.IsNullOrEmpty(group.ID));
        }

        [TestMethod]
        public async Task GetLightIDAsync()
        {
            string lightID = await lightQuery.GetLightIDAsync("landing");
            Assert.IsTrue(lightID.Equals("8"));
        }

        [TestMethod]
        public async Task GetLightIDAsyncAgain()
        {
            string lightID = await lightQuery.GetLightIDAsync("landing");
            Assert.IsTrue(lightID.Equals("8"));
        }

    }
   
}
