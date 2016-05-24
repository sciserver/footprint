using System;
using System.Web.UI;
using Jhu.Graywulf.Web.UI;

namespace Jhu.Footprint.Web.UI
{
    public partial class UI : MasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.Scripts.Add(new ScriptReference("Jhu.Footprint.Web.UI.Scripts.Editor.js", "Jhu.Footprint.Web.UI"));
            //footprintScriptManager.Scripts.Add(new ScriptReference("Jhu.Footprint.Web.UI.Scripts.Editor.js", "Jhu.Footprint.Web.UI"));

            //this.theScriptManager.Scripts.Add(new ScriptReference("Jhu.Footprint.Web.UI.Scripts.Coordinates.js", "Jhu.Footprint.Web.UI"));
        }
    }
}