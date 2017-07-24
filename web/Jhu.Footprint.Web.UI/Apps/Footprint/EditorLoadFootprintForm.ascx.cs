using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class LoadFootprintView : CustomUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FootprintSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j;
            if (Int32.TryParse(FootprintSelect.SelectedValue, out j))
            {
                RefreshFootprintRegionList(j);
            }
        }

        protected void LoadRegionButton_OnClick(object sender, EventArgs e)
        {
            var footprintName = FootprintSelect.SelectedItem.ToString();
            var regionName = RegionSelect.SelectedItem.ToString();
            var es = new Api.V1.EditorService();
            es.Load(Page.User.Identity.Name, footprintName, regionName);

            Response.Redirect(String.Format("Editor.aspx?footprintName={0}&regionName={1}", footprintName, regionName));
        }

        private void RefreshFootprintList()
        {
            FootprintSelect.Items.Clear();
            FootprintSelect.Items.Add(new ListItem("Select item...", ""));
            FootprintSelect.Items[0].Attributes.Add("disabled", "disabled");
            FootprintSelect.Items[0].Attributes.Add("selected", "True");

            var s = new Lib.FootprintSearch(FootprintContext)
            {
                Owner = Page.User.Identity.Name
            };

            foreach (var f in s.Find())
            {
                var it = new ListItem()
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                };

                FootprintSelect.Items.Add(it);
            }
        }

        private void RefreshFootprintRegionList(int id)
        {
            RegionSelect.Items.Clear();
            RegionSelect.Items.Add(new ListItem("Select item...", ""));
            RegionSelect.Items[0].Attributes.Add("disabled", "disabled");
            RegionSelect.Items[0].Attributes.Add("selected", "True");


            var s = new Lib.FootprintRegionSearch(FootprintContext)
            {
                Owner = Page.User.Identity.Name,
                FootprintId = id
            };

            foreach (var r in s.Find())
            {
                var it = new ListItem()
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                };

                RegionSelect.Items.Add(it);
            }
        }
    }
}