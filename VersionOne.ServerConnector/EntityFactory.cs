using System;
using System.Collections.Generic;
using System.Linq;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector {
    // TODO extract interface and inject into VersionOneProcessor
    internal class EntityFactory {
        private readonly IMetaModel metaModel;
        private readonly IServices services;
        private readonly IEnumerable<AttributeInfo> attributesToQuery; 

        internal EntityFactory(IMetaModel metaModel, IServices services, IEnumerable<AttributeInfo> attributesToQuery) {
            this.metaModel = metaModel;
            this.services = services;
            this.attributesToQuery = attributesToQuery;
        }

        internal Asset Create(string assetTypeName, IEnumerable<AttributeValue> attributeValues) {
            var assetType = metaModel.GetAssetType(assetTypeName);
            var asset = services.New(assetType, Oid.Null);

            foreach (var attributeValue in attributeValues) {
                if(attributeValue is SingleAttributeValue) {
                    asset.SetAttributeValue(assetType.GetAttributeDefinition(attributeValue.Name), ((SingleAttributeValue)attributeValue).Value);
                } else if(attributeValue is MultipleAttributeValue) {
                    var values = ((MultipleAttributeValue) attributeValue).Values;
                    var attributeDefinition = assetType.GetAttributeDefinition(attributeValue.Name);

                    foreach (var value in values) {
                        asset.AddAttributeValue(attributeDefinition, value);
                    }

                } else {
                    throw new NotSupportedException("Unknown Attribute Value type.");
                }
            }

            foreach (var attributeInfo in attributesToQuery.Where(attributeInfo => attributeInfo.Prefix == assetTypeName)) {
                asset.EnsureAttribute(assetType.GetAttributeDefinition(attributeInfo.Attr));
            }

            services.Save(asset);
            return asset;
        }
    }
}