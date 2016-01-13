using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public class Footprint
    {
        #region Member variables

        private long id;
        private string name;
        private Guid userGuid;               // user database guid
        private byte @public;                         // public flag, >0 when visible to the public
        private DateTime dateCreated;
        private long customId;
        private Region region;
        private double fillFactor;
        private string type;
        private byte mask;
        private GroupType groupType;
        private string comment;
        private string regionString;
        #endregion

        #region Properties
        /// <summary>
        /// Returns or sets the database ID of the region. Set this before loading or
        /// modifying. If set to 0, Save() will create a new record in the database.
        /// </summary>
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

        /// <summary>
        /// Returns or sets the user ID. Set this before doing any database operation
        /// since it determines the security context for the stored procedures.
        /// </summary>
        
        [XmlIgnore]
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        /// <summary>
        /// Returns or sets the public flag of the region. Set
        /// </summary>
        public byte Public
        {
            get { return @public; }
            set { @public = value; }
        }

        public DateTime DateCreated
        {
            get { return dateCreated; }
            private set { dateCreated = value; }
        }

        public long CustomId
        {
            get { return customId; }
            set { customId = value; }
        }


        public Region Region
        {
            get { return region; }
            set { region = value;}
        }

        public double FillFactor
        {
            get { return fillFactor; }
            set { fillFactor = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public byte Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public GroupType GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [XmlIgnore]
        public string RegionString
        {
            get { return regionString; }
            set { regionString = value; }
        }
        #endregion

        public Footprint()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            Id = 0;
            Name = "";
            UserGuid = Guid.Empty;               // user database guid
            Public = 0;                         // public flag, >0 when visible to the public
            DateCreated = DateTime.Now;
            CustomId = 0;
            Region = null;
            FillFactor = 1.0;
            Type = "";
            Mask = 0;
            GroupType = GroupType.Unknown;
            Comment = "";
            RegionString = "";
        }
        
    }
}
