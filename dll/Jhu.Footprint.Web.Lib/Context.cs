using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Jhu.Footprint.Web.Lib
{
    public class Context : Jhu.Graywulf.Entities.Context, IDisposable
    {
        public Context()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["Jhu.Footprint"].ConnectionString;
        }
    }
}
