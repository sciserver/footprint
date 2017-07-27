﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class CircleModal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            circleNameFormatValidator.ValidationExpression = Lib.Constants.NamePattern;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType().FullName, "CircleModal.ascx.js");
        }
    }
}