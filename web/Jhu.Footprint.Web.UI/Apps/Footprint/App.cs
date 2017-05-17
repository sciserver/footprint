using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Graywulf.Web.UI;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public class App : AppBase
    {
        public override void RegisterButtons(UIApplicationBase application)
        {
            base.RegisterButtons(application);

            application.RegisterMenuButton(
                new Graywulf.Web.UI.Controls.MenuButton()
                {
                    Text = "search",
                    NavigateUrl = Search.GetUrl()
                });

            application.RegisterMenuButton(
                new Graywulf.Web.UI.Controls.MenuButton()
                {
                    Text = "editor",
                    NavigateUrl = Editor.GetUrl()
                });

            application.RegisterMenuButton(
                new Graywulf.Web.UI.Controls.MenuButton()
                {
                    Text = "my footprints",
                    NavigateUrl = MyFootprint.GetUrl()
                });
        }
    }
}