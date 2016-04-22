using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Graywulf.Entities.AccessControl;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    [DbTable]
    public class Footprint : Jhu.Graywulf.Entities.SecurableEntity
    {
        #region Member variables

        private int id;
        private string name;
        private int regionId;
        private FootprintType type;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string comments;

        private FootprintRegion region;

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
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DbColumn]
        public int RegionId
        {
            get { return regionId; }
            set { regionId = value; }
        }

        [DbColumn]
        [XmlIgnore]
        public string Owner
        {
            get { return Permissions.Owner; }
            set { Permissions.Owner = value; }
        }

        [DbColumn]
        public FootprintType Type
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

        public FootprintRegion Region
        {
            get { return region; }
            set { region = value; }
        }

        #endregion
        #region Contsructors and Initializers

        public Footprint()
        {
            InitializeMembers();
        }

        public Footprint(Context context)
            : base(context)
        {
            InitializeMembers();
        }

        public Footprint(Footprint old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.regionId = 0;
            this.name = "";
            this.type = FootprintType.None;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.comments = "";

            this.region = null;
        }

        private void CopyMembers(Footprint old)
        {
            this.id = old.id;
            this.name = old.name;
            this.regionId = old.regionId;
            this.type = old.type;
            this.dateCreated = old.dateCreated;
            this.dateModified = old.dateModified;
            this.comments = old.comments;

            this.region = new FootprintRegion(old.region);
        }

        #endregion

        protected override SqlCommand GetSelectCommand()
        {
            if (id == 0 && Owner != null && Name != null)
            {
                var sql = @"
WITH __e AS
(
{0}
)
SELECT * 
FROM __e
WHERE Owner = @Owner AND Name = @Name;
";

                var cmd = new SqlCommand(
                String.Format(
                    sql,
                    GetTableQuery()));

                cmd.Parameters.Add("@Owner", SqlDbType.NVarChar).Value = Owner;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;

                return cmd;
            }
            else
            {
                return base.GetSelectCommand();
            }
        }
        
        protected Boolean IsNameDuplicate()
        {
            var sql = @"
SELECT COUNT(*) FROM [Footprint]
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

        protected override void OnSaving(Graywulf.Entities.EntityEventArgs e)
        {
            // Make sure user can save footprint with specified owner

            if (!Context.Identity.IsSame(this.Owner) &&
                !Context.Identity.IsInRole(this.Owner, Constants.GroupRoleAdmin))
            {
                throw Error.AccessDenied();
            }
            
            base.OnSaving(e);
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

        private void LoadFootprintRegion()
        {
            region = new FootprintRegion(this)
            {
                Id = this.regionId,
            };

            // if a folderFootprint exists, load it
            if (this.regionId > 0)
            {
                region.Load();
            }
        }

        private void InitializeFootprintRegion(FootprintRegion f)
        {
            f.Type = RegionType.Footprint;
            f.Name = "footprintRegion";
        }

        /// <summary>
        /// Updates region cache if a new region is linked to the RegionGroup
        /// </summary>
        public void UpdateRegion(FootprintRegion newFootprint)
        {
            LoadFootprintRegion();

            if (region.Region == null)
            {
                // If this is the first footprint in the folder
                this.regionId = newFootprint.Id;
                Save();
                return;
            }

            if (region.Type == RegionType.Region)
            {
                // We only had one region in the folder so far, now need to create
                // a new region to hold the intersection/union

                region = new FootprintRegion(region);
                region.Id = 0;
            }

            switch (type)
            {
                case FootprintType.Union:
                    region.Region.SmartUnion(newFootprint.Region);
                    break;
                case FootprintType.Intersection:
                    region.Region.SmartIntersect(newFootprint.Region, false);
                    break;
            }


            InitializeFootprintRegion(region);
            region.Save();
            regionId = region.Id;
            Save();
        }

        /// <summary>
        /// Refrehes the region cache completely after a region delete
        /// </summary>
        public void RefreshRegion()
        {
            LoadFootprintRegion();

            var search = new FootprintRegionSearch((Context)Context)
            { 
                FootprintId = this.id
            };
            var footprints = search.Find();

            // if less than 2 footprints are associated with this FootprintFolder,
            // FolderFootprint is not needed
            if (footprints.Count() <= 1)
            {
                // delete old folderFootprint if exists
                if (region != null && region.Type == RegionType.Footprint)
                {
                    region.Delete();
                }

                // (folder)footprintID must be set in DB
                this.RegionId = footprints.Count() == 1 ? footprints.ElementAt(0).Id : -1;

                Save();
                return;
            }

            Spherical.Region r = new Spherical.Region();

            if (region == null || region.Type != RegionType.Footprint)
            {
                region.Id = 0; // brand new folderFootprint 
            }

            // intersection must be started from an all-sky region
            if (this.type == FootprintType.Intersection)
            {
                r.Add(new Spherical.Convex(new Spherical.Halfspace(0, 0, 1, false, -1)));
                r.Simplify();
            }


            // update the folderFootprint
            foreach (FootprintRegion f in footprints)
            {
                switch (this.type)
                {
                    case FootprintType.Union:
                        r.SmartUnion(f.Region);
                        break;
                    case FootprintType.Intersection:
                        r = r.SmartIntersect(f.Region, false);
                        break;
                }
            }

            // save the new folderFootprint if required
            if (region != null)
            {
                InitializeFootprintRegion(region);

                r.Simplify();
                region.Region = r;

                region.Save(); // save the new folderFootprint

                this.regionId = region.Id;
                Save();
            }

        }

        public void SetPublic()
        {
        }

        public void SetPrivate()
        {
        }
    }
}
