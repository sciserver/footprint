using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class FootprintRegionList : System.Web.UI.UserControl
    {
        private Lib.FootprintRegionSearch searchObject;

        public Lib.FootprintRegionSearch SearchObject
        {
            get { return searchObject; }
            set { searchObject = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void dataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            searchObject.Context = ((CustomPageBase)Page).FootprintContext;
            e.ObjectInstance = searchObject;
        }
    }
}