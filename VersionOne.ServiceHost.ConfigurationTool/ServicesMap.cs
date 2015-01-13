using System;
using System.Collections.Generic;
using System.Linq;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool {
    /// <summary>
    /// Collect all services and timers class names and map them to Entity.
    /// </summary>
    public class ServicesMap {
        private static readonly ICollection<ServicesMap> instances = new List<ServicesMap>();

        public static readonly ServicesMap WorkitemWriterService = new ServicesMap(
            typeof(WorkitemWriterEntity),
            "VersionOne.ServiceHost.WorkitemServices.WorkitemWriterHostedService",
            "VersionOne.ServiceHost.WorkitemServices.WorkitemWriterHostedService+IntervalSync");

        public static readonly ServicesMap TestWriterService = new ServicesMap(
            typeof(TestWriterEntity),
            "VersionOne.ServiceHost.TestServices.TestWriterService",
            null);

        public static readonly ServicesMap TestService = new ServicesMap(
            typeof(TestServiceEntity),
            "VersionOne.ServiceHost.TestServices.TestService",
            "VersionOne.ServiceHost.TestServices.TestReaderService+IntervalSync"
            );

        public static readonly ServicesMap ChangeSetWriterService = new ServicesMap(
            typeof(ChangesetWriterEntity),
            "VersionOne.ServiceHost.SourceServices.ChangeSetWriterService",
            null);

        public static readonly ServicesMap BugzillaService = new ServicesMap(
            typeof(BugzillaServiceEntity),
            "VersionOne.ServiceHost.BugzillaServices.BugzillaHostedService",
            "VersionOne.ServiceHost.BugzillaServices.BugzillaHostedService+IntervalSync");

        public static readonly ServicesMap TimerService = new ServicesMap(
            typeof(TimerEntity),
            "VersionOne.ServiceHost.Core.Services.TimePublisherService",
            null,
            "VersionOne.ServiceHost.Core");

        public readonly Type EntityType;
        public readonly string TypeName;
        public readonly string Namespace;
        public readonly string Assembly;
        public readonly string PublishClass;

        public string FullTypeName {
            get { return Namespace + "." + TypeName; }
        }

        public string FullTypeNameAndAssembly {
            get { return FullTypeName + ", " + Assembly; }
        }

        private ServicesMap(Type entity, string serviceFullName, string publishClass) {
            EntityType = entity;
            var x = serviceFullName.LastIndexOf('.');
            TypeName = serviceFullName.Substring(x + 1);
            Namespace = serviceFullName.Substring(0, x);
            Assembly = Namespace;
            PublishClass = publishClass;
            instances.Add(this);
        }

        private ServicesMap(Type entity, string serviceFullName, string publishClass, string serviceAssembly) {
            EntityType = entity;
            var x = serviceFullName.LastIndexOf('.');
            TypeName = serviceFullName.Substring(x + 1);
            Namespace = serviceFullName.Substring(0, x);
            Assembly = serviceAssembly;
            PublishClass = publishClass;
            instances.Add(this);
        }

        public static ServicesMap GetByFullTypeAndAssembly(string fullType, string assembly) {
            return instances.FirstOrDefault(service => service.FullTypeName == fullType && service.Assembly == assembly);
        }

        public static ServicesMap GetByEntityType(Type type) {
            return instances.FirstOrDefault(service => service.EntityType == type);
        }

        public static ServicesMap GetByPublishClass(string fullType, string assembly) {
            return instances.FirstOrDefault(service => service.PublishClass == fullType && service.Assembly == assembly);
        }
    }
}