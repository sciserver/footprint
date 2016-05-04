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
    public class RegionFormatter : StreamingRawFormatter<Spherical.Region>, IDispatchMessageFormatter, IClientMessageFormatter
    {
        protected override StreamingRawAdapter<Spherical.Region> CreateAdapter()
        {
            return new RegionAdapter();
        }
    }
}