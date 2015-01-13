using System;

namespace VersionOne.ServiceHost.ConfigurationTool {
    public class StateDefinition {
        public readonly Type ControllerType;
        public readonly Type ViewType;
        public readonly Type EntityType;
        public readonly ServicesGroup Group;

        public StateDefinition(Type controllerType, Type viewType, Type entityType, ServicesGroup servicesGroup) {
            ControllerType = controllerType;
            ViewType = viewType;
            EntityType = entityType;
            Group = servicesGroup;
        }
    }
}
