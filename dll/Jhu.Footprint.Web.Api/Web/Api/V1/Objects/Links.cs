using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    public class Links
    {
        [DataMember(Name = "self", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Uri Self { get; set; }

        [DataMember(Name = "parent", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Uri Parent { get; set; }

        [DataMember(Name = "prev", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Uri Prev { get; set; }

        [DataMember(Name = "next", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Uri Next { get; set; }

        public Links()
        {
        }
    }
}
