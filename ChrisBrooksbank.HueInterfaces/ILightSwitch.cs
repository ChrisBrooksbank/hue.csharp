using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightSwitch
    {
        Task TurnOnAllAsync();
        Task TurnOnGroupAsync(string groupName);
        Task TurnOnLightAsync(string lightName);
        Task TurnOffAllAsync();
        Task TurnOffGroupAsync(string groupName);
        Task TurnOffLightAsync(string lightName);
    }
}
