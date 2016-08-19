using System;
using System.Net;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public class ELinkButtonNotPressed : ApplicationException
    {
    }

    public interface IBridgeCommand
    {
        Task<bool> Ping();
        Task<string> CreateNewUser();
    }
}
