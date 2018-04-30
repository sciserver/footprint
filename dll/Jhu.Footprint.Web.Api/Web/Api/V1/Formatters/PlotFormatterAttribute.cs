using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace Jhu.Footprint.Web.Api.V1
{
    public class PlotFormatterAttribute : StreamingRawFormatAttribute
    {
        protected override StreamingRawFormatterBase OnCreateFormatter()
        {
            return new PlotRawFormatter();
        }
    }
}
