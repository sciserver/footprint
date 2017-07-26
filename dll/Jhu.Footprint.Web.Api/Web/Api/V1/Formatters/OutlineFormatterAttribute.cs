﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace Jhu.Footprint.Web.Api.V1
{
    public class OutlineFormatterAttribute : StreamingRawFormatAttribute
    {
        protected override StreamingRawFormatterBase OnCreateDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IDispatchMessageFormatter fallbackFormatter)
        {
            return new OutlineFormatter(operationDescription, endpoint, fallbackFormatter);
        }

        protected override StreamingRawFormatterBase OnCreateClientFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint, IClientMessageFormatter fallbackFormatter)
        {
            return new OutlineFormatter(operationDescription, endpoint, fallbackFormatter);
        }
    }
}