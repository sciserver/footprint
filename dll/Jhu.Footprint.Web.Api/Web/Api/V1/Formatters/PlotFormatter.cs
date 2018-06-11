using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using Jhu.Graywulf.Web.Services.Serialization;

namespace Jhu.Footprint.Web.Api.V1
{
    public class PlotFormatter : RawMessageFormatterBase
    {
        public PlotFormatter()
        {
        }

        protected override Type GetFormattedType()
        {
            return typeof(Spherical.Visualizer.Plot);
        }

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

        protected override object OnDeserialize(Stream stream, string contentType, Type parameterType)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Response

        protected override void OnSerialize(Stream stream, string contentType, Type parameterType, object value)
        {
            var plot = (Spherical.Visualizer.Plot)value;

            switch (contentType)
            {
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeJpeg:
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypePng:
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeGif:
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeBmp:
                    WriteAsBitmap(stream, plot, contentType);
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypePdf:
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeEps:
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeEmf:
                default:
                    // TODO: wire these up to visualizer lib
                    throw new NotImplementedException();
            }
        }

        private static void WriteAsBitmap(Stream stream, Spherical.Visualizer.Plot plot, string contentType)
        {
            ImageFormat format;

            switch (contentType)
            {
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeJpeg:
                    format = ImageFormat.Jpeg;
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypePng:
                    format = ImageFormat.Png;
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeGif:
                    format = ImageFormat.Gif;
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeBmp:
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
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypePdf:
                    plot.RenderToPdf(stream);
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeEps:
                    plot.RenderToEps(stream);
                    break;
                case Jhu.Graywulf.Web.Services.Serialization.Constants.MimeTypeEmf:
                    plot.RenderToEmf(stream);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}