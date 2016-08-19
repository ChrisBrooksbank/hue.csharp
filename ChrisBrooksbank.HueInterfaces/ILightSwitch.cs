using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightSwitch
    {
        void TurnOnAll();
        void TurnOnGroup(string groupName);
        void TurnOnLight(string lightName);
        void TurnOffAll();
        void TurnOffGroup(string groupName);
        void TurnOffLIght(string lightName);
    }
}
