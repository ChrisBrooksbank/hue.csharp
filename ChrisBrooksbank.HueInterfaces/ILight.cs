using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILight
    {
        string ID { get; set; }
        ILightState State { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        string ModelID { get; set; }
        string ManufacturerName { get; set; }
        string Uniqueid { get; set; }
        string SWVersion { get; set; }
    }
}
