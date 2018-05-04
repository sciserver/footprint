using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Api.V1
{
    public enum CoordinateSystem
    {
        Eq = 1,
        EqJ2000 = 1,
        Gal = 2,
        GalJ2000 = 2,
    }

    public enum CoordinateRepresentation
    {
        Dec,
        Sexa,
        Cart
    }

    public enum OutlineReduction
    {
        Dp,
        CHull
    }

    public enum Projection
    {
        Aitoff,
        Hammer,
        Mollweide,
        Stereo,
        Ortho
    }



    public enum DegreeStyle
    {
        Dec,
        Sexa
    }

    public enum ColorTheme
    {
        Light,
        Dark
    }
}
