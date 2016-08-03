using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintRegionSearch : Jhu.Graywulf.Entities.SecurableEntitySearch<FootprintRegion>
    {
        #region Member variables

        private string owner;
        private int? footprintId;
        private string footprintName;
        private string name;
        private SearchMethod searchMethod;
        private Cartesian points;
        private double radius;
        private Region region;

        #endregion
        #region Properties

        [DbColumn]
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [DbColumn]
        public int? FootprintId
        {
            get { return footprintId; }
            set { footprintId = value; }
        }

        public string FootprintName
        {
            get { return footprintName; }
            set { footprintName = value; }
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
            get { return points; }
            set { points = value; }
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

        #endregion

        #region Constructors & intitializers

        public FootprintRegionSearch()
            : base()
        {
            InitializeMembers();
        }

        public FootprintRegionSearch(Context context)
            : base(context)
        {
            InitializeMembers();
        }

        public void InitializeMembers()
        {
            this.owner = null;
            this.footprintId = null;
            this.footprintName = null;
            this.name = null;
        }

        #endregion

        protected override string GetTableQuery()
        {
            switch (searchMethod)
            {
                case SearchMethod.Point:
                    return GetTableQuery_PointSearch();
                case SearchMethod.Cone:
                case SearchMethod.Intersect:
                case SearchMethod.Contain:
                    throw new NotImplementedException();
                default:
                    return base.GetTableQuery();
            }
        }

        private string GetTableQuery_PointSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type], [__acl]
FROM [dbo].[FootprintRegion] r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID
INNER JOIN [dbo].[FindFootprintRegionEq](@ra, @dec) ff
	ON r.ID = ff.RegionID";
        }

        protected override void AppendParameters()
        {
            switch (searchMethod)
            {
                case SearchMethod.Point:
                    AppendParameters_PointSearch();
                    break;
                default:
                    base.AppendParameters();
                    break;
            }
        }

        private void AppendParameters_PointSearch()
        {
            AppendSearchParameter("@ra",SqlDbType.Float,Point.RA);
            AppendSearchParameter("@dec", SqlDbType.Float, Point.Dec);
        }
    }
}
