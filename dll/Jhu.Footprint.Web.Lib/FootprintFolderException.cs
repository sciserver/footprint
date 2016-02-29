using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    [Serializable]
    public class FootprintFolderException : Exception
    {
        public FootprintFolderException(string message, Exception innerException)
            : base(message, innerException)
        { 
        }

        public FootprintFolderException(string message)
            : base(message)
        { 
        }
    }
}
