using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Lib
{
    public enum FootprintSearchMethod
    {
        Name,
        Object,
        Point,
        Intersect
    }

    [Flags]
    public enum SearchSource : int
    {
        None = 0,
        Public = 0x01,         // all public owned by someone
        My = 0x02,          // all in myfootprint, required login
        //All = 0x04,      // all public owned by noone // Do we really need this?
    }

    [Flags]
    public enum FolderType : byte
    {
        None = 0,
        Union = 1,
        Intersection = 2,

        Any = 0x7F,
    }

    public enum FootprintType
    {
        Region = 0,
        Footprint = 1
    }
}
