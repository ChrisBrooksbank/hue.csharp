using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public interface IGroupQuery
    {
        Task<IGroup> GetGroupAsync(string lightName);
        Task<Dictionary<string, IGroup>> GetGroupsAsync();
    }
}
