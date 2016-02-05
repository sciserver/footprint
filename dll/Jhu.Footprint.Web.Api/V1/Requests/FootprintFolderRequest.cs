using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Represents a footprint folder.")]
    public class FootprintFolderRequest
    {
        [DataMember(Name = "footprintFolder", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Conveys a footprint folder.")]
        public FootprintFolder FootprintFolder { get; set; }

        public FootprintFolderRequest()
        {
        }
    }
}
