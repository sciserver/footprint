using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Jhu.Graywulf.Web.Services;
using Jhu.Graywulf.Web.Services.Serialization;

namespace Jhu.Footprint.Web.Api.V1
{
    public class OutlineFormatter : RawMessageFormatterBase
    {
        public OutlineFormatter()
        {
        }

        protected override Type GetFormattedType()
        {
            return typeof(Spherical.Outline);
        }

        public override List<RestBodyFormat> GetSupportedFormats()
        {
            return new List<RestBodyFormat>()
            {
                RestBodyFormats.Text,
            };
        }

        #region Request

        protected override object OnDeserializeRequest(Stream stream, string contentType, Type parameterType)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Response

        protected override void OnSerializeResponse(Stream stream, string contentType, Type parameterType, object value)
        {
            if (value != null)
            {
                switch (contentType)
                {
                    case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeText:
                        WriteAsText(stream, (Spherical.Outline)value);
                        break;
                    default:
                        throw new NotImplementedException();
                }
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