using Jhu.Graywulf.Entities.Mapping;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintRegionSearch : Lib.RegionSearch<FootprintRegion>
    {
        #region Member variables

        private int? footprintId;
        private string footprintName;
        private SearchType searchType;

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

        public SearchType SearchType
        {
            get { return searchType; }
            set { searchType = value; }
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
            footprintId = null;
            footprintName = null;
            searchType = SearchType.Region;
        }
        #endregion


        protected override void AppendSearchCriteria()
        {
            switch (searchType)
            {
                case SearchType.Footprint:
                    base.AppendSearchCriteria();
                    AppendSearchCriterion("ID = CombinedRegionID");
                    break;
                case SearchType.Region:
                default:
                    base.AppendSearchCriteria();
                    break;

            }
        }

        protected override string GetTableQuery_PointSearch()
        {
            return @"
SELECT  r.ID, [FootprintID], r.[Name], [FillFactor], [Type], f.[Name] AS FootprintName, f.owner,  f.CombinedRegionID, [__acl]
FROM [dbo].[FootprintRegion] r
INNER JOIN [fps].[FindFootprintRegionEq](@ra, @dec) ff
	ON r.ID = ff.RegionID
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        protected override string GetTableQuery_IntersectSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type],  f.[Name] AS FootprintName, f.owner,  f.CombinedRegionID, [__acl]
FROM [fps].[FindFootprintRegionIntersect](@region) r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }

        protected override string GetTableQuery_ContainSearch()
        {
            return @"
SELECT r.ID, [FootprintID], r.[Name], [FillFactor], [Type],  f.[Name] AS FootprintName, f.owner, f.CombinedRegionID, [__acl]
FROM [fps].[FindFootprintRegionContain](@region) r
INNER JOIN [dbo].[Footprint] f 
    ON r.footprintID = f.ID";
        }
    }
}
