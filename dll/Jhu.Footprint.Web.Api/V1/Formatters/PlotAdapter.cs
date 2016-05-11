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
using System.Drawing;
using System.Drawing.Imaging;
using Jhu.Graywulf.Web.Services;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.Api.V1
{
    public class PlotAdapter : StreamingRawAdapter<Spherical.Visualizer.Plot>
    {
        public override string[] GetSupportedMimeTypes()
        {
            return new string[]
            {
                Constants.MimeTypeJpeg,
                Constants.MimeTypePng,
                Constants.MimeTypeGif,
                Constants.MimeTypeBmp,
                Constants.MimeTypePdf,
                Constants.MimeTypeEps,
                Constants.MimeTypeEmf,
            };
        }

        #region Request

        protected override Spherical.Visualizer.Plot OnDeserializeRequest(Stream stream, string contentType)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Response

        protected override void OnSerializeResponse(Stream stream, string contentType, Spherical.Visualizer.Plot plot)
        {
            switch (contentType)
            {
                case Constants.MimeTypeJpeg:
                case Constants.MimeTypePng:
                case Constants.MimeTypeGif:
                case Constants.MimeTypeBmp:
                    WriteAsBitmap(stream, plot, contentType);
                    break;
                case Constants.MimeTypePdf:
                case Constants.MimeTypeEps:
                case Constants.MimeTypeEmf:
                default:
                    throw new NotImplementedException();
            }
        }

        private static void WriteAsBitmap(Stream stream, Spherical.Visualizer.Plot plot, string contentType)
        {
            ImageFormat format;

            switch (contentType)
            {
                case Constants.MimeTypeJpeg:
                    format = ImageFormat.Jpeg;
                    break;
                case Constants.MimeTypePng:
                    format = ImageFormat.Png;
                    break;
                case Constants.MimeTypeGif:
                    format = ImageFormat.Gif;
                    break;
                case Constants.MimeTypeBmp:
                    format = ImageFormat.Bmp;
                    break;
                default:
                    throw new NotImplementedException();
            }

            plot.RenderToBitmap(stream, format);
        }

        private static void WriteAsVector(Stream stream, Spherical.Visualizer.Plot plot, string contentType)
        {
            switch (contentType)
            {
                case Constants.MimeTypePdf:
                    plot.RenderToPdf(stream);
                    break;
                case Constants.MimeTypeEps:
                    plot.RenderToEps(stream);
                    break;
                case Constants.MimeTypeEmf:
                    plot.RenderToEmf(stream);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
