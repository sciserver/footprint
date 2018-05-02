using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using Jhu.Spherical;
using Jhu.SharpAstroLib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Represents a point on the sphere. Specify only one of (RA, Dec), (lon, lat) or (cx, cy, cz).")]
    public class Point
    {
        [DataMember(Name = "ra", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Right ascension. Specify either decimal degrees or HMS.")]
        public string RA { get; set; }

        [DataMember(Name = "dec", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Declination. Specify either decimal degrees or DMS.")]
        public string Dec { get; set; }

        [DataMember(Name = "lon", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Longitude in degrees.")]
        public double? Lon { get; set; }

        [DataMember(Name = "lat", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Latitude in degrees.")]
        public double? Lat { get; set; }

        [DataMember(Name = "cx", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Cartesian X coordinate of unit vector.")]
        public double? Cx { get; set; }

        [DataMember(Name = "cy", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Cartesian Y coordinate of unit vector.")]
        public double? Cy { get; set; }

        [DataMember(Name = "cz", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Cartesian Z coordinate of unit vector.")]
        public double? Cz { get; set; }

        public Cartesian ToCartesian()
        {
            if (RA != null && Dec != null && (Lon.HasValue || Lat.HasValue || Cx.HasValue || Cy.HasValue && Cz.HasValue) ||
                Lon.HasValue && Lat.HasValue && (RA != null || Dec != null || Cx.HasValue || Cy.HasValue && Cz.HasValue) ||
                Cx.HasValue && Cy.HasValue && Cz.HasValue && (RA != null || Dec != null || Lon.HasValue || Lat.HasValue))
            {
                throw Error.OneCoordinateRepresentationRequired();
            }

            if (RA != null && Dec != null)
            {
                if (SharpAstroLib.Coords.Angle.TryParseHmsOrDecimal(RA, out double ra) &
                    SharpAstroLib.Coords.Angle.TryParseDmsOrDecimal(Dec, out double dec))
                {
                    return new Cartesian(ra, dec);
                }
                else
                {
                    throw Error.CannotParseCoordinates(RA + "," + Dec);
                }
            }
            else if (Lon.HasValue && Lat.HasValue)
            {
                return new Cartesian(Lon.Value, Lat.Value);
            }
            else if (Cx.HasValue && Cy.HasValue && Cz.HasValue)
            {
                return new Cartesian(Cx.Value, Cy.Value, Cz.Value, true);
            }
            else
            {
                throw Error.InvalidCoordinates();
            }
        }

        public static Point ToRADec(Cartesian c)
        {
            return new Point()
            {
                RA = c.RA.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Dec = c.Dec.ToString(System.Globalization.CultureInfo.InvariantCulture),
            };
        }

        public static Point ToLonLat(Cartesian c)
        {
            return new Point()
            {
                Lon = c.RA,
                Lat = c.Dec,
            };
        }

        public static Point ToCartesian(Cartesian c)
        {
            return new Point()
            {
                Cx = c.X,
                Cy = c.Y,
                Cz = c.Z,
            };
        }
    }
}
