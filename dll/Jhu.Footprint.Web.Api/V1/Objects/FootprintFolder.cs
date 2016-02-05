﻿using System;
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

        [DataMember(Name = "url")]
        [Description("Folder url.")]
        public string Url { get; set; }

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
            this.Name = folder.Name;
            this.User = folder.User;
            this.Id = folder.Id;
            this.Public = folder.Public;
            this.Type = folder.Type;
            //this.Url = System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.OriginalString;
            this.Comment = folder.Comment;
        }

        public Jhu.Footprint.Web.Lib.FootprintFolder GetValue()
        {
            var folder = new Jhu.Footprint.Web.Lib.FootprintFolder();

            folder.Name = this.Name;
            folder.User = this.User;
            folder.Id = this.Id;
            folder.Public = this.Public;
            folder.Type = this.Type;
            folder.Comment = this.Comment;

            return folder;
        }
    }
}
