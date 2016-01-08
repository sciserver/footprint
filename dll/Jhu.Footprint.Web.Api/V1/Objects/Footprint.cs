using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("TODO")]
    public class Footprint
    {
        [DataMember(Name = "name")]
        [Description("Name of the footprint.")]
        public string Name { get; set; }

        public Footprint()
        {
        }
    }
}
