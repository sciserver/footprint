using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionMessageWriter : StreamBodyWriter
    {
        private Spherical.Region region;
        private string mimeType;

        public RegionMessageWriter(Spherical.Region region, string mimeType)
            : base(false)
        {
            this.region = region;
            this.mimeType = mimeType;
        }

        protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
        {
            // NOTE: this might break in .Net 4.5
            // In .Net 4.5 it is not necessary to write out Binary tag

            //writer.WriteStartElement("Binary");

            base.OnWriteBodyContents(writer);

            //writer.WriteEndElement();
        }

        protected override void OnWriteBodyContents(Stream stream)
        {
            if (region != null)
            {
                switch (mimeType)
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
            var writer = new StreamWriter(stream, Encoding.ASCII);
            //region.ToString(writer, "    ", 0);

            for (int i = 0; i < 10; i++)
            {
                writer.WriteLine("Hello world.");
                writer.Flush();
                System.Threading.Thread.Sleep(3000);
            }

            
        }
    }
}