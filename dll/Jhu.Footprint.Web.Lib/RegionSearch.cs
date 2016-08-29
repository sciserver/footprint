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
    public class RegionSearch<T> : Graywulf.Entities.SecurableEntitySearch<T>, IRegionSearch
        where T : Jhu.Graywulf.Entities.SecurableEntity, new()
    {
        protected string owner;
        protected string name;
        protected SearchMethod searchMethod;
        protected Cartesian point;
        protected double radius;
        protected Region region;

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

        #region Constructors & initializers
        public RegionSearch()
            : base()
        {
        }

        public RegionSearch(Context context)
                : base(context)
        {
            InitializeMembers();
        }
        #endregion
        private void InitializeMembers()
        {

            name = null;
            owner = null;
            searchMethod = SearchMethod.Name;
            point = new Cartesian();
            radius = 0;
            region = null;
        }

        protected override string GetTableQuery()
        {
            switch (searchMethod)
            {

                case SearchMethod.Point:
                    return GetTableQuery_PointSearch();
                case SearchMethod.Cone:
                case SearchMethod.Intersect:
                    return GetTableQuery_IntersectSearch();
                case SearchMethod.Contain:
                    return GetTableQuery_ContainSearch();
                default:
                    return base.GetTableQuery();
            }
        }

        private string GetTableQuery_PointSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type], f.CombinedRegionID, [__acl]
FROM [dbo].[FootprintRegion] r
INNER JOIN [fps].[FindFootprintRegionEq](@ra, @dec) ff
	ON r.ID = ff.RegionID
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        private string GetTableQuery_IntersectSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type],  f.CombinedRegionID, [__acl]
FROM [fps].[FindFootprintRegionIntersect](@region) r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        private string GetTableQuery_ContainSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type],  f.CombinedRegionID, [__acl]
FROM [fps].[FindFootprintRegionContain](@region) r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        protected override void AppendParameters()
        {
            switch (searchMethod)
            {
                case SearchMethod.Point:
                    AppendParameters_PointSearch();
                    break;
                case SearchMethod.Cone:
                    AppendParameters_ConeSearch();
                    break;
                case SearchMethod.Contain:
                case SearchMethod.Intersect:
                    AppendParameters_IntersectSearch();
                    break;
                default:
                    base.AppendParameters();
                    break;
            }
        }

        private void AppendParameters_PointSearch()
        {
            AppendSearchParameter("@ra", SqlDbType.Float, Point.RA);
            AppendSearchParameter("@dec", SqlDbType.Float, Point.Dec);
        }

        private void AppendParameters_ConeSearch()
        {
            var sb = new ShapeBuilder();
            var circle = sb.CreateCircle(Point, radius);
            var region = new Region(circle, false);
            AppendSearchParameter("@region", SqlDbType.VarBinary, region.ToSqlBytes().Value);
        }
        private void AppendParameters_IntersectSearch()
        {
            AppendSearchParameter("@region", SqlDbType.VarBinary, Region.ToSqlBytes().Value);
        }


    }
}


