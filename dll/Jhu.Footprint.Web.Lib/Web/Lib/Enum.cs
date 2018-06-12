using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    public enum SearchType
    {
        Footprint,
        Region
    }

    public enum SearchMethod
    {
        Name,       // footprint name
        Point,      // Coordinates
        Intersect,
        Contain,
        Cover
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
    public enum CombinationMethod : byte
    {
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "union")]
        Union = 1,

        [EnumMember(Value = "intersection")]
        Intersection = 2,

        [EnumMember(Value = "intersect")]
        Intersect = 2,

        [EnumMember(Value = "subtract")]
        Subtract = 4,

        [EnumMember(Value = "chull")]
        CHull = 5,

        [EnumMember(Value = "any")]
        Any = 0x7F,
    }

    public enum RegionType : byte
    {
        Single = 0,
        Combined = 1
    }
}
