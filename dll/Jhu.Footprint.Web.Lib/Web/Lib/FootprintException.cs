using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    [Serializable]
    public class FootprintException : Exception
    {
        public FootprintException()
        {
        }

        public FootprintException(string message, Exception innerException)
            :base(message, innerException)
        {
        }

        public FootprintException(string message)
            : base(message)
        {
        }

        protected FootprintException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
