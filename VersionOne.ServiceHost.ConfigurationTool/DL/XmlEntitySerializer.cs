using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using VersionOne.ServiceHost.ConfigurationTool.Entities;
using log4net;

namespace VersionOne.ServiceHost.ConfigurationTool.DL {
    /// <summary>
    /// Support for converting entities to XML and flushing to file.
    /// </summary>
    public class XmlEntitySerializer {
        private readonly XmlDocument document = new XmlDocument();
        private readonly XmlDocument outputDocument = new XmlDocument();

        private readonly ILog logger = LogManager.GetLogger(typeof(XmlEntitySerializer)); 

        public const string RootNodeXPath = "/configuration/Services";

        public XmlDocument Document {
            get { return document; }
        }

        /// <summary>
        /// Tests-only usage. TODO refactor to something like [string GetXmlOutput()]
        /// </summary>
        public XmlDocument OutputDocument {
            get { return outputDocument; }
        }

        // TODO move XML stub to tests, the only place that needs it, and delete .ctor
        public XmlEntitySerializer() {
            Document.LoadXml(Resources.ConfigurationXmlStub);
        }

        private XmlNode GetServicesNode() {
            var node = document.SelectSingleNode(RootNodeXPath);
            
            if (node == null) {
                throw new InvalidOperationException("Document does not contain node accessible by path " + RootNodeXPath);
            }
            return node;
        }

        private XmlNode GetServicesNodeInOutputDoc() {
            var node = outputDocument.SelectSingleNode(RootNodeXPath);
            
            if (node == null) {
                throw new InvalidOperationException("Document does not contain node accessible by path " + RootNodeXPath);
            }
            return node;
        }

        /// <summary>
        /// Create XML Node from configuration entity
        /// </summary>
        /// <param name="entity">Entity to process</param>
        /// <returns>Converted XML node</returns>
        private XmlNode TransformEntityToNode(BaseEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            XmlNode resultNode;
            var xmlSerializer = new XmlSerializer(entity.GetType());
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            
            using (var memoryStream = new MemoryStream()) {
                try {
                    xmlSerializer.Serialize(memoryStream, entity, namespaces);
                } catch (InvalidOperationException) {
                    return null;
                }

                memoryStream.Position = 0;
                var serializationDoc = new XmlDocument();
                serializationDoc.Load(memoryStream);
                resultNode = serializationDoc.DocumentElement;
            }

            return outputDocument.ImportNode(resultNode, true);
        }

        /// <summary>
        /// Convert entities to XML nodes and append to Services node in XML document.
        /// </summary>
        /// <param name="serviceEntities">Entities to convert to XML and store in document.</param>
        public void Serialize(IEnumerable<BaseServiceEntity> serviceEntities) {
            outputDocument.LoadXml(document.InnerXml);
            
            foreach (var entity in serviceEntities) {
                Serialize(entity);
            }
        }

        /// <summary>
        /// Convert entity to XML node and append to Services node in XML document.
        /// </summary>
        /// <param name="entity">Entities to convert to XML and store in document.</param>
        private void Serialize(BaseEntity entity) {
            if(entity == null) {
                return;
            }
            
            var servicesNode = GetServicesNodeInOutputDoc();
            var node = TransformEntityToNode(entity);
            servicesNode.AppendChild(node);
            
            if(entity is BaseServiceEntity) {
                var serviceEntity = (BaseServiceEntity) entity;
                
                if (serviceEntity.HasTimer) {
                    var timerNode = TransformEntityToNode(serviceEntity.Timer);
                    servicesNode.AppendChild(timerNode);
                }
            }
        }

        /// <summary>
        /// Dump XML to file.
        /// </summary>
        /// <param name="fileName">File path</param>
        public void SaveToFile(string fileName) {
            outputDocument.Save(fileName);
        }

        /// <summary>
        /// Load XML from file.
        /// </summary>
        /// <param name="fileName">File path</param>
        public void LoadFromFile(string fileName) {
            document.Load(fileName);
        }

        /// <summary>
        /// Convert XML nodes in Document property to entities.
        /// </summary>
        /// <returns>Collection of entities. Not null.</returns>
        // TODO refactor
        public ICollection<BaseServiceEntity> Deserialize() {
            var serviceNodes = document.SelectNodes(RootNodeXPath + "/*");
            var servicesNode = GetServicesNode();
            
            StripComments(servicesNode);
            
            var services = new List<BaseServiceEntity>();
            var timers = new List<TimerEntity>();
            IDictionary<TimerEntity, XmlNode> timerToNodeMap = new Dictionary<TimerEntity, XmlNode>();

            foreach (XmlNode node in serviceNodes) {
                var entity = TransformNodeToEntity(node);

                if(entity is TimerEntity) {
                    var timer = (TimerEntity) entity;
                    timers.Add(timer);
                    timerToNodeMap.Add(timer, node);
                } else if(entity != null) {
                    services.Add((BaseServiceEntity) entity);
                    servicesNode.RemoveChild(node);
                }
            }

            var mappedEntities = BindTimersToServices(services, timers);
            
            foreach(var entity in mappedEntities.Where(entity => entity.HasTimer && timerToNodeMap.ContainsKey(entity.Timer))) {
                try {
                    servicesNode.RemoveChild(timerToNodeMap[entity.Timer]);
                } catch(ArgumentException) {
                    // NOTE We do nothing. Timer may be shared between several services, so it could have been removed.
                }
            }

            return mappedEntities;
        }

        private static void StripComments(XmlNode parentNode) {
            var commentNodes = parentNode.ChildNodes.Cast<XmlNode>().Where(node => node.NodeType == XmlNodeType.Comment).ToList();
            commentNodes.ForEach(item => parentNode.RemoveChild(item));
        }

        private static ICollection<BaseServiceEntity> BindTimersToServices(ICollection<BaseServiceEntity> services, IEnumerable<TimerEntity> timers) {
            foreach (var timer in timers) {
                var serviceDesc = FindEntityByPublishString(timer.PublishClass);
                
                if(serviceDesc != null) {
                    foreach(var entity in services.Where(entity => entity.GetType() == serviceDesc.EntityType)) {
                        entity.Timer = timer;
                    }
                }
            }
            return services;
        }

        private static ServicesMap FindEntityByPublishString(string publishString) {
            if (publishString == null) {
                return null;
            }
            
            publishString = publishString.Replace(" ", "");
            var x = publishString.IndexOf(',');
            var publishFullType = publishString.Substring(0, x);
            var publishAssembly = publishString.Substring(x + 1);
            
            return ServicesMap.GetByPublishClass(publishFullType, publishAssembly);
        }

        private BaseEntity TransformNodeToEntity(XmlNode node) {
            var serviceString = node.Attributes["class"].Value.Replace(" ", "");
            var x = serviceString.IndexOf(',');
            var serviceFullType = serviceString.Substring(0, x);
            var serviceAssembly = serviceString.Substring(x + 1);
            var serviceType = serviceFullType.Substring(serviceFullType.LastIndexOf('.') + 1);
            var service = ServicesMap.GetByFullTypeAndAssembly(serviceFullType, serviceAssembly);
            
            if (service == null) {
                logger.Debug("While deserializing skipped unknown entity: " + serviceString);
                return null;
            }
            
            XmlReader reader = new XmlNodeReader(RenameNode(node, serviceType));
            var serializer = new XmlSerializer(service.EntityType);
            
            return (BaseEntity) serializer.Deserialize(reader);
        }

        private static XmlNode RenameNode(XmlNode node, string newName) {
            var newNode = node.OwnerDocument.CreateNode(node.NodeType, newName, null);
            
            foreach (XmlNode childNode in node.ChildNodes) {
                newNode.AppendChild(childNode.CloneNode(true));
            }

            foreach (XmlAttribute attribute in node.Attributes) {
                newNode.Attributes.Append((XmlAttribute) attribute.CloneNode(true));
            }

            return newNode;
        }
    }
}