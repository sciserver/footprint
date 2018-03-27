using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    [Serializable]
    public class DuplicateNameException : FootprintException
    {
        public DuplicateNameException()
        {
        }

        public DuplicateNameException(string message)
            : base(message)
        {
        }

        protected DuplicateNameException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
