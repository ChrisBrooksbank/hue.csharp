using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightDimmerSwitch
    {
        Task SetMinBrightnessAllAsync();
        Task SetMaxBrightnessAllASync();
    }
}
