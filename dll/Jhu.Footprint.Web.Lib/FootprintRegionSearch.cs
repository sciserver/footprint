using Jhu.Graywulf.Entities.Mapping;

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
        }
        #endregion
    }
}
