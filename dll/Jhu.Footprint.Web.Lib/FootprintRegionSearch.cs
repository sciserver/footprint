﻿using System;
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
    public class FootprintRegionSearch : Lib.RegionSearch<FootprintRegion>
    {
        #region Member variables

        private int? footprintId;
        private string footprintName;


        #endregion
        #region Properties

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
        
        #endregion

        #region Constructors & intitializers

        public FootprintRegionSearch()
            : base()
        {
            //InitializeMembers();
        }

        public FootprintRegionSearch(Context context)
            : base(context)
        {
            //InitializeMembers();
        }
        
        /*
        public void InitializeMembers()
        {
            owner = null;
            footprintId = null;
            footprintName = null;
            name = null;
            searchMethod = SearchMethod.Name;
            point = new Cartesian();
            radius = 0;
            region = null;
        }
        */
        #endregion
        /*
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
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type], [__acl]
FROM [dbo].[FootprintRegion] r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID
INNER JOIN [fps].[FindFootprintRegionEq](@ra, @dec) ff
	ON r.ID = ff.RegionID";
        }

        private string GetTableQuery_IntersectSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type], [__acl]
FROM [fps].[FindFootprintRegionIntersect](@region) r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        private string GetTableQuery_ContainSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type], [__acl]
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

    */
    }
}
