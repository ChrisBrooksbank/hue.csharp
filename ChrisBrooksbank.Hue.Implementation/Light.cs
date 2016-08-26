using ChrisBrooksbank.Hue.Interfaces;
using System;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class Light : ILight
    {
        LightState state;

        public Light()
        {
            state = new LightState();
        }


        public string ID { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelID { get; set; }
        public string Name { get; set; }

        public ILightState State
        {
            get
            {
                return state;
            }

            set
            {
                state = (LightState)value;
            }
        }

        public string SWVersion { get; set; }
        public string Type { get; set; }
        public string Uniqueid { get; set; }
    }
}