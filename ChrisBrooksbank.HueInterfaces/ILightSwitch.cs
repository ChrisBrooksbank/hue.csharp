﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightSwitch
    {
        Task TurnOnAllAsync();
        Task TurnOnLightAsync(string lightName);
        Task TurnOffAllAsync();
        Task TurnOffLightAsync(string lightName);
    }
}
