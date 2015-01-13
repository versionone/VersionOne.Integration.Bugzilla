using System;
using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.DL;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    internal class ConnectionValidatorFactory {
        private readonly IDictionary<Type, Type> entityToValidatorTypeMappings = new Dictionary<Type, Type>();

        private static ConnectionValidatorFactory instance;

        public static ConnectionValidatorFactory Instance {
            get { return instance ?? (instance = new ConnectionValidatorFactory()); }
        }

        private ConnectionValidatorFactory() {
            entityToValidatorTypeMappings.Add(typeof(BugzillaServiceEntity), typeof(BugzillaConnectionValidator));
        }

        public IConnectionValidator CreateValidator(BaseServiceEntity entity) {
            var entityType = entity.GetType();

            if(!entityToValidatorTypeMappings.ContainsKey(entityType)) {
                throw new NotSupportedException("Wrong entity type");
            }

            var validatorType = entityToValidatorTypeMappings[entityType];
            var validator = (IConnectionValidator) Activator.CreateInstance(validatorType, new object[] { entity });

            return validator;
        }
    }
}