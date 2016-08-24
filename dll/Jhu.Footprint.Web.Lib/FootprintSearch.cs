using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Spherical;


namespace Jhu.Footprint.Web.Lib
{
    public class FootprintSearch : RegionSearch<Footprint>
    {
#if false
        private string owner;
        private string name;
        private SearchMethod searchMethod;
        private Cartesian point;
        private double radius;
        private Region region;

        [DbColumn]
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [DbColumn]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SearchMethod SearchMethod
        {
            get { return searchMethod; }
            set { searchMethod = value; }
        }

        public Cartesian Point
        {
            get { return point; }
            set { point = value; }
        }

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }
#endif

        #region Constructors & initializers
        public FootprintSearch()
            : base()
        {
        }

        public FootprintSearch(Context context)
            : base(context)
        {
        }

        #endregion

        protected override void AppendSearchCriteria()
        {
            base.AppendSearchCriteria();
            AppendSearchCriterion("WHERE r.ID = f.CombinedRegionID");
        }

    }
}
