using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Spherical;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintSearch : Search
    {
        private FootprintSearchMethod searchMethod;
        private long folderId;
        private string user;
        private string name;

        public FootprintSearchMethod SearchMethod
        {
            get { return searchMethod; }
            set { searchMethod = value; }
        }

        public long FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public FootprintSearch()
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
            this.name = "";
            this.user = "";
        }

        override public int Count()
        {
            switch (SearchMethod)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Footprint> Find()
        {
            switch (SearchMethod)
            {
                case FootprintSearchMethod.FolderId:
                    return FindByFolderId();

                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<Footprint> FindByFolderId()
        {
            var res = new List<Footprint>();
            string sql = "fps.spFindFootprintsByFolderId";


            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = this.folderId;
                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var f = new Footprint(Context);
                        f.LoadFromDataReader(dr);

                        res.Add(f);
                    }
                }

            }

            return res;
        }


    }
}
