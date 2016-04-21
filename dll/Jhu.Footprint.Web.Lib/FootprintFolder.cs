using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    [DbTable]
    public class FootprintFolder : Jhu.Graywulf.Entities.SecurableEntity
    {
        #region Member variables

        private int id;
        private int footprintId;
        private string name;
        private FolderType type;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string comments;
        private Footprint folderFootprint;

        #endregion

        #region Properties

        /// <summary>
        /// Returns or sets the database ID of the region. Set this before loading or
        /// modifying. If set to 0, Save() will create a new record in the database.
        /// </summary>
        [DbColumn(Binding = DbColumnBinding.Key)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DbColumn]
        public int FootprintId
        {
            get { return footprintId; }
            set { footprintId = value; }
        }

        [DbColumn]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DbColumn]
        [XmlIgnore]
        public string Owner
        {
            get { return Permissions.Owner; }
            set { Permissions.Owner = value; }
        }

        [DbColumn]
        public FolderType Type
        {
            get { return type; }
            set { type = value; }
        }

        [DbColumn]
        public DateTime DateCreated
        {
            get { return dateCreated; }
            protected set { dateCreated = value; }
        }

        [DbColumn]
        public DateTime DateModified
        {
            get { return dateModified; }
            protected set { dateCreated = value; }
        }

        [DbColumn]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
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

        public FootprintFolder(FootprintFolder old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.name = "";
            this.footprintId = 0;
            this.type = FolderType.None;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.comments = "";
            this.folderFootprint = null;
        }

        private void CopyMembers(FootprintFolder old)
        {
            this.id = old.id;
            this.name = old.name;
            this.footprintId = old.footprintId;
            this.type = old.type;
            this.dateCreated = old.dateCreated;
            this.dateModified = old.dateModified;
            this.comments = old.comments;
            this.folderFootprint = new Footprint(old.folderFootprint);
        }

        #endregion
        
        protected Boolean IsNameDuplicate()
        {
            var sql = @"
SELECT COUNT(*) FROM [FootprintFolder]
WHERE Owner = @Owner
      AND ID != @ID
      AND Name = @Name";

            using (var cmd = Context.CreateCommand(sql))
            {
                cmd.Parameters.Add("@Owner", SqlDbType.NVarChar, 250).Value = this.Owner;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = this.Id;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = this.Name;

                return (int)Context.ExecuteCommandScalar(cmd) > 0;
            }
        }

        protected override void OnValidating(Graywulf.Entities.EntityEventArgs e)
        {
            if (Constants.RestictedNames.Contains(this.name))
            {
                throw Error.FootprintNameNotAvailable(this.name);
            }

            if (!Constants.NamePattern.Match(this.name).Success)
            {
                throw Error.FootprintNameInvalid(this.name);
            }

            if (IsNameDuplicate())
            {
                throw Error.DuplicateFootprintFolderName(this.name);
            }

            base.OnValidating(e);
        }

        protected override void OnModifying(Graywulf.Entities.EntityEventArgs e)
        {
            this.dateModified = DateTime.Now;
            
            base.OnModifying(e);
        }

        /*
        private void GetFootprintFolderId()
        {
            // TODO: move to search class

            var sql = "fps.spGetFootprintFolderId";
            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
                cmd.Parameters.Add("@FolderName", SqlDbType.NVarChar, 256).Value = Name;
                cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@FolderId"].Value == DBNull.Value)
                {
                    throw Error.CannotFindfootprintFolder(this.user, this.Name);
                }

                this.Id = (long)cmd.Parameters["@FolderId"].Value;
            }
        }
        */

        private void LoadFolderFootprint()
        {
            folderFootprint = new Footprint((Context)Context)
            {
                Id = this.footprintId,
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
                Save();
                return;
            }

            if (folderFootprint.Type == FootprintType.None)
            {
                // We only had one region in the folder so far, now need to create
                // a new region to hold the intersection/union

                folderFootprint = new Footprint(folderFootprint);
                folderFootprint.Id = 0;
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


            InitializeFolderFootprint(folderFootprint);
            folderFootprint.Save();
            footprintId = folderFootprint.Id;
            Save();
        }

        /// <summary>
        /// Refrehes the region cache completely after a footprint delete
        /// </summary>
        public void RefreshFolderFootprint()
        {
            LoadFolderFootprint();

            var search = new FootprintSearch((Context)Context) { User = this.Owner, FolderId = this.id };
            IEnumerable<Footprint> footprints = search.GetFootprintsByFolderId();

            // if less than 2 footprints are associated with this FootprintFolder,
            // FolderFootprint is not needed
            if (footprints.Count() <= 1)
            {
                // delete old folderFootprint if exists
                if (folderFootprint != null && folderFootprint.Type == FootprintType.Folder)
                {
                    folderFootprint.Delete();
                }

                // (folder)footprintID must be set in DB
                this.FootprintId = footprints.Count() == 1 ? footprints.ElementAt(0).Id : -1;

                Save();
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
                region.Simplify();
            }


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
                folderFootprint.FolderId = id;

                region.Simplify();
                folderFootprint.Region = region;

                folderFootprint.Save(); // save the new folderFootprint

                this.footprintId = folderFootprint.Id;
                Save();
            }

        }
    }
}
