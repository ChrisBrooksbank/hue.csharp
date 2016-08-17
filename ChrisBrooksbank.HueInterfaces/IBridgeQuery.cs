using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface IBridgeQuery
    {
        Task<IPAddress> GetBridgeAddress();
    }
}
