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

        IBridgeQuery bridgeQuery;
        IBridgeCommand bridgeCommand;
        ILightQuery lightQuery;
        ILightCommand lightCommand;
        ILightStateCommand lightStateCommand;
        IHueDotNetConfigurationReader hueDotNetconfigurationReader;

        public BridgeTests()
        {
            ChrisBrooksbank.Hue.Implementation.Bridge bridge = new ChrisBrooksbank.Hue.Implementation.Bridge();

            hueDotNetconfigurationReader = bridge;
            bridgeQuery = bridge;
            bridgeCommand = bridge;
            lightQuery = bridge;
            lightCommand = bridge;
            lightStateCommand = bridge;
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
            bool bridgeFound = await bridgeCommand.Ping(hueDotNetconfigurationReader.BridgeAddress);

            Assert.IsTrue(bridgeFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ELinkButtonNotPressed))]
        public async Task CreateNewUserFailsWithoutPressingLinkButton()
        {
            string newUser = await bridgeCommand.CreateNewUser(hueDotNetconfigurationReader.BridgeAddress);
        }

        [TestMethod]
        public async Task GetLight()
        {
            Light light = await lightQuery.GetLight(hueDotNetconfigurationReader, testLightName);

            Assert.IsTrue(light != null);
        }

        [TestMethod]
        public async Task AtLeastOneLight()
        {
            Dictionary<string, Light> lights = await lightQuery.GetLights(hueDotNetconfigurationReader);
            Assert.IsTrue(lights.Count > 0);
        }

        [TestMethod]
        public async Task TurnAllLightsOn()
        {
            bool turnedOn = await lightStateCommand.TurnAllOn(hueDotNetconfigurationReader);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnAllLightsOff()
        {
            bool turnedOn = await lightStateCommand.TurnAllOff(hueDotNetconfigurationReader);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnLightOn()
        {
            bool turnedOn = await lightStateCommand.TurnOn(hueDotNetconfigurationReader, testLightName);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnLightOff()
        {
            bool turnedOff = await lightStateCommand.TurnOff(hueDotNetconfigurationReader, testLightName);
            Assert.IsTrue(turnedOff);
        }


    }
   
}
