
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
            throw new NotImplementedException();
        }

        protected override string GetTableQuery_IntersectSearch()
        {
            throw new NotImplementedException();
        }

        protected override string GetTableQuery_ContainSearch()
        {
            throw new NotImplementedException();
        }
    }
}
