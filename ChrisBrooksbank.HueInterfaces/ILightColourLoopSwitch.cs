﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightColourLoopSwitch
    {
        Task TurnOnAllASync();
        Task TurnOffAllASync();
    }
}
