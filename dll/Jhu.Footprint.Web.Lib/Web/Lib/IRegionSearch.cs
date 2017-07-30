using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Lib
{
    public interface IRegionSearch
    {
        string Owner { get; set; }
        string Name { get; set; }
        SearchMethod SearchMethod { get; set; }
        Spherical.Cartesian Point { get; set; }
        Spherical.Region Region { get; set; }
    }
}
