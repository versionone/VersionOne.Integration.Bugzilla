using System;
using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    public abstract class BaseEntity {
        public const string DisabledProperty = "Disabled";

        [XmlIgnore]
        public virtual string TagName { get; set; }

        [XmlAttribute("class")]
        public string ClassName {
            get { return ServicesMap.GetByEntityType(GetType()).FullTypeNameAndAssembly; }
            set { }
        }

        [XmlIgnore]
        [HelpString(HelpResourceKey = "CommonDisabled")]
        public bool Disabled { get; set; }

        [XmlAttribute("disabled")]
        public int DisabledNumeric {
            get { return Convert.ToInt32(Disabled); }
            set { Disabled = Convert.ToBoolean(value); }
        }
    }
}