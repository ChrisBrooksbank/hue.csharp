using System;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColourSwitch
    {
        Task SetColourAllAsync(NamedColour namedColour);
        Task SetColourLightAsync(NamedColour namedColour, string lightName);
    }

}
