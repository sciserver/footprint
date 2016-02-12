using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    public abstract class Search : ContextObject
    {

        public Search()
        {
        }

        public Search(Context context)
            : base(context)
        {
        }
        
        public abstract int Count();
    }
}
