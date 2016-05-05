using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class OutlineAdapter : StreamingRawAdapter<Spherical.Outline>
    {
        public override string[] GetSupportedMimeTypes()
        {
            return new string[]
            {
                Constants.MimeTypeText,
            };
        }

        #region Request

        protected override Spherical.Outline OnDeserializeRequest(Stream stream, string contentType)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        #region Response

        protected override void OnSerializeResponse(Stream stream, string contentType, Spherical.Outline value)
        {
            switch (contentType)
            {
                case Constants.MimeTypeText:
                    WriteAsText(stream, value);
                    break;
                default:
                    throw new NotImplementedException();
            }   
        }

        private static void WriteAsText(Stream stream, Spherical.Outline outline)
        {
            using (var writer = new StreamWriter(stream, System.Text.Encoding.ASCII))
            {
                outline.ToString(writer, "    ", 0);
            }
        }

        #endregion
    }
}
