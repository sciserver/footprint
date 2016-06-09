using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;
using System.ServiceModel.Dispatcher;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionFormatterAttribute : StreamingRawFormatAttribute
    {
        protected override StreamingRawFormatterBase CreateDispatchFormatter(IDispatchMessageFormatter formatter)
        {
            return new RegionFormatter(formatter);
        }

        protected override StreamingRawFormatterBase CreateClientFormatter(IClientMessageFormatter formatter)
        {
            return new RegionFormatter(formatter);
        }
    }
}
