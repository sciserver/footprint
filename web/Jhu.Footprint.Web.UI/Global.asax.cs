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
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Install;

namespace Jhu.Footprint.Web.UI
{
    public class Global : FederationApplicationBase
    {

        protected override void RegisterApps()
        {
            base.RegisterApps();

            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Common.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Api.App));
            RegisterApp(typeof(Jhu.Graywulf.Web.UI.Apps.Docs.App));
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            RegisterService(typeof(Jhu.Footprint.Web.Api.V1.IEditorService));
            RegisterService(typeof(Jhu.Footprint.Web.Api.V1.IFootprintService));
        }

        protected override void RegisterButtons()
        {
            base.RegisterButtons();

            RegisterMenuButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text = "search",
                NavigateUrl = Search.GetUrl()
            });

            RegisterMenuButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text= "editor",
                NavigateUrl = Editor.GetUrl()
            });

            RegisterMenuButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text = "my footprints",
                NavigateUrl = MyFootprint.GetUrl()
            });

            RegisterFooterButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text = "copyright",
                NavigateUrl = "~/Docs/99_info/03_copyright.aspx"
            });

            RegisterFooterButton(new Graywulf.Web.UI.Controls.MenuButton()
            {
                Text = "personnel",
                NavigateUrl = "~/Docs/99_info/01_personnel.aspx"
            });
        }
    }
}