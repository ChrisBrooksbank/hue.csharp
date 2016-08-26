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

        public BridgeTests()
        {
            hueDotNetconfigurationReader = new Implementation.HueDotNetConfigurationReader();

            bridgeQuery = new Implementation.BridgeQuery();
            bridgeCommand = new Implementation.BridgeCommand(hueDotNetconfigurationReader);

            lightQuery = new Implementation.LightQuery(hueDotNetconfigurationReader);
            lightSwitch = new Implementation.LightSwitch(hueDotNetconfigurationReader, lightQuery);
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

    }
   
}
