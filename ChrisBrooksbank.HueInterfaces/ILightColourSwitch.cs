using System;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColourSwitch
    {
        Task SetColourAllAsync(NamedColour namedColour);
        Task SetColourGroupAsync(NamedColour namedColour);
        Task SetColourLightAsync(NamedColour namedColour);
    }

}
