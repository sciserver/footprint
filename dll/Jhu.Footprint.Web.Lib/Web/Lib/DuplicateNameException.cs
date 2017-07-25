using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    [Serializable]
    public class DuplicateNameException : FootprintException
    {
        public DuplicateNameException(string message)
            : base(message)
        {
        }
    }
}
