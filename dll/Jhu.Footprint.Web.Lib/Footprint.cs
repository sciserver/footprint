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
        private CombinationMethod combinationMethod;
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
        [DbKey]
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
        public CombinationMethod CombinationMethod
        {
            get { return combinationMethod; }
            set { combinationMethod = value; }
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

        public FootprintRegion CombinedRegion
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
            this.combinationMethod = CombinationMethod.None;
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
            this.combinationMethod = old.combinationMethod;
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

        public bool CheckExists(string owner, string name)
        {
            this.Owner = owner;
            this.name = name;

            return CheckExists();
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

        protected override SqlCommand GetCheckExistsCommand()
        {
            if (id == 0 && Owner != null && Name != null)
            {
                var sql = @"
WITH __e AS
(
{0}
)
SELECT ISNULL(COUNT(*), 0)
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
                return base.GetCheckExistsCommand();
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
        public void UpdateCombinedRegion(FootprintRegion region)
        {
            if (combinationMethod != CombinationMethod.None)
            {
                using (var cmd = Context.CreateStoredProcedureCommand("fps.spUpdateCombinedRegion"))
                {
                    cmd.Parameters.Add("@FootprintID", SqlDbType.BigInt).Value = this.Id;
                    cmd.Parameters.Add("@RegionID", SqlDbType.BigInt).Value = region.Id;

                    Context.ExecuteCommandNonQuery(cmd);
                }
            }
        }

        /// <summary>
        /// Refrehes the region cache completely after a region delete
        /// </summary>
        public void RefreshCombinedRegion()
        {
            if (combinationMethod != CombinationMethod.None)
            {
                using (var cmd = Context.CreateStoredProcedureCommand("fps.spRefreshCombinedRegion"))
                {
                    cmd.Parameters.Add("@FootprintID", SqlDbType.BigInt).Value = this.Id;

                    Context.ExecuteCommandNonQuery(cmd);
                }
            }
        }
    }
}
