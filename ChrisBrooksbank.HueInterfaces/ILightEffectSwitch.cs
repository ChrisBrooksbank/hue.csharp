using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightEffect
    {
    }

    public interface ILightEffectSwitch
    {
        void SetEffectAll(ILightEffect effect);
        void SetEffectGroup(string groupName, ILightEffect effect);
        void SetEffectLight(string lightName, ILightEffect effect);
    }


}
