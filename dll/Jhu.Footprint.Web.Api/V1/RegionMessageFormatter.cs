using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.IO;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionMessageFormatter : GraywulfMessageFormatter, IDispatchMessageFormatter
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

        public override void DeserializeRequest(Message message, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            var region = (Spherical.Region)result;
            //var writer = new RegionMessageWriter(region, MimeType);

            //return WebOperationContext.Current.CreateStreamResponse(writer, MimeType);

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
                        WriteAsTest(stream);
                        break;
                    case RegionMessageFormatter.MimeTypeStc:
                    case RegionMessageFormatter.MimeTypeBinary:
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void WriteAsTest(Stream stream)
        {
            var writer = new StreamWriter(stream, System.Text.Encoding.ASCII);
            //region.ToString(writer, "    ", 0);

            for (int i = 0; i < 5; i++)
            {
                writer.WriteLine("Hello world.");
                writer.Flush();
                System.Threading.Thread.Sleep(3000);
            }


        }
    }
}