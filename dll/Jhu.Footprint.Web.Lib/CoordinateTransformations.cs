using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public static class CoordinateTransformations
    {

        #region Equatorial to Galactic
        public static double GetLatitudeFromEquatorial(double ra, double dec)
        {
            ra = ra * Constant.Degree2Radian;
            dec = dec * Constant.Degree2Radian;
            double sinb = Math.Sin(Constants.decGP) * Math.Sin(dec) + Math.Cos(Constants.decGP) * Math.Cos(dec) * Math.Cos(ra - Constants.raGP);
            double b = Math.Asin(sinb);
            return b * Constant.Radian2Degree;
        }

        public static double GetLongitudeFromEquatorial(double ra, double dec, double b)
        {
            ra = ra * Constant.Degree2Radian;
            dec = dec * Constant.Degree2Radian;
            b = b * Constant.Degree2Radian;

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

            return l * Constant.Radian2Degree;
        }
        #endregion

        #region Galactic to Equatorial
        public static double GetDeclinationFromGalactic(double l, double b)
        {
            l = l * Constant.Degree2Radian;
            b = b * Constant.Degree2Radian;

            double sind = Math.Sin(Constants.decGP) * Math.Sin(b) + Math.Cos(Constants.decGP) * Math.Cos(b) * Math.Cos(Constants.lCP - l);
            double dec = Math.Asin(sind);
            return dec * Constant.Radian2Degree;
        }

        public static double GetRightAscensionFromGalactic(double l, double b, double dec)
        {
            l = l * Constant.Degree2Radian;
            b = b * Constant.Degree2Radian;
            dec = dec * Constant.Degree2Radian;

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

            // Ensure right ascension (ra) lies in the range 0-2pi 

            if (ra < 0) ra = ra + (2.0 * Math.PI);
            if (ra > (2.0 * Math.PI)) ra = ra - (2.0 * Math.PI);

            return ra * Constant.Radian2Degree;
        }

        #endregion
    }
}
