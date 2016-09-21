using System;
using ChrisBrooksbank.Hue.Interfaces;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class LightDescription : ILightDescription
    {
        public string ManufacturerName { get; set; }
        public string ModelID { get; set; }
        public string Name { get; set; }
        public string SWVersion { get; set; }
        public string Type { get; set; }
        public string Uniqueid { get; set; }
    }
}
