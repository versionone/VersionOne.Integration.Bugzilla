using System;

namespace VersionOne.ServiceHost.ConfigurationTool.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HelpStringAttribute : Attribute {
        public readonly string Content;

        public string HelpResourceKey { get; set; }

        public HelpStringAttribute() { }

        public HelpStringAttribute(string content) {
            if (content == null) {
                throw new ArgumentNullException("content");
            }

            Content = content;
        }
    }
}