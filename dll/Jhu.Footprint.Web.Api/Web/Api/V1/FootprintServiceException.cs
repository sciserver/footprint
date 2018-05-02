using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Jhu.Footprint.Web.Api.V1
{
    [Serializable]
    public class FootprintServiceException : Exception, ISerializable
    {
        public FootprintServiceException()
        {
        }

        public FootprintServiceException(string message)
            : base(message)
        {
        }

        public FootprintServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FootprintServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
