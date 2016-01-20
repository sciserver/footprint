using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Lib
{
    public abstract class ContextObject
    {
        private Context context;

        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        public ContextObject(Context context)
        { 
            this.context = context;
        }
    }
}
