using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Graywulf.AccessControl;
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
        private CombineMethod method;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string comments;

        private FootprintRegion combinedRegion;

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
        public int CombinedRegionId
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
        public CombineMethod Method
        {
            get { return method; }
            set { method = value; }
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
            get { return combinedRegion; }
            set { combinedRegion = value; }
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
            this.method = CombineMethod.None;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.comments = "";

            this.combinedRegion = null;
        }

        private void CopyMembers(Footprint old)
        {
            this.id = old.id;
            this.name = old.name;
            this.regionId = old.regionId;
            this.method = old.method;
            this.dateCreated = old.dateCreated;
            this.dateModified = old.dateModified;
            this.comments = old.comments;

            this.combinedRegion = new FootprintRegion(old.combinedRegion);
        }

        #endregion

        public void Load(string owner, string name)
        {
            this.Owner = owner;
            this.name = name;

            Load();
        }

        public void SetDefaultPermissions(bool @public)
        {
            if (Permissions.Owner == null)
            {
                Permissions.Owner = Context.Principal.Identity.Name;
            }

            if (Identity.Compare(Context.Principal.Identity.Name, Permissions.Owner) != 0)
            {
                // Footprint is created under a group account, set appropriate permissions
                Permissions.Grant(Owner, Lib.Constants.RoleAdmin, DefaultAccess.All);
                Permissions.Grant(Owner, Lib.Constants.RoleWriter, DefaultAccess.All);
                Permissions.Grant(Owner, Lib.Constants.RoleReader, DefaultAccess.Read);
            }

            if (@public)
            {
                // Footprint is publicly visible without registration (guest)
                Permissions.Grant(DefaultIdentity.Guest, DefaultAccess.Read);
            }
        }

        protected override void OnCreating(Graywulf.Entities.EntityEventArgs e)
        {
            // If footprint is created under an account different from the user's,
            // it's assumed to be under a group account so verify role

            if (Context.Principal == null || Context.Principal.Identity == null || !Context.Principal.Identity.IsAuthenticated)
            {
                throw Error.AccessDenied();
            }

            if (Identity.Compare(Context.Principal.Identity.Name, Owner) != 0)
            {
                var principal = Context.Principal as Principal;

                if (principal == null)
                {
                    throw Error.AccessDenied();
                }

                if (!principal.IsInRole(Owner, Constants.RoleAdmin) &&
                    !principal.IsInRole(Owner, Constants.RoleWriter))
                {
                    throw Error.AccessDenied();
                }
            }

            base.OnCreating(e);
        }

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
                throw Error.DuplicateFootprintName(this.name);
            }

            base.OnValidating(e);
        }

        protected override void OnModifying(Graywulf.Entities.EntityEventArgs e)
        {
            this.dateModified = DateTime.Now;
            
            base.OnModifying(e);
        }

        private void LoadFootprintRegion()
        {
            combinedRegion = new FootprintRegion(this)
            {
                Id = this.regionId,
            };

            // if a folderFootprint exists, load it
            if (this.regionId > 0)
            {
                combinedRegion.Load();
            }
        }

        private void InitializeFootprintRegion(FootprintRegion f)
        {
            f.Type = RegionType.Combined;
            f.Name = "footprintRegion";
        }

        /// <summary>
        /// Updates region cache if a new region is linked to the RegionGroup
        /// </summary>
        public void UpdateRegion(FootprintRegion newFootprint)
        {
            LoadFootprintRegion();

            if (combinedRegion.Region == null)
            {
                // If this is the first footprint in the folder
                this.regionId = newFootprint.Id;
                Save();
                return;
            }

            if (combinedRegion.Type == RegionType.Single)
            {
                // We only had one region in the folder so far, now need to create
                // a new region to hold the intersection/union

                combinedRegion = new FootprintRegion(combinedRegion);
                combinedRegion.Id = 0;
            }

            switch (method)
            {
                case CombineMethod.Union:
                    combinedRegion.Region.SmartUnion(newFootprint.Region);
                    break;
                case CombineMethod.Intersection:
                    combinedRegion.Region.SmartIntersect(newFootprint.Region, false);
                    break;
            }


            InitializeFootprintRegion(combinedRegion);
            combinedRegion.Save();
            regionId = combinedRegion.Id;
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
                if (combinedRegion != null && combinedRegion.Type == RegionType.Combined)
                {
                    combinedRegion.Delete();
                }

                // (folder)footprintID must be set in DB
                this.CombinedRegionId = footprints.Count() == 1 ? footprints.ElementAt(0).Id : -1;

                Save();
                return;
            }

            Spherical.Region r = new Spherical.Region();

            if (combinedRegion == null || combinedRegion.Type != RegionType.Combined)
            {
                combinedRegion.Id = 0; // brand new folderFootprint 
            }

            // intersection must be started from an all-sky region
            if (this.method == CombineMethod.Intersection)
            {
                r.Add(new Spherical.Convex(new Spherical.Halfspace(0, 0, 1, false, -1)));
                r.Simplify();
            }


            // update the folderFootprint
            foreach (FootprintRegion f in footprints)
            {
                switch (this.method)
                {
                    case CombineMethod.Union:
                        r.SmartUnion(f.Region);
                        break;
                    case CombineMethod.Intersection:
                        r = r.SmartIntersect(f.Region, false);
                        break;
                }
            }

            // save the new folderFootprint if required
            if (combinedRegion != null)
            {
                InitializeFootprintRegion(combinedRegion);

                r.Simplify();
                combinedRegion.Region = r;

                combinedRegion.Save(); // save the new folderFootprint

                this.regionId = combinedRegion.Id;
                Save();
            }

        }
    }
}
