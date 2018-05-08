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
        public override List<RestBodyFormat> GetSupportedFormats()
        {
            return new List<RestBodyFormat>()
            {
                RestBodyFormats.Png,
                RestBodyFormats.Jpeg,
                RestBodyFormats.Gif,
                RestBodyFormats.Bmp,
                RestBodyFormats.Pdf,
                RestBodyFormats.Eps,
                RestBodyFormats.Emf,
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
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeJpeg:
                case Jhu.Graywulf.Web.Services.Constants.MimeTypePng:
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeGif:
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeBmp:
                    WriteAsBitmap(stream, plot, contentType);
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypePdf:
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeEps:
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeEmf:
                default:
                    throw new NotImplementedException();
            }
        }

        private static void WriteAsBitmap(Stream stream, Spherical.Visualizer.Plot plot, string contentType)
        {
            ImageFormat format;

            switch (contentType)
            {
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeJpeg:
                    format = ImageFormat.Jpeg;
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypePng:
                    format = ImageFormat.Png;
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeGif:
                    format = ImageFormat.Gif;
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeBmp:
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
                case Jhu.Graywulf.Web.Services.Constants.MimeTypePdf:
                    plot.RenderToPdf(stream);
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeEps:
                    plot.RenderToEps(stream);
                    break;
                case Jhu.Graywulf.Web.Services.Constants.MimeTypeEmf:
                    plot.RenderToEmf(stream);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
