using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    [Serializable]
    public class FootprintException : Exception
    {
        public FootprintException(string message, Exception innerException)
            :base(message, innerException)
        {
        }

        public FootprintException(string message)
            : base(message)
        {
        }
    }
}
