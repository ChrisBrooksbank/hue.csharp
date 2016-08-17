using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightCommand
    {
        Task<bool> RenameLight(IPAddress bridgeAddress, string userName, string oldLightName, string newLightName);
        Task<bool> DeleteLight(IPAddress bridgeAddress, string userName, string lightName);
        List<Light> FindNewLights(IPAddress bridgeAddress, string userName);
    }
}
