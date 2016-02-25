using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public abstract class Entity: ContextObject
    {
        protected Entity()
        {
        }

        protected Entity(Context context)
            : base(context)
        {
        }

        protected abstract SqlCommand GetCreateCommand();

        protected abstract SqlCommand GetModifyCommand();

        protected abstract SqlCommand GetDeleteCommand();

        protected abstract SqlCommand GetLoadCommand();

        protected abstract SqlCommand GetNameIsAvailableCommand();
        
        public abstract void LoadFromDataReader(SqlDataReader dr);

        public abstract void Save();

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

        protected Boolean NameIsAvailable()
        {
            using (var cmd = GetNameIsAvailableCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int match = (int)cmd.Parameters["@Match"].Value;

                if (match > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
}
