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
    public enum FolderType
    {
        Any = 0,
        Unknown = 1,
        Union = 2,
        Intersection = 3,
        None = 4
    }

}
