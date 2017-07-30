
using System;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintSearch : RegionSearch<Footprint>
    {
        
        #region Constructors & initializers
        public FootprintSearch()
            : base()
        {
        }

        public FootprintSearch(FootprintContext context)
            : base(context)
        {
        }

        #endregion

        protected override string GetTableQuery_PointSearch()
        {
            return @"
SELECT  f.ID, f.[Owner], f.[Name], f.CombinedRegionID, f.CombinationMethod, f.DateCreated, f.DateModified, f.Comments, [__acl]
FROM [dbo].[Footprint] f
INNER JOIN [fps].[FindFootprintRegionEq](@ra, @dec) ff
	ON ff.RegionID = f.CombinedRegionID";
        }

        protected override string GetTableQuery_IntersectSearch()
        {
            throw new NotImplementedException();
        }

        protected override string GetTableQuery_ContainSearch()
        {
            throw new NotImplementedException();
        }

        protected override string GetTableQuery_CoverSearch()
        {
            throw new NotImplementedException();
        }
    }
}
