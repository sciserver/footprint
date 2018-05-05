using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Entities;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("A footprint is a collection of regions representing the sky coverage of observations.")]
    public class Footprint
    {
        [DataMember(Name = "owner", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Name of the folder containing the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "combinationMethod", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Method to combine regions: none, union or intersection.")]
        public Lib.CombinationMethod? CombinationMethod { get; set; }
                
        [DataMember(Name = "comments", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Comments.")]
        public string Comments { get; set; }
        
        [DataMember(Name = "public", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Visibility of the footprint to the public.")]
        public bool? Public { get; set; }

        public Footprint()
        {
        }

        public Footprint(Lib.Footprint footprint)
        {
            SetValues(footprint);
        }

        public void GetValues(Lib.Footprint footprint)
        {
            footprint.Name = this.Name ?? footprint.Name;
            footprint.CombinationMethod = this.CombinationMethod ?? footprint.CombinationMethod;
            footprint.Comments = this.Comments ?? footprint.Comments;
            
            // To prevent resetting permission when modifying a footprint,
            // only set permission when the public field is present
            // in the request
            if (footprint.Context != null && (!footprint.IsExisting || Public.HasValue))
            {
                footprint.SetDefaultPermissions(Public ?? false);
            }
        }

        public void SetValues(Lib.Footprint footprint)
        {
            var access = footprint.Permissions.EvaluateAccess(Principal.Guest);

            this.Owner = footprint.Owner;
            this.Name = footprint.Name;
            this.CombinationMethod = footprint.CombinationMethod;
            this.Comments = footprint.Comments;
            this.Public = access.CanRead();
        }
    }
}
