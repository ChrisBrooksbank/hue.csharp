using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightNamedColour
    {
        string Colour { get; set; }
        string RGB { get; set; }
        string GamutA { get; set; }
        string GamutB { get; set; }
        string GamutC { get; set; }
    }

    public interface ILightColourSwitch
    {
        IEnumerable<ILightNamedColour> GetNamedColours();

        Task SetColourAllAsync(string namedColour, UInt16 transitionTimeIn100MS = 1);
        Task SetColourGroupAsync(string namedColour, UInt16 transitionTimeIn100MS = 1);
        Task SetColourLightAsync(string namedColour, UInt16 transitionTimeIn100MS = 1);
    }
}
