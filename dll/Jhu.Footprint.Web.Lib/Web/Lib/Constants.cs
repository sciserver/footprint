﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jhu.Footprint.Web.Lib
{
    public static class Constants
    {
        public static readonly HashSet<string> RestictedNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "plot",
            "convexhull",
            "footprint",
            "region",
            "points",
            "outline",
            "reduced",
            "editor"
        };

        public const string RoleAdmin = "footprint-admin";
        public const string RoleReader = "footprint-reader";
        public const string RoleWriter = "footprint-writer";

        public const string NamePattern = @"^[a-zA-Z0-9_\-\.\+]{3,}$";
        public static readonly Regex NamePatternRegex = new Regex(NamePattern, RegexOptions.Compiled);

        public const int ImageThumbnailWidth = 120;
        public const int ImageThumbnailHeight = 60;

        public const int ImagePreviewWidth = 320;
        public const int ImagePreviewHeight = 240;
        public const double ImagePreviewAreaLimit = 5000;

        /*
         *   North galactic pole and zeropoint of l are from : astropy-1.0.6-np110py34_0 package
         *   ""
         #   These are *not* from Reid & Brunthaler 2004 - instead, they were
         #   derived by doing:
         #  
         #    >>> FK4NoETerms(ra=192.25*u.degree, dec=27.4*u.degree).transform_to(FK5) 
         #  
         #  This gives better consistency with other codes than using the values
         #  from Reid & Brunthaler 2004 and the best self-consistency between FK5
         #  -> Galactic and FK5 -> FK4 -> Galactic. The lon0 angle was found by
         #  optimizing the self-consistency.
         *  """
         */

        // TODO: move to astrolib
        public const double raGP = 192.8594812065348 * Math.PI / 180.0;     // right ascenstion of the north Galactic pole (J2000) 
        public const double decGP = 27.12825118085622 * Math.PI / 180.0;    // declination of the north Galactic pole (J2000)
        public const double lCP = 122.9319185680026 * Math.PI / 180.0;      // longitude of the north celestial pole 

    }
}
