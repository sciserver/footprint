
namespace Jhu.Footprint.Web.Lib
{
    public class FootprintSearch : RegionSearch<Footprint>
    {
        
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
            AppendSearchCriterion("ID = CombinedRegionID");
        }

    }
}
