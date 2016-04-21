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
    public class FootprintRegionSearch : Jhu.Graywulf.Entities.EntitySearch<FootprintRegion>
    {
        #region Member variables

        private string owner;
        private int? folderId;
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
        public int? FolderId
        {
            get { return folderId; }
            set { folderId = value; }
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
            this.folderId = null;
            this.footprintName = null;
            this.name = null;
        }

        #endregion
    }
}
