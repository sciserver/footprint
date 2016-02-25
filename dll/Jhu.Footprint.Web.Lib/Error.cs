using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    static class Error
    {
        public static FootprintException DuplicateFootprintName(string name)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateFootprintName,
                name);

            return new FootprintException(message);
        }

        public static FootprintFolderException DuplicateFootprintFolderName(string name)
        {
            var message = String.Format(
                ExceptionMessages.DuplicateFootprintFolderName,
                name);

            return new FootprintFolderException(message);
        }
    }
}
