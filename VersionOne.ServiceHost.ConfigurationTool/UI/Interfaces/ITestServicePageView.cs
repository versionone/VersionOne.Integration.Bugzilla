using System;
using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface ITestServicePageView : IPageView<TestServiceEntity> {
        event EventHandler<TestProjectEventArgs> ProjectMapRowsChanged;

        IEnumerable<string> ReferenceFieldList { get; set; }
        IList<ListValue> PassedTestStatusList { get; set; }
        IList<ListValue> FailedTestStatusList { get; set; }
        IEnumerable<string> CreateDefectChoiceList { get; set; }
        IList<string> Projects { get; set; }
    }
}