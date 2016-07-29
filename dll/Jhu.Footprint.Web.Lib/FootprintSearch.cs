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
    public class FootprintSearch: Jhu.Graywulf.Entities.SecurableEntitySearch<Footprint>
    {
        private string owner;
        private string name;
        private SearchMethod searchMethod;
        private Cartesian[] points;
        private double radius;
        private Region region;

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

        public Cartesian[] Point
        {
            get { return points; }
            set { points = value; }
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

        public FootprintSearch()
            : base()
        {
            InitializeMembers();
        }

        public FootprintSearch(Context context)
            : base(context) 
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.name = null;
            this.owner = null;
        }

        
    }
}
