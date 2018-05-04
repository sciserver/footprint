using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Jhu.Graywulf.AccessControl;

namespace Jhu.Footprint.Web.Lib
{
    public static class Error
    {
        public static FootprintException RestrictedName(string name)
        {
            var message = String.Format(ExceptionMessages.RestrictedName, name);
            return new FootprintException(message);
        }

        public static FootprintException InvalidName(string name)
        {
            var message = String.Format(ExceptionMessages.InvalidName, name);
            return new FootprintException(message);
        }

        public static AccessDeniedException AccessDenied()
        {
            return new AccessDeniedException(ExceptionMessages.AccessDenied);
        }

        public static FootprintException RegionNotSimplified()
        {
            return new FootprintException(ExceptionMessages.RegionNotSimplified);
        }

        #region Footprint errors

        public static DuplicateNameException DuplicateFootprintName(string userName, string footprintName)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateFootprintName,
                userName,
                footprintName);

            return new DuplicateNameException(message);
        }

        /* TODO: delete
        public static FootprintException NoFootprintFolderDataToLoad()
        {
            var message = ExceptionMessages.NoFootprintFolderDataToLoad;
            return new FootprintException(message);
        }
        */

        public static FootprintException FootprintNotFound(string userName, string footprintName)
        {
            var message = String.Format(
                ExceptionMessages.FootprintNotFound,
                userName, 
                footprintName);

            return new FootprintException(message);
        }

        #endregion
        #region Region errors

        public static DuplicateNameException DuplicateRegionName(string userName, string footprintName, string regionName)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateRegionName,
                userName,
                footprintName,
                regionName);

            return new DuplicateNameException(message);
        }

        /* TODO: delete
        public static FootprintException NoRegionDataToLoad()
        {
            var message = ExceptionMessages.NoFootprintDataToLoad;
            return new FootprintException(message);
        }
        */

        public static KeyNotFoundException RegionNotFound(string userName, string footprintName, string regionName)
        {
            var message = String.Format(
                ExceptionMessages.RegionNotFound,
                userName, 
                footprintName, 
                regionName);

            return new KeyNotFoundException(message);
        }
        
        #endregion

        
    }
}
