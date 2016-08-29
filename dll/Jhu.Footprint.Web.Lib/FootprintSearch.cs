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
