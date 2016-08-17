using System;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{ 
    public class LightStateChangeCommand
    {
        public LightState NewState { get; set; }
        public UInt16 TransitionTimeIn100MS { get; set; }
        public int bri_inc { get; set; }
        public int sat_inc { get; set; }
        public int hue_inc { get; set; }
        public int ct_inc { get; set; }
        public int xy_inc { get; set; }
    }

    public interface ILightStateCommand
    {
        Task<bool> TurnAllOn(IPAddress bridgeAddress, string userName);
        Task<bool> TurnAllOff(IPAddress bridgeAddress, string userName);
        Task<bool> TurnOn(IPAddress bridgeAddress, string userName, string lightName);
        Task<bool> TurnOff(IPAddress bridgeAddress, string userName, string lightName);
        Task<bool> SetState(IPAddress bridgeAddress, string userName, string lightName, LightStateChangeCommand stateChangeCommand);
    }
}
