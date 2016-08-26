using ChrisBrooksbank.Hue.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
   
    public interface ILightQuery
    {
        Task<ILight> GetLightAsync(string lightName);
        Task<Dictionary<string, ILight>> GetLightsAsync();
    }

}
