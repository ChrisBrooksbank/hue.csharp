using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightState : ILightState
    {
        public string Alert { get; set; }
        public byte Bri { get; set; }
        public string ColorMode { get; set; }
        public int CT { get; set; }
        public string Effect { get; set; }
        public int Hue { get; set; }
        public bool On { get; set; }
        public bool Reachable { get; set; }
        public byte Sat { get; set; }
        public float[] XY { get; set; }
    }
}
