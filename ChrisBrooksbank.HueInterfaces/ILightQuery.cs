using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public class Light
    {
        public string id { get; set; }
        public LightState state { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string manufacturername { get; set; }
        public string uniqueid { get; set; }
        public string swversion { get; set; }
    }


    public class LightState
    {
        public bool on { get; set; }
        public byte bri { get; set; }
        public int hue { get; set; }
        public byte sat { get; set; }
        public string effect { get; set; }
        public float[] xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }
    }

    public interface ILightQuery
    {
        Task<Light> GetLight(string lightName);
        Task<Dictionary<string, Light>> GetLights();
    }

}
