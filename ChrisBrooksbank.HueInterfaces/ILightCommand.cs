using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightCommand
    {
        Task<bool> RenameLight(IHueDotNetConfigurationReader hueDotNetconfigurationReader, string oldLightName, string newLightName);
        Task<bool> DeleteLight(IHueDotNetConfigurationReader hueDotNetconfigurationReader, string lightName);
        List<Light> FindNewLights(IHueDotNetConfigurationReader hueDotNetconfigurationReader);
    }
}
