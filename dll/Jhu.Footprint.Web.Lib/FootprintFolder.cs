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
        private long footprintId;
        private string name;
        private string user;
        private FolderType type;

        private byte @public;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string comment;
        private Footprint folderFootprint;

        #endregion

        #region Properties
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public long FootprintId
        {
            get { return footprintId; }
            set { footprintId = value; }
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

        public Footprint FolderFootprint
        {
            get { return folderFootprint; }
            set { folderFootprint = value; }
        }

        #endregion

        #region Contsructors and Initializers
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
            this.footprintId = 0;
            this.name = "";
            this.user = "";
            this.type = FolderType.None;

            this.@public = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.comment = "";
            this.folderFootprint = null;
        }
        #endregion

        #region SQL - get commands
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
            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = (footprintId <= 0) ? DBNull.Value : (object)footprintId;
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
            this.footprintId = dr.IsDBNull(dr.GetOrdinal("FootprintId")) ? -1 : (long)dr["FootprintId"];
            this.name = (string)dr["Name"];
            this.user = (string)dr["User"];
            this.type = (FolderType)Enum.ToObject(typeof(FolderType), dr["Type"]);
            this.@public = (byte)dr["Public"];
            this.dateCreated = (DateTime)dr["DateCreated"];
            this.dateModified = (DateTime)dr["DateModified"];
            this.comment = (string)dr["Comment"];
        }

        protected override SqlCommand GetIsNameDuplicateCommand()
        {
            // TODO
            var sql = "fps.spIsDuplicateFootprintFolderName";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user;
            cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = this.id;
            cmd.Parameters.Add("@FolderName", SqlDbType.NVarChar, 256).Value = this.name;


            cmd.Parameters.Add("@Match", SqlDbType.Int).Direction = ParameterDirection.Output;

            return cmd;
        }

#endregion

        public override void Save()
        {
            Save(true);
        }

        public void Save(bool refreshFolderFootprint)
        {
            if (IsNameDuplicate())
            {
                throw Error.DuplicateFootprintFolderName(this.name);
            }

            if (this.id == 0)
            {
                Create();
            }
            else
            {
                Modify(refreshFolderFootprint);
            }
        }

        private void Create()
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

        private void Modify()
        {
            Modify(true);
        }

        private void Modify(bool refreshFolderFootprint)
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

            // false parameter prevents recursive updating!
            if (refreshFolderFootprint)
            {
                RefreshFolderFootprint();
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

        // TODO: move to Footprint search
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

        private void LoadFolderFootprint()
        {
            folderFootprint = new Footprint(Context)
            {
                Id = this.footprintId,
                User = this.User,
            };

            // if a folderFootprint exists, load it
            if (this.footprintId > 0)
            {
                folderFootprint.Load();
            }
        }

        private void InitializeFolderFootprint(Footprint f)
        {
            f.Type = FootprintType.Folder;
            f.Name = "folderFootprint";
            f.Comment = "Footprint of the folder.";
        }

        /// <summary>
        /// Updates region cache if a new region is linked to the RegionGroup
        /// </summary>
        public void UpdateFolderFootprint(Footprint newFootprint)
        {
            LoadFolderFootprint();

            if (folderFootprint.Region == null)
            {
                // If this is the first footprint in the folder
                this.footprintId = newFootprint.Id;
                Save(false);
                return;
            }

            if (folderFootprint.Type == FootprintType.None)
            {
                // We only had one region in the folder so far, now need to create
                // a new region to hold the intersection/union

                folderFootprint = new Footprint(folderFootprint);
                folderFootprint.Id = 0;
                InitializeFolderFootprint(folderFootprint);
            }

            switch (type)
            {
                case FolderType.Union:
                    folderFootprint.Region.SmartUnion(newFootprint.Region);
                    break;
                case FolderType.Intersection:
                    folderFootprint.Region.SmartIntersect(newFootprint.Region, false);
                    break;
            }

            folderFootprint.Save();
            footprintId = folderFootprint.Id;
            Save(false);
        }

        /// <summary>
        /// Refrehes the region cache completely after a RegionLink delete
        /// </summary>
        public void RefreshFolderFootprint()
        {
            LoadFolderFootprint();
            
            IEnumerable<Footprint> footprints = GetFootprintsByFolderId();

            // if less than 2 footprints are associated with this FootprintFolder,
            // FolderFootprint is not needed
            if (footprints.Count() <= 2)
            {
                // delete old folderFootprint if exists
                if (folderFootprint != null && folderFootprint.Type == FootprintType.Folder)
                {
                    folderFootprint.Delete();
                }

                // (folder)footprintID must be set in DB
                if (footprints.Count() == 1)
                {
                    this.FootprintId = footprints.ElementAt(0).Id;
                }
                else
                {
                    this.FootprintId = -1;
                }

                Save(false);

                return;
            }

            Spherical.Region region = new Spherical.Region();

            if (folderFootprint == null || folderFootprint.Type != FootprintType.Folder)
            {
                folderFootprint.Id = 0; // brand new folderFootprint 
            }

            // intersection must be started from an all-sky region
            if (this.type == FolderType.Intersection)
            {
                region.Add(new Spherical.Convex(new Spherical.Halfspace(0, 0, 1, false, -1)));
            }

            region.Simplify();
            
            // update the folderFootprint
            foreach (Footprint f in footprints)
            {
                switch (this.type)
                {
                    case FolderType.Union:
                        region.SmartUnion(f.Region);
                        break;
                    case FolderType.Intersection:
                        region = region.SmartIntersect(f.Region, false);
                        break;
                }
            }

            // save the new folderFootprint if required
            if (region != null)
            {

                InitializeFolderFootprint(folderFootprint);
                folderFootprint.User = user;
                folderFootprint.Public = @public;
                folderFootprint.FolderId = id;
                folderFootprint.FolderName = name;

                region.Simplify();
                folderFootprint.Region = region;

                folderFootprint.Save(); // save the new folderFootprint

                this.footprintId = folderFootprint.Id;
                Save(false);
            }

        }

    }
}
