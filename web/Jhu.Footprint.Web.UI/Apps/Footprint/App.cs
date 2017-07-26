using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Graywulf.Web.UI;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public class App : AppBase
    {
        public const string ButtonKeyBrowse = "footprint-browse";
        public const string ButtonKeySearch = "footprint-search";
        public const string ButtonKeyEditor = "footprint-editor";
        public const string ButtonKeyMyFootprint = "footprint-myfootprint";

        public override void RegisterButtons(UIApplicationBase application)
        {
            application.RegisterMenuButton(
                new Graywulf.Web.UI.MenuButton()
                {
                    Key = ButtonKeyBrowse,
                    Text = "browse",
                    NavigateUrl = ""        // *** TODO
                });

            /*application.RegisterMenuButton(
                new Graywulf.Web.UI.MenuButton()
                {
                    Key = ButtonKeySearch,
                    Text = "search",
                    NavigateUrl = Search.GetUrl()
                });*/

            application.RegisterMenuButton(
                new Graywulf.Web.UI.MenuButton()
                {
                    Key = ButtonKeyEditor,
                    Text = "editor",
                    NavigateUrl = Editor.GetUrl()
                });

            /*
            application.RegisterMenuButton(
                new Graywulf.Web.UI.MenuButton()
                {
                    Key = ButtonKeyMyFootprint,
                    Text = "my footprints",
                    NavigateUrl = MyFootprint.GetUrl()
                });*/
        }
    }
}