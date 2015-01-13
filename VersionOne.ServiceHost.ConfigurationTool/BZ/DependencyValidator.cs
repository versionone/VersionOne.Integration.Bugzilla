using System.Linq;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    /// <summary>
    /// Dependency validator to check Services consistency, validate dependencies that exist between services
    /// and to enforce pages requiring established V1 connection.
    /// </summary>
    public class DependencyValidator {
        private readonly IFacade facade;

        public DependencyValidator(IFacade facade) {
            this.facade = facade;
        }

        /// <summary>
        /// Throw exception if entity requires connection to V1 and actual connection is down.
        /// </summary>
        /// <param name="entity">entity to validate</param>
        /// <exception cref="V1ConnectionRequiredException"/>
        public void CheckVersionOneDependency(BaseServiceEntity entity) {
            if(entity.GetType().IsDefined(typeof(DependsOnVersionOneAttribute), false) && !facade.IsConnected) {
                throw new V1ConnectionRequiredException();
            }
        }

        /// <summary>
        /// Throw exception if entity depends on other entity that is missing.
        /// </summary>
        /// <param name="entity">entity to validate</param>
        /// <param name="config">Service Host configuration container</param>
        /// <exception cref="DependencyFailureException" />
        public void CheckOtherServiceDependency(BaseServiceEntity entity, ServiceHostConfiguration config) {
            var attributes = entity.GetType().GetCustomAttributes(typeof (DependsOnServiceAttribute), false);
            
            if(attributes.Length < 1) {
                return;
            }

            foreach(var attribute in attributes.Cast<DependsOnServiceAttribute>().Where(attribute => config[attribute.ServiceType] == null)) {
                throw new DependencyFailureException(attribute.ServiceType, "Service dependency does not exist");
            }
        }

        /// <summary>
        /// Throw exception if Service entities depend on other entities that are missing. Usable when business-validating data
        /// coming from configuration file.
        /// </summary>
        /// <param name="config">Service Host configuration container to validate</param>
        /// <exception cref="DependencyFailureException" />
        public void CheckServiceDependencies(ServiceHostConfiguration config) {
            foreach(var entity in config.Services) {
                CheckOtherServiceDependency(entity, config);
            }
        }
    }
}