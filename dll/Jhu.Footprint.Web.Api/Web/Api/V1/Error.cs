using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Api.V1
{
    static class Error
    {
        public static FootprintServiceException OneCoordinateRepresentationRequired()
        {
            return new FootprintServiceException(ExceptionMessages.OneCoordinateRepresentationRequired);
        }

        public static FormatException CannotParseCoordinates(string coords)
        {
            return new FormatException(String.Format(ExceptionMessages.CannotParseCoordinates, coords));
        }

        public static FootprintServiceException InvalidCoordinates()
        {
            return new FootprintServiceException(ExceptionMessages.InvalidCoordinates);
        }

        public static FootprintServiceException OneAngleRepresentationRequired()
        {
            return new FootprintServiceException(ExceptionMessages.OneAngleRepresentationRequired);
        }

        public static FormatException CannotParseAngle(string angle)
        {
            return new FormatException(String.Format(ExceptionMessages.CannotParseAngle, angle));
        }

        public static FootprintServiceException InvalidAngle()
        {
            return new FootprintServiceException(ExceptionMessages.InvalidAngle);
        }

        public static FootprintServiceException NoRegionSpecified()
        {
            return new FootprintServiceException(ExceptionMessages.NoRegionSpecified);
        }

        public static FootprintServiceException MultipleRegionsSpecified()
        {
            return new FootprintServiceException(ExceptionMessages.MultipleRegionsSpecified);
        }
    }
}
