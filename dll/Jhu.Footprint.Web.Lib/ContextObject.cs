using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public abstract class ContextObject
    {
        private Context context;

        protected abstract SqlCommand GetCreateCommand();

        protected abstract SqlCommand GetModifyCommand();

        protected abstract SqlCommand GetDeleteCommand();

        protected abstract SqlCommand GetLoadCommand();

        public abstract void LoadFromDataReader(SqlDataReader dr);

        public void Load()
        {
            using (var cmd = GetLoadCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                using (var dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    dr.Read();
                    LoadFromDataReader(dr);
                }
            }
        }

        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        public ContextObject(Context context)
        { 
            this.context = context;
        }
    }
}
