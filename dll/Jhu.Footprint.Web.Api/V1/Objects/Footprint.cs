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
    [DataContract(Name = "footprint")]
    [Description("TODO")]
    public class Footprint
    {

        [DataMember(Name = "id")]
        [Description("Footprint Id.")]
        public long Id { get; set; }

        [DataMember(Name = "url")]
        [Description("Footprint url.")]
        public Uri Url { get; set; }

        [DataMember(Name = "folderId")]
        [Description("Id of the folder containing the footprint.")]
        public long FolderId { get; set; }

        [DataMember(Name = "folderName")]
        [Description("Name of the folder containing the footprint.")]
        public string FolderName { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "user")]
        [Description("User of the footprint.")]
        public string User { get; set; }

        [DataMember(Name = "public")]
        [Description("Publicity of footprint. 0 - not public, 1 - public.")]
        public byte Public { get; set; }

        [DataMember(Name = "fillFactor")]
        [Description("TODO")]
        public double FillFactor { get; set; }

        [IgnoreDataMember]
        public Jhu.Footprint.Web.Lib.FootprintType FootprintType { get; set; }

        [DataMember(Name = "folderType")]
        [Description("Type of the folder containing the footprint. Could be: Any, Unknown, Union, Intersection, None.")]
        public string Type_ForXml
        {
            get { return Util.EnumFormatter.ToXmlString(FootprintType); }
            set { FootprintType = Util.EnumFormatter.FromXmlString<Lib.FootprintType>(value); }

        }


        [DataMember(Name = "comment")]
        [Description("Comment.")]
        public string Comment { get; set; }

        [DataMember(Name = "regionString")]
        [Description("Region string.")]
        public string RegionString { get; set; }

        public Footprint()
        {
        }

        public Footprint(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            SetValue(footprint);
        }

        public void SetValue(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            this.FolderName = footprint.FolderName;
            this.Name = footprint.Name;
            this.User = footprint.User;
            this.Id = footprint.Id;
            this.Public = footprint.Public;
            this.FillFactor = footprint.FillFactor;
            this.FootprintType = footprint.Type;
            this.Comment = footprint.Comment;
            this.FolderId = footprint.FolderId;
            //TODO : host name?
            this.Url = new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/"+this.User+"/"+this.FolderName+"/"+this.Name);
            if (footprint.Region != null)
            {
                this.RegionString = footprint.Region.ToString();
            }
            else this.RegionString = null;
        }

        public Jhu.Footprint.Web.Lib.Footprint GetValue()
        {
            var footprint = new Jhu.Footprint.Web.Lib.Footprint();

            footprint.Name = this.Name;
            footprint.FolderName = this.FolderName;
            footprint.User = this.User;
            footprint.Id = this.Id;
            footprint.Public = this.Public;
            footprint.FillFactor = this.FillFactor;
            footprint.Type = this.FootprintType;
            footprint.Comment = this.Comment;
            footprint.FolderId = this.FolderId;
            if (this.RegionString != null)
            {
                footprint.Region = Jhu.Spherical.Region.Parse(this.RegionString);
            }
            else footprint.Region = null;

            return footprint;
        }
    }
}
