using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface ILightFlashSwitch
    {
        Task FlashAllOnceAsync();
        Task FlashAllSeveralTimesASync();
    }
}
