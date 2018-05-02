using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    public class Angle
    {
        [DataMember(Name = "degrees", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Angle in degrees")]
        public string Degrees { get; set; }

        [DataMember(Name = "radians", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Angle in radians")]
        public double? Radians { get; set; }

        [DataMember(Name = "arcMinutes", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Angle in arc minutes")]
        public double? ArcMinutes { get; set; }

        [DataMember(Name = "arcSeconds", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Angle in arc seconds")]
        public double? ArcSeconds { get; set; }

        public double ToDegrees()
        {
            EnsureValid();

            if (Degrees != null)
            {
                if (SharpAstroLib.Coords.Angle.TryParseDmsOrDecimal(Degrees, out double angle))
                {
                    return angle;
                }
                else
                {
                    throw Error.CannotParseAngle(Degrees);
                }
            }
            else if (Radians.HasValue)
            {
                return Radians.Value * 180.0 / Math.PI;
            }
            else if (ArcMinutes.HasValue)
            {
                return ArcMinutes.Value / 60.0;
            }
            else if (ArcSeconds.HasValue)
            {
                return ArcSeconds.Value / 3600.0;
            }
            else
            {
                throw Error.InvalidAngle();
            }
        }

        public double ToRadians()
        {
            EnsureValid();

            if (Degrees != null)
            {
                if (SharpAstroLib.Coords.Angle.TryParseDmsOrDecimal(Degrees, out double angle))
                {
                    return angle / 180.0 * Math.PI;
                }
                else
                {
                    throw Error.CannotParseAngle(Degrees);
                }
            }
            else if (Radians.HasValue)
            {
                return Radians.Value;
            }
            else if (ArcMinutes.HasValue)
            {
                return ArcMinutes.Value / 60.0 / 180.0 * Math.PI;
            }
            else if (ArcSeconds.HasValue)
            {
                return ArcSeconds.Value / 3600.0 / 180.0 * Math.PI;
            }
            else
            {
                throw Error.InvalidAngle();
            }
        }

        private void EnsureValid()
        {
            if (Degrees != null && (Radians.HasValue || ArcMinutes.HasValue || ArcSeconds.HasValue) ||
                Radians.HasValue && (Degrees != null || ArcMinutes.HasValue || ArcSeconds.HasValue) ||
                ArcMinutes.HasValue && (Degrees != null || Radians.HasValue || ArcSeconds.HasValue) ||
                ArcSeconds.HasValue && (Degrees != null || Radians.HasValue || ArcMinutes.HasValue))
            {
                throw Error.OneAngleRepresentationRequired();
            }
        }
    }
}
