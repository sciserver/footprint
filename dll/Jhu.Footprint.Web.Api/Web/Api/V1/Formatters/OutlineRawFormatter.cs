using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
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
        public OutlineFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IDispatchMessageFormatter fallbackFormatter)
            : base(operationDescription, endpoint, fallbackFormatter)
        {
        }

        public OutlineFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IClientMessageFormatter fallbackFormatter)
            : base(operationDescription, endpoint, fallbackFormatter)
        {
        }

        protected override StreamingRawAdapter<Spherical.Outline> CreateAdapter()
        {
            return new OutlineAdapter();
        }
    }
}