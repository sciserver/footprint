using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.UI
{
    public class CustomPageBase : Jhu.Graywulf.Web.UI.PageBase
    {
        private Context footprintContext;

        public Context FootprintContext
        {
            get 
            {
                if (footprintContext == null)
                {
                    CreateContext();
                }

                return footprintContext;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            if (footprintContext != null)
            {
                footprintContext.Dispose();
            }

            base.OnUnload(e);
        }

        private void CreateContext()
        {
            footprintContext = new Context()
            {
                Principal = Page.User
            };
        }
    }
}