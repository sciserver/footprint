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
    public class FootprintFolder : ContextObject
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

        public int Public
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

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = name;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
            cmd.Parameters.Add("@Type", SqlDbType.TinyInt).Value = type;
            cmd.Parameters.Add("@Public", SqlDbType.TinyInt).Value = @public;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1).Value = comment;

            cmd.Parameters.Add("@NewID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;

        }


        protected override SqlCommand GetModifyCommand()
        {
            string sql = "fps.spModifyFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderID", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = name;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
            cmd.Parameters.Add("@Type", SqlDbType.TinyInt).Value = type;
            cmd.Parameters.Add("@Public", SqlDbType.TinyInt).Value = @public;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1).Value = comment;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;
        }

        protected override SqlCommand GetDeleteCommand()
        {
            string sql = "fps.spDeleteFootprintFolder";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
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

    }
}
