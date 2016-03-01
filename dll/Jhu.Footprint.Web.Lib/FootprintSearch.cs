using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintSearch : ContextObject
    {
        #region Member variables
        private string user;
        #endregion

        #region Properties
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        #endregion

        #region Constructors & intitializers
        public FootprintSearch(Context context)
            : base(context)
        {
            InitializeMembers();
        }

        public void InitializeMembers()
        {
            this.user = "";
        }

        #endregion

        #region Methods
        public IEnumerable<Footprint> GetFootprintsByFolderId(long folderId)
        {
            var res = new List<Footprint>();
            string sql = "fps.spGetFootprintsByFolderId";


            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = folderId;
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
        #endregion
    }
}
