using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    public static class CoordinateTransformations
    {

        #region Equatorial to Galactic
        public static double GetLatitudeFromEquatorial(double ra, double dec)
        {
            ra = ToRad(ra);
            dec = ToRad(dec);
            double sinb = Math.Sin(Constants.decGP) * Math.Sin(dec) + Math.Cos(Constants.decGP) * Math.Cos(dec) * Math.Cos(ra - Constants.raGP);
            double b = Math.Asin(sinb);
            return ToDeg(b);
        }

        public static double GetLongitudeFromEquatorial(double ra, double dec, double b)
        {
            ra = ToRad(ra);
            dec = ToRad(dec);
            b = ToRad(b);

            // the two equation to get sin(l_CP-l) = sinE and cos(l_CP-l) = cosE
            double sinE = (Math.Cos(dec) * Math.Sin(ra - Constants.raGP)) / Math.Cos(b);
            double cosE = (Math.Cos(Constants.decGP) * Math.Sin(dec) - Math.Sin(Constants.decGP) * Math.Cos(dec) * Math.Cos(ra - Constants.raGP)) / Math.Cos(b);

            double E = Math.Asin(sinE);

            // find the correct quadrant
            if (sinE >= 0 & cosE < 0 | sinE < 0 & cosE < 0)
            {
                E = Math.PI - E;
            }
            else if (sinE < 0 & cosE >= 0)
            {
                E = Math.PI * 2.0 + E;
            }

            double l = Constants.lCP - E;

            //Ensure l lies in the range 0-2pi 

            if (l < 0) l = l + (2.0 * Math.PI);
            if (l > (2.0 * Math.PI)) l = l - (2.0 * Math.PI);

            return ToDeg(l);
        }
        #endregion

        #region Galactic to Equatorial
        public static double GetDeclinationFromGalactic(double l, double b)
        {
            l = ToRad(l);
            b = ToRad(b);

            double sind = Math.Sin(Constants.decGP) * Math.Sin(b) + Math.Cos(Constants.decGP) * Math.Cos(b) * Math.Cos(Constants.lCP - l);
            double dec = Math.Asin(sind);
            return ToDeg(dec);
        }

        public static double GetRightAscensionFromGalactic(double l, double b, double dec)
        {
            l = ToRad(l);
            b = ToRad(b);
            dec = ToRad(dec);

            // the two equation to get sin(ra - ra_GP) = sinE and cos(ra - ra_GP) = cosE
            double sinE = (Math.Cos(b) * Math.Sin(Constants.lCP - l)) / Math.Cos(dec);
            double cosE = (Math.Cos(Constants.decGP) * Math.Sin(b) - Math.Sin(Constants.decGP) * Math.Cos(b) * Math.Cos(Constants.lCP - l)) / Math.Cos(dec);

            double E = Math.Asin(sinE);

            // find the correct quadrant
            if (sinE >= 0 & cosE < 0 | sinE < 0 & cosE < 0)
            {
                E = Math.PI - E;
            }
            else if (sinE < 0 & cosE >= 0)
            {
                E = Math.PI * 2.0 + E;
            }

            double ra = Constants.raGP + E;

            // Ensure ra lies in the range 0-2pi 

            if (ra < 0) l = l + (2.0 * Math.PI);
            if (ra > (2.0 * Math.PI)) l = l - (2.0 * Math.PI);

            return ToDeg(ra);
        }

        #endregion

        #region Radian - degree conversions
        private static double ToRad(double x)
        {
            return x * 2 * Math.PI / 360;
        }

        private static double ToDeg(double x)
        {
            return x * 360 / (2 * Math.PI);
        }
        #endregion
    }
}
