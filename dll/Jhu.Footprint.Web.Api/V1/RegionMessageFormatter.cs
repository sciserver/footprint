using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionMessageFormatter : GraywulfMessageFormatter, IDispatchMessageFormatter, IClientMessageFormatter
    {
        public const string MimeTypeText = "text/plain";
        public const string MimeTypeStc = "text/xml";
        public const string MimeTypeBinary = "application/octet-stream";

        public override string[] GetSupportedMimeTypes()
        {
            return new string[]
            {
                MimeTypeText,
                MimeTypeStc,
                MimeTypeBinary,
            };
        }

        #region Writer

        public override void DeserializeRequest(Message message, object[] parameters)
        {
            var body = message.GetReaderAtBodyContents();
            byte[] raw = body.ReadContentAsBase64();

            using (var ms = new MemoryStream(raw))
            {
                switch (MimeType)
                {
                    case RegionMessageFormatter.MimeTypeText:
                        parameters[parameters.Length - 1] = ReadAsText(ms);
                        break;
                    case RegionMessageFormatter.MimeTypeBinary:
                        parameters[parameters.Length - 1] = ReadAsBinary(ms);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private Spherical.Region ReadAsText(Stream stream)
        {
            using (var reader = new StreamReader(stream, System.Text.Encoding.ASCII))
            {
                // TODO: modify to read from stream
                var text = reader.ReadToEnd();
                return Spherical.Region.Parse(text);
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

        public override Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            var region = (Spherical.Region)result;

            var message = WebOperationContext.Current.CreateStreamResponse(s => { OnWriteBodyContents(s, region); }, MimeType);

            return message;
        }

        protected void OnWriteBodyContents(Stream stream, Spherical.Region region)
        {
            if (region != null)
            {
                switch (MimeType)
                {
                    case RegionMessageFormatter.MimeTypeText:
                        WriteAsTest(stream, region);
                        break;
                    case RegionMessageFormatter.MimeTypeBinary:
                        WriteAsBinary(stream, region);
                        break;
                    case RegionMessageFormatter.MimeTypeStc:
                        WriteAsStc(stream, region);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void WriteAsTest(Stream stream, Spherical.Region region)
        {
            using (var writer = new StreamWriter(stream, System.Text.Encoding.ASCII))
            {
                region.ToString(writer, "    ", 0);
            }
        }

        private void WriteAsBinary(Stream stream, Spherical.Region region)
        {
            using (var writer = new Spherical.IO.RegionWriter(stream))
            {
                writer.Write(region);
                writer.Flush();
            }
        }

        private void WriteAsStc(Stream stream, Spherical.Region region)
        {
            var stc = STC.Adapter.FromRegion(region);
            var s = new XmlSerializer(stc.GetType());
            s.Serialize(stream, region);
            stream.Flush();
        }

        #endregion
        #region Reader

        public override Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object DeserializeReply(Message message, object[] parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}