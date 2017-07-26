﻿using System;
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
    public class PlotRawFormatter : StreamingRawFormatter<Spherical.Visualizer.Plot>, IDispatchMessageFormatter, IClientMessageFormatter
    {
        public PlotRawFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IDispatchMessageFormatter fallbackFormatter)
            : base(operationDescription, endpoint, fallbackFormatter)
        {
        }

        public PlotRawFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IClientMessageFormatter fallbackFormatter)
            : base(operationDescription, endpoint, fallbackFormatter)
        {
        }

        protected override StreamingRawAdapter<Spherical.Visualizer.Plot> CreateAdapter()
        {
            return new PlotAdapter();
        }
    }
}