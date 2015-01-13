using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class TestsPageControl : BasePageControl<TestWriterEntity>, ITestsPageView {
        public TestsPageControl() {
            InitializeComponent();

            AddControlTextValidation<TestWriterEntity>(txtComment, TestWriterEntity.ChangeCommentProperty);
            AddControlTextValidation<TestWriterEntity>(txtDescriptionSuffix, TestWriterEntity.DescriptionSuffixProperty);
            AddControlTextValidation<TestWriterEntity>(cboCreateDefect, TestWriterEntity.CreateDefectProperty);
            AddControlTextValidation<TestWriterEntity>(cboPassedTestStatus, TestWriterEntity.PassedOidProperty);
            AddControlTextValidation<TestWriterEntity>(cboFailedTestStatus, TestWriterEntity.FailedOidProperty);
            AddControlTextValidation<TestWriterEntity>(cboReferenceField, TestWriterEntity.ReferenceAttributeProperty);
        }

        public override void DataBind() {
            AddControlBinding(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddSimpleComboboxBinding(cboReferenceField, Model, TestWriterEntity.ReferenceAttributeProperty);
            AddControlBinding(cboPassedTestStatus, Model, TestWriterEntity.PassedOidProperty);
            AddControlBinding(cboFailedTestStatus, Model, TestWriterEntity.FailedOidProperty);
            AddControlBinding(txtComment, Model, TestWriterEntity.ChangeCommentProperty);
            AddControlBinding(txtDescriptionSuffix, Model, TestWriterEntity.DescriptionSuffixProperty);
            AddSimpleComboboxBinding(cboCreateDefect, Model, TestWriterEntity.CreateDefectProperty);

            FillComboBoxWithStrings(cboReferenceField, ReferenceFieldList);
            FillComboBoxWithStrings(cboCreateDefect, CreateDefectChoiceList);
            FillComboBoxWithListValues(cboPassedTestStatus, PassedTestStatusList);
            FillComboBoxWithListValues(cboFailedTestStatus, FailedTestStatusList);

            BindHelpStrings();
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(cboReferenceField, Model, TestWriterEntity.ReferenceAttributeProperty);
            AddHelpSupport(cboPassedTestStatus, Model, TestWriterEntity.PassedOidProperty);
            AddHelpSupport(cboFailedTestStatus, Model, TestWriterEntity.FailedOidProperty);
            AddHelpSupport(txtComment, Model, TestWriterEntity.ChangeCommentProperty);
            AddHelpSupport(txtDescriptionSuffix, Model, TestWriterEntity.DescriptionSuffixProperty);
            AddHelpSupport(cboCreateDefect, Model, TestWriterEntity.CreateDefectProperty);            
        }

        #region ITestsPageView Members

        public IEnumerable<string> ReferenceFieldList { get; set; }

        public IEnumerable<string> CreateDefectChoiceList { get; set; }

        public IList<ListValue> PassedTestStatusList { get; set; }

        public IList<ListValue> FailedTestStatusList { get; set; }

        #endregion
    }
}