using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    static class Error
    {
        #region Footprint Errors
        public static FootprintException DuplicateFootprintName(string name)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateFootprintName,
                name);

            return new FootprintException(message);
        }

        public static FootprintException NoFootprintDataToLoad()
        {
            var message = ExceptionMessages.NoFootprintDataToLoad;
            return new FootprintException(message);
        }

        public static FootprintException CannotFindFootprint(string user, string folder, string name)
        {
            var message = String.Format(
                ExceptionMessages.CannotFindFootprint,
                user, folder, name);
            return new FootprintException(message);
        }

        public static FootprintException FootprintNameNotAvailable(string name)
        {
            var message = String.Format(ExceptionMessages.FootprintNameNotAvailable, name);
            return new FootprintException(message);
        }

        public static FootprintException FootprintNameInvalid(string name)
        {
            var message = String.Format(ExceptionMessages.FootprintNameInvalid, name);
            return new FootprintException(message);
        }

        #endregion

        #region Footprint Folder Errors
        public static FootprintFolderException DuplicateFootprintFolderName(string name)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateFootprintFolderName,
                name);

            return new FootprintFolderException(message);
        }

        public static FootprintFolderException NoFootprintFolderDataToLoad()
        {
            var message = ExceptionMessages.NoFootprintFolderDataToLoad;
            return new FootprintFolderException(message);
        }

        public static FootprintFolderException CannotFindfootprintFolder(string user, string name)
        {
            var message = String.Format(
                ExceptionMessages.CannotFindFootprintFolder,
                user, name);

            return new FootprintFolderException(message);
        }
        #endregion
    }
}
