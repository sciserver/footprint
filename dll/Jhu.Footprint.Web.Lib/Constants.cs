using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    public static class Constants
    {
        public static readonly HashSet<string> RestictedNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {  
            "plot",
            "convexhull",
            "footprint",
            "points",
            "outline",
            "reduced"
        };
    }
}
