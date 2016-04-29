using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Jhu.Graywulf.Entities.Mapping;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintRegionSearch : Jhu.Graywulf.Entities.SecurableEntitySearch<FootprintRegion>
    {
        #region Member variables

        private string owner;
        private int? footprintId;
        private string footprintName;
        private string name;

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
    }
}
