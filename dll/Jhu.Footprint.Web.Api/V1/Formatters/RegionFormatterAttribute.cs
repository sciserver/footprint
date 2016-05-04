using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class RegionFormatterAttribute : StreamingRawFormatAttribute
    {
        protected override System.ServiceModel.Dispatcher.IDispatchMessageFormatter CreateDispatchFormatter()
        {
            return new RegionFormatter();
        }

        protected override System.ServiceModel.Dispatcher.IClientMessageFormatter CreateClientFormatter()
        {
            return new RegionFormatter();
        }
    }
}
