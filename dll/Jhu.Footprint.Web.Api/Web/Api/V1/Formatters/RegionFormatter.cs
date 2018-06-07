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
using Jhu.Graywulf.Web.Services.Serialization;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionFormatter : RawMessageFormatterBase
    {
        public const string MimeTypeStc = "text/xml";

        public RegionFormatter()
        {
        }

        protected override Type GetFormattedType()
        {
            return typeof(Spherical.Region);
        }

        public override List<RestBodyFormat> GetSupportedFormats()
        {
            return new List<RestBodyFormat>()
            {
                RestBodyFormats.Text,
                new RestBodyFormat("binary", "dat", Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeBinary),
                new RestBodyFormat("stc", "stc", MimeTypeStc),
            };
        }

        #region Request

        protected override object OnDeserializeRequest(Stream stream, string contentType, Type parameterType)
        {
            switch (contentType)
            {
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeText:
                    return ReadAsText(stream);
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeBinary:
                    return ReadAsBinary(stream);
                case MimeTypeStc:
                    return ReadAsStc(stream);
                default:
                    throw new NotImplementedException();
            }
        }

        private Spherical.Region ReadAsText(Stream stream)
        {
            using (var reader = new StreamReader(stream, System.Text.Encoding.ASCII))
            {
                // TODO: modify to read from stream
                var text = reader.ReadToEnd();
                var region = Spherical.Region.Parse(text);
                region.Simplify();

                return region;
            }
        }

        private Spherical.Region ReadAsBinary(Stream stream)
        {
            using (var r = new Spherical.IO.RegionReader(stream))
            {
                return r.ReadRegion();
            }
        }

        private Spherical.Region ReadAsStc(Stream stream)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Response

        protected override void OnSerializeResponse(Stream stream, string contentType, Type parameterType, object value)
        {
            var region = (Spherical.Region)value;

            if (region != null)
            {
                switch (contentType)
                {
                    case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeText:
                        WriteAsText(stream, region);
                        break;
                    case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeBinary:
                        WriteAsBinary(stream, region);
                        break;
                    case MimeTypeStc:
                        WriteAsStc(stream, region);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void WriteAsText(Stream stream, Spherical.Region region)
        {
            using (var writer = new StreamWriter(stream, System.Text.Encoding.ASCII))
            {
                region.ToString(writer, "    ", 0);
            }
        }

        private static void WriteAsBinary(Stream stream, Spherical.Region region)
        {
            using (var writer = new Spherical.IO.RegionWriter(stream))
            {
                writer.Write(region);
                writer.Flush();
            }
        }

        private static void WriteAsStc(Stream stream, Spherical.Region region)
        {
            var stc = STC.Adapter.FromRegion(region);
            var s = new XmlSerializer(stc.GetType());
            s.Serialize(stream, region);
            stream.Flush();
        }

        #endregion
    }
}