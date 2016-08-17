using ChrisBrooksbank.Hue.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.BridgeTests
{
    [TestClass]
    public class BridgeTests {

        readonly IPAddress bridgeAddress;
        readonly string bridgeUserName;
        readonly string testLightName;

        IBridgeQuery bridgeQuery;
        IBridgeCommand bridgeCommand;
        ILightQuery lightQuery;
        ILightCommand lightCommand;
        ILightStateCommand lightStateCommand;

        public BridgeTests()
        {
            bridgeUserName = "hS582W-AhSdUEE7Tfjll2xslcgFOTOEglDTOZTpA";
            testLightName = "landing";

            ChrisBrooksbank.Hue.Implementation.Bridge bridge = new ChrisBrooksbank.Hue.Implementation.Bridge();

            bridgeQuery = bridge;
            bridgeCommand = bridge;
            lightQuery = bridge;
            lightCommand = bridge;
            lightStateCommand = bridge;

            bridgeAddress = this.GetTestsBridgeAddress().Result;
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
        public async Task BridgeFoundAtAddress()
        {
            bool bridgeFound = await bridgeCommand.Ping(bridgeAddress);

            Assert.IsTrue(bridgeFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ELinkButtonNotPressed))]
        public async Task CreateNewUserFailsWithoutPressingLinkButton()
        {
            string newUser = await bridgeCommand.CreateNewUser(bridgeAddress);
        }

        [TestMethod]
        public async Task GetLight()
        {
            Light light = await lightQuery.GetLight(bridgeAddress, bridgeUserName, testLightName);

            Assert.IsTrue(light != null);
        }

        [TestMethod]
        public async Task AtLeastOneLight()
        {
            Dictionary<string, Light> lights = await lightQuery.GetLights(bridgeAddress, bridgeUserName);
            Assert.IsTrue(lights.Count > 0);
        }

        [TestMethod]
        public async Task TurnAllLightsOn()
        {
            bool turnedOn = await lightStateCommand.TurnAllOn(bridgeAddress, bridgeUserName);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnAllLightsOff()
        {
            bool turnedOn = await lightStateCommand.TurnAllOff(bridgeAddress, bridgeUserName);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnLightOn()
        {
            bool turnedOn = await lightStateCommand.TurnOn(bridgeAddress, bridgeUserName, testLightName);
            Assert.IsTrue(turnedOn);
        }

        [TestMethod]
        public async Task TurnLightOff()
        {
            bool turnedOff = await lightStateCommand.TurnOff(bridgeAddress, bridgeUserName, testLightName);
            Assert.IsTrue(turnedOff);
        }


    }
   
}
