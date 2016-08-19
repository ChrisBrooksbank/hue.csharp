using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColour
    {

    }

    public interface ILightColourSwitch
    {
        void SetColourAll(ILightColour colour, UInt16 transitionTimeIn100MS = 1);
        void SetColourGroup(ILightColour colour, UInt16 transitionTimeIn100MS = 1);
        void SetColourLight(ILightColour colour, UInt16 transitionTimeIn100MS = 1);
    }
}
