using ChrisBrooksbank.Hue.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
        IColourQuery colourQuery;
        ILightColourSwitch lightColourSwitch;

        IGroupQuery groupQuery;

        public BridgeTests()
        {
            hueDotNetconfigurationReader = new Implementation.HueDotNetConfigurationReader();

            bridgeQuery = new Implementation.BridgeQuery();
            bridgeCommand = new Implementation.BridgeCommand(hueDotNetconfigurationReader);

            lightQuery = new Implementation.LightQuery(hueDotNetconfigurationReader);
            lightSwitch = new Implementation.LightSwitch(hueDotNetconfigurationReader, lightQuery);
            colourQuery = new Implementation.ColourQuery();
            lightColourSwitch = new Implementation.LightColourSwitch(hueDotNetconfigurationReader, lightQuery, colourQuery);

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
        public void ReadHueDotNetConfigurationBridgeAddress()
        {
            Assert.IsTrue(hueDotNetconfigurationReader != null && hueDotNetconfigurationReader.BridgeAddress != null);
        }

        [TestMethod]
        public void ReadHueDotNetConfigurationUserName()
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
        public async Task TurnLightOn()
        {
            await lightSwitch.TurnOnLightAsync("landing");
        }


        [TestMethod]
        public async Task TurnLightOff()
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
            IGroup group = await groupQuery.GetGroupAsync("Living");
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


        [TestMethod]
        public async Task GetGroupIDAsync()
        {
            string groupID = await groupQuery.GetGroupIDAsync("Living");
            Assert.IsTrue(groupID.Equals("1"));
        }

        [TestMethod]
        public async Task GetGroupIDAsyncAgain()
        {
            string groupID = await groupQuery.GetGroupIDAsync("Living");
            Assert.IsTrue(groupID.Equals("1"));
        }

        [TestMethod]
        public void AzureIsANamedColour()
        {
            IEnumerable<INamedColourDetail> namedColours = colourQuery.GetNamedColourDetails();

            bool foundAzure = false;
            foreach(INamedColourDetail namedColour in namedColours)
            {
                if (namedColour.Colour.Equals("Azure"))
                {
                    foundAzure = true;
                }
            }

            Assert.IsTrue(foundAzure);
        }

        [TestMethod]
        public void RedIsANamedColour()
        {
            IEnumerable<INamedColourDetail> namedColours = colourQuery.GetNamedColourDetails();

            bool foundAzure = false;
            foreach (INamedColourDetail namedColour in namedColours)
            {
                if (namedColour.Colour.Equals("Red"))
                {
                    foundAzure = true;
                }
            }

            Assert.IsTrue(foundAzure);
        }

        [TestMethod]
        public void GetNamedColoursAsCSV()
        {
            IEnumerable<INamedColourDetail> namedColours = colourQuery.GetNamedColourDetails();

            StringBuilder namedColoursCSVBuilder = new StringBuilder();
            foreach (INamedColourDetail namedColour in namedColours)
            {
                namedColoursCSVBuilder.Append(namedColour.Colour.Replace(" ", string.Empty));
                namedColoursCSVBuilder.Append(",");
            }

            string namedColoursCSV = namedColoursCSVBuilder.ToString();

            Assert.IsTrue(namedColoursCSV.Length > 100);
        }

        [TestMethod]
        public async Task AllLightsRed()
        {
            await lightColourSwitch.SetColourAllAsync( NamedColour.Red );
            // TODO check colour was set
        }

        [TestMethod]
        public async Task AllLightsDarkSeaGreen()
        {
            await lightColourSwitch.SetColourAllAsync(NamedColour.DarkSeaGreen);
            // TODO check colour was set
        }


    }
   
}
