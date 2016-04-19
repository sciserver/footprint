using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprintfolder")]
    [Description("Folder that is able to contain footprint(s).")]
    public class FootprintFolder
    {
        [DataMember(Name = "id")]
        [Description("Folder Id.")]
        public long Id { get; set; }


        [DataMember(Name = "footprintId")]
        [Description("Id of the folder footprint.")]
        public long FootprintId { get; set; }

        [DataMember(Name = "url")]
        [Description("Folder url.")]
        public Uri Url { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the folder containing the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "user")]
        [Description("User of the folder.")]
        public string User { get; set; }

        [DataMember(Name = "public")]
        [Description("Publicity of folder. 0 - not public, 1 - public.")]
        public byte Public { get; set; }

        [IgnoreDataMember]
        public Jhu.Footprint.Web.Lib.FolderType Type { get; set; }

        [DataMember(Name = "type")]
        [Description("Type of the folder. Could be: Any, Unknown, Union, Intersection, None.")]
        public string Type_ForXml
        {
            get { return Util.EnumFormatter.ToXmlString(Type); }
            set { Type = Util.EnumFormatter.FromXmlString<Lib.FolderType>(value); }

        }

        [DataMember(Name = "comment")]
        [Description("Comment.")]
        public string Comment { get; set; }

                public FootprintFolder()
        {
        }

        public FootprintFolder(Jhu.Footprint.Web.Lib.FootprintFolder folder)
        {
            SetValue(folder);
        }

        public void SetValue(Jhu.Footprint.Web.Lib.FootprintFolder folder)
        {
            this.Id = folder.Id;
            this.FootprintId = folder.FootprintId;
            this.Name = folder.Name;
            this.User = folder.Owner;
            this.Public = folder.Public;
            this.Type = folder.Type;
            //TODO : host name?
            this.Url = new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + this.User + "/" + this.Name);
            this.Comment = folder.Comments;
        }

        public Jhu.Footprint.Web.Lib.FootprintFolder GetValue()
        {
            var folder = new Jhu.Footprint.Web.Lib.FootprintFolder();

            folder.Id = this.Id;
            folder.FootprintId = this.FootprintId;
            folder.Name = this.Name;
            folder.Owner = this.User;
            folder.Public = this.Public;
            folder.Type = this.Type;
            folder.Comments = this.Comment;

            return folder;
        }
    }
}
