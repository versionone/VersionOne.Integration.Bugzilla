using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class TestCompletePageControl : BasePageControl<TestCompleteEntity>, ITestCompletePageView {
        public TestCompletePageControl () {
            InitializeComponent();

            var validationProvider = GetValidationProvider(typeof(TestCompleteEntity));
            pscWatchSuite.AddControlValidation(TestCompleteEntity.ProjectSuiteConfigProperty, validationProvider);
        }

        public override void DataBind () {
            AddControlBinding(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddControlBinding(numTimeoutInterval, Model, TestCompleteEntity.RetryTimeoutProperty);
            AddControlBinding(numAttemps, Model, TestCompleteEntity.RetryAttemptsProperty);
            AddControlBinding(numIntervalMinutes, Model.Timer, TimerEntity.TimerProperty);

            pscWatchSuite.AddControlBinding(Model, TestCompleteEntity.ProjectSuiteConfigProperty);

            BindHelpStrings();
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(lblPollIntervalSuffix, Model.Timer, TimerEntity.TimerProperty);
            AddHelpSupport(lblTimeoutIntervalSuffix, Model, TestCompleteEntity.RetryTimeoutProperty);
            AddHelpSupport(lblAttemptsSuffix, Model, TestCompleteEntity.RetryAttemptsProperty);
            AddHelpSupport(pscWatchSuite, Model, TestCompleteEntity.ProjectSuiteConfigProperty, 0);
        }
    }
}