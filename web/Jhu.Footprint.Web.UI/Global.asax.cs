using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Web;
using Jhu.Graywulf.Web.UI;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Install;

namespace Jhu.Footprint.Web.UI
{
    public class Global : FederationApplicationBase
    {

        protected override void OnRegisterApps()
        {
            base.OnRegisterApps();

            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Common.App));
            RegisterApp(typeof(Jhu.Footprint.Web.UI.Apps.Footprint.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Api.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Docs.App));
        }

        protected override void OnRegisterServices()
        {
            base.OnRegisterServices();

            RegisterService(typeof(Jhu.Footprint.Web.Api.V1.IEditorService));
            RegisterService(typeof(Jhu.Footprint.Web.Api.V1.IFootprintService));
        }

        protected override void OnRegisterButtons()
        {
            base.OnRegisterButtons();

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "copyright",
                NavigateUrl = "~/Docs/99_info/03_copyright.aspx"
            });

            RegisterFooterButton(new Graywulf.Web.UI.MenuButton()
            {
                Text = "personnel",
                NavigateUrl = "~/Docs/99_info/01_personnel.aspx"
            });
        }
    }
}