using System;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColourSwitch
    {
        Task SetColourAllAsync(NamedColour namedColour, UInt16 transitionTimeIn100MS = 1);
        Task SetColourGroupAsync(NamedColour namedColour, UInt16 transitionTimeIn100MS = 1);
        Task SetColourLightAsync(NamedColour namedColour, UInt16 transitionTimeIn100MS = 1);
    }

}
