using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI
{
    public partial class Search : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Search.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ok_Click(object sender, EventArgs e)
        {
            footprintList.Visible = true;
            footprintList.DataBind();
        }
        

        protected void footprintRegionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            var method = (Lib.SearchMethod)Enum.Parse(typeof(Lib.SearchMethod), SearchMethod.Value, true);
            var search = new Lib.FootprintRegionSearch(FootprintContext)
            {                
                SearchMethod = method
            };

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
                case Lib.SearchMethod.Contain:
                default:
                    throw new NotImplementedException();
            }

            e.ObjectInstance = search;

        }
    }
}