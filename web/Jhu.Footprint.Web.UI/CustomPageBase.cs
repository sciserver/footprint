using System;
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
                    footprintContext = new Context();
                }

                return footprintContext;
            }
        }

        /*
        protected override void OnPreRender(EventArgs e)
        {
            Page.DataBind();

            base.OnPreRender(e);
        }*/

        protected override void OnUnload(EventArgs e)
        {
            if (footprintContext != null)
            {
                footprintContext.Dispose();
            }

            base.OnUnload(e);
        }
    }
}