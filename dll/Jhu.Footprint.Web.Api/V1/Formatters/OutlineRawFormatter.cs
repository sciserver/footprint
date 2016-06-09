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
    public class OutlineFormatter : StreamingRawFormatter<Spherical.Outline>, IDispatchMessageFormatter, IClientMessageFormatter
    {
        public OutlineFormatter(IDispatchMessageFormatter dispatchMessageFormatter)
            :base(dispatchMessageFormatter)
        {
        }

        public OutlineFormatter(IClientMessageFormatter clientMessageFormatter)
            :base(clientMessageFormatter)
        {
        }

        protected override StreamingRawAdapter<Spherical.Outline> CreateAdapter()
        {
            return new OutlineAdapter();
        }
    }
}