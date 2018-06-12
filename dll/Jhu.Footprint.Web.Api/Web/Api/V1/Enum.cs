using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Jhu.Footprint.Web.Api.V1
{
    public enum CoordinateSystem
    {
        [EnumMember(Value = "eq")]
        Eq = 1,

        [EnumMember(Value = "eqj2000")]
        EqJ2000 = 1,

        [EnumMember(Value = "gal")]
        Gal = 2,

        [EnumMember(Value = "galj2000")]
        GalJ2000 = 2,
    }

    public enum CoordinateRepresentation
    {
        [EnumMember(Value = "dec")]
        Dec,

        [EnumMember(Value = "sexa")]
        Sexa,

        [EnumMember(Value = "cart")]
        Cart
    }

    public enum OutlineReduction
    {
        [EnumMember(Value = "dp")]
        Dp,

        [EnumMember(Value = "chull")]
        CHull
    }

    public enum Projection
    {
        [EnumMember(Value = "aitoff")]
        Aitoff,

        [EnumMember(Value = "hammer")]
        Hammer,

        [EnumMember(Value = "mollweide")]
        Mollweide,

        [EnumMember(Value = "stereo")]
        Stereo,

        [EnumMember(Value = "ortho")]
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
