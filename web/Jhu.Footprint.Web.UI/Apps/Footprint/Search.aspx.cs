using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class Search : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Apps/Footprint/Search.aspx";
        }

        public override string SelectedButton
        {
            get { return App.ButtonKeySearch; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ok_Click(object sender, EventArgs e)
        {
            regionList.Visible = true;
            regionList.DataBind();

        }


        protected void footprintRegionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            /* TODO: this will go to the form
            var method = (Lib.SearchMethod)Enum.Parse(typeof(Lib.SearchMethod), SearchMethod.Value, true);
            var searchType = Lib.SearchType.Footprint;

            if (RegionSearchToggle.Checked)
            {
                searchType = Lib.SearchType.Region;
            }


            var search = new Lib.FootprintRegionSearch(FootprintContext);
            search.SearchType = searchType;
            search.SearchMethod = method;

            switch (method)
            {
                case Lib.SearchMethod.Name:
                    search.Name = name.Text;
                    break;
                case Lib.SearchMethod.Point:
                    search.Point = new Spherical.Cartesian(Convert.ToDouble(PointRAInput.Text), Convert.ToDouble(PointDecInput.Text));
                    break;
                case Lib.SearchMethod.Cone:
                    search.Point = new Spherical.Cartesian(Convert.ToDouble(ConeRAInput.Text), Convert.ToDouble(ConeDecInput.Text));
                    search.Radius = Convert.ToDouble(ConeRadiusInput.Text);
                    break;
                case Lib.SearchMethod.Intersect:
                    search.Region = Spherical.Region.Parse(IntersectRegion.Text);
                    break;
                case Lib.SearchMethod.Contain:
                    search.Region = Spherical.Region.Parse(ContainRegion.Text);
                    break;
                default:
                    throw new NotImplementedException();
            }

            e.ObjectInstance = search;
            */
        }


    }
}