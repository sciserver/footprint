using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class SearchForm : System.Web.UI.UserControl
    {
        public event EventHandler<EventArgs> Click;

        public Lib.SearchType SearchType
        {
            get
            {
                return (Lib.SearchType)Enum.Parse(typeof(Lib.SearchType), searchMode.SelectedValue, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ok_Click(object sender, EventArgs e)
        {
            Click?.Invoke(sender, e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(typeof(UserControl).FullName, "ControlBase.js");
            Page.ClientScript.RegisterClientScriptInclude(typeof(SearchForm).FullName, "SearchForm.ascx.js");
        }

        public Lib.IRegionSearch GetSearchObject()
        {
            Lib.IRegionSearch search;

            switch (SearchType)
            {
                case Lib.SearchType.Footprint:
                    search = new Lib.FootprintSearch();
                    break;
                case Lib.SearchType.Region:
                    search = new Lib.FootprintRegionSearch();
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (selectedTab.Value)
            {
                case "keywordSearch":
                    search.SearchMethod = Lib.SearchMethod.Name;
                    search.Name = keyword.Text;
                    break;
                case "objectSearch":
                    search.SearchMethod = Lib.SearchMethod.Point;
                    search.Point = new Spherical.Cartesian(
                        (double)SharpAstroLib.Coords.Angle.ParseHmsOrDecimal(objectRa.Text),
                        (double)SharpAstroLib.Coords.Angle.ParseDmsOrDecimal(objectDec.Text));
                    break;
                case "pointSearch":
                    search.SearchMethod = Lib.SearchMethod.Point;
                    search.Point = new Spherical.Cartesian(
                        (double)SharpAstroLib.Coords.Angle.ParseHmsOrDecimal(pointRa.Text),
                        (double)SharpAstroLib.Coords.Angle.ParseDmsOrDecimal(pointDec.Text));
                    break;
                case "coneSearch":
                    if (coneIntersect.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Intersect;
                    }
                    else if (coneContain.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Contain;
                    }
                    else if (coneCover.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Cover;
                    }
                    search.Region = Spherical.Region.Parse("CIRCLE J2000 " + coneRa.Text + " " + coneDec + " " + coneRadius);
                    break;
                case "regionSearch":
                    if (regionIntersect.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Intersect;
                    }
                    else if (regionContain.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Contain;
                    }
                    else if (regionCover.Checked)
                    {
                        search.SearchMethod = Lib.SearchMethod.Cover;
                    }
                    search.Region = Spherical.Region.Parse(regionString.Text);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return search;

            /*var method = (Lib.SearchMethod)Enum.Parse(typeof(Lib.SearchMethod), SearchMethod.Value, true);
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

            e.ObjectInstance = search;*/
        }
    }
}