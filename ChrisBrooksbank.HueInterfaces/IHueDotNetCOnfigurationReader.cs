using System.Net;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface IHueDotNetConfigurationReader
    {
        IPAddress BridgeAddress { get; }
        string UserName { get; }
    }
}
