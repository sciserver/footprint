using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprintFolderList")]
    [Description("Represents a list of footprint folder queues.")]
    public class FootprintListResponse
    {
        [DataMember(Name = "footprintFolders")]
        [Description("List of footprint folders.")]
        public Footprint[] FootprintFolders { get; set; }

        public FootprintListResponse()
        {
        }

        public FootprintListResponse(IEnumerable<Lib.FootprintFolder> folders)
        {
            this.FootprintFolders = folders.Select(f => new Footprint(f)).ToArray();
        }
    }
}
