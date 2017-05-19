using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Jhu.Footprint.Web.Lib
{
    public static class AppSettings
    {
        public static string WebUIPath
        {
            get
            {
                return ConfigurationManager.AppSettings["Jhu.Footprint.Web.UI.Path"] ?? "/footprint";
            }
        }
    }
}
