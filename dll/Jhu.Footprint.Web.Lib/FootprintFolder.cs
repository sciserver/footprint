using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Jhu.Spherical;
using System.Xml.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintFolder : Entity
    {
        #region Member variables

        private long id;
        private string name;
        private string user;
        private FolderType type;

        private byte @public;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string comment;

        #endregion

        #region Properties
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public FolderType Type
        {
            get { return type; }
            set { type = value; }
        }

        [XmlIgnore]
        public string User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }

        public byte Public
        {
            get { return @public; }
            set
            {
                @public = (byte)value;
            }
        }

        public DateTime DateCreated
        {
            get { return dateCreated; }
        }

        public DateTime DateModified
        {
            get { return dateModified; }
        }


        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        #endregion

        public FootprintFolder()
        {
            InitializeMembers();
        }
        public FootprintFolder(Context context)
            : base(context)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.name = "";
            this.user = "";
            this.type = FolderType.Unknown;

            this.@public = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.comment = "";

        }

        protected override SqlCommand GetCreateCommand()
        {
            string sql = "fps.spCreateFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AppendCreateModifyParameters(cmd);

            cmd.Parameters.Add("@NewID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;

        }


        protected override SqlCommand GetModifyCommand()
        {
            if (this.id == 0) { GetFootprintFolderId(); } 

            string sql = "fps.spModifyFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderID", SqlDbType.BigInt).Value = id;
            AppendCreateModifyParameters(cmd);

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;
        }

        protected override SqlCommand GetDeleteCommand()
        {
            if (this.id == 0) { GetFootprintFolderId(); } 

            string sql = "fps.spDeleteFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        protected override SqlCommand GetLoadCommand()
        {
            if (this.id == 0) { GetFootprintFolderId(); } 

            string sql = "fps.spGetFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@user", SqlDbType.NVarChar, 250).Value = user;

            return cmd;
        }

        private void AppendCreateModifyParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = name;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
            cmd.Parameters.Add("@Public", SqlDbType.TinyInt).Value = @public;
            cmd.Parameters.Add("@Type", SqlDbType.TinyInt).Value = Type;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1).Value = comment;
        }

        public override void LoadFromDataReader(SqlDataReader dr)
        {
            this.id = (long)dr["FolderId"];
            this.name = (string)dr["Name"];
            this.user = (string)dr["User"];
            this.type = (FolderType)Enum.ToObject(typeof(FolderType),dr["Type"]);
            this.@public = (byte)dr["Public"];
            this.dateCreated = (DateTime)dr["DateCreated"];
            this.dateModified = (DateTime)dr["DateModified"];
            this.comment = (string)dr["Comment"];
        }

        public void Create()
        {
            using (var cmd = GetCreateCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot create FootprintFolder.");
                }
                else
                {
                    this.id = (long)cmd.Parameters["@NewID"].Value;
                }
            }

        }

        public void Modify()
        {
            using (var cmd = GetModifyCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot update FootprintFolder.");
                }

            }
        }

        public void Delete()
        {
            using (var cmd = GetDeleteCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot delete FootprintFolder.");
                }
            }
        }

        private void GetFootprintFolderId()
        {
            var sql = "fps.spGetFootprintFolderId";
            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
                cmd.Parameters.Add("@FolderName", SqlDbType.NVarChar, 256).Value = name;
                cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                this.id = (long)cmd.Parameters["@FolderId"].Value;
            }
        }

        public IEnumerable<Footprint> GetFootprintsByFolderId()
        {
            var res = new List<Footprint>();
            string sql = "fps.spGetFootprintsByFolderId";


            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = this.id;
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
