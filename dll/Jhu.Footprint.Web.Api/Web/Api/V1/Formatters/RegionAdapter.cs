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
    public class RegionAdapter : StreamingRawAdapter<Spherical.Region>
    {
        public const string MimeTypeStc = "text/xml";

        public override string[] GetSupportedMimeTypes()
        {
            return new string[]
            {
                Jhu.Graywulf.Web.Services.Constants.MimeTypeBinary,
                Jhu.Graywulf.Web.Services.Constants.MimeTypeText,
                MimeTypeStc,
            };
        }

        #region Request

        protected override Spherical.Region OnDeserializeRequest(Stream stream, string contentType)
        {
            switch (contentType)
            {
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeText:
                    return ReadAsText(stream);
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeBinary:
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

        protected override void OnSerializeResponse(Stream stream, string contentType, Spherical.Region value)
        {
            if (value != null)
            {
                switch (contentType)
                {
                    case Jhu.Graywulf.Web.Services.Constants.MimeTypeText:
                        WriteAsText(stream, value);
                        break;
                    case Jhu.Graywulf.Web.Services.Constants.MimeTypeBinary:
                        WriteAsBinary(stream, value);
                        break;
                    case MimeTypeStc:
                        WriteAsStc(stream, value);
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
