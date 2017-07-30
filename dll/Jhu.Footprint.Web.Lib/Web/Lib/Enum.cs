﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        None = 0,
        Union = 1,
        Intersection = 2,
        Intersect = 2,
        Subtract = 4,

        Any = 0x7F,
    }

    public enum RegionType : byte
    {
        Single = 0,
        Combined = 1
    }
}
