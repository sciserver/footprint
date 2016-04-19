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
    public class FootprintSearch : Jhu.Graywulf.Entities.EntitySearch<Footprint>
    {
        #region Member variables
        private string user;
        private string footprintName;
        private string folderName;
        private long folderId;
        #endregion

        #region Properties
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string FootprintName
        {
            get { return footprintName; }
            set { footprintName = value; }
        }

        public string FolderName
        {
            get 
            {
                return folderName;
            }
            set { folderName = value; }
        }

        public long FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }
        #endregion

        #region Constructors & intitializers
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

        public void InitializeMembers()
        {
            this.user = "";
        }

        #endregion

        #region Methods
        public IEnumerable<Footprint> GetFootprintsByFolderId()
        {
            var res = new List<Footprint>();
            string sql = "fps.spGetFootprintsByFolderId";


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
                        var f = new Footprint((Context)Context);
                        f.LoadFromDataReader(dr);

                        res.Add(f);
                    }
                }

            }

            return res;
        }

        public long GetFootprintId()
        {
            var sql = "fps.spGetFootprintId";
            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user;
                cmd.Parameters.Add("@FolderName", SqlDbType.NVarChar, 256).Value = this.folderName;
                cmd.Parameters.Add("@FootprintName", SqlDbType.NVarChar, 256).Value = this.footprintName;
                cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@FootprintId"].Value == DBNull.Value)
                {
                    throw Error.CannotFindFootprint(this.user, this.folderName, this.footprintName);
                }

                return (long)cmd.Parameters["@FootprintId"].Value;
            }
        }
         #endregion


    }
}
