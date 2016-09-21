using System;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColourSwitch
    {
        Task SetColourAllAsync(NamedColour namedColour);
        Task SetColourGroupAsync(NamedColour namedColour, string groupName);
        Task SetColourLightAsync(NamedColour namedColour, string lightName);
    }

}
