using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightState
    {
        bool On { get; set; }
        byte Bri { get; set; }
        int Hue { get; set; }
        byte Sat { get; set; }
        string Effect { get; set; }
        float[] XY { get; set; }
        int CT { get; set; }
        string Alert { get; set; }
        string ColorMode { get; set; }
        bool Reachable { get; set; }
    }
}
