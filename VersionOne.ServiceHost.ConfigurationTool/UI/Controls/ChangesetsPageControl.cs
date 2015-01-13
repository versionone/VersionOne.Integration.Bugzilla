using System;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class ChangesetsPageControl : BasePageControl<ChangesetWriterEntity>, IChangesetsPageView {
        public ChangesetsPageControl() {
            InitializeComponent();

            AddValidationProvider(typeof(ChangesetLink));
            AddControlValidation<ChangesetWriterEntity>(txtComment, ChangesetWriterEntity.ChangeCommentProperty, "Text");
            AddControlValidation<ChangesetLink>(txtLinkUrl, ChangesetLink.UrlProperty, "Text");
            AddControlValidation<ChangesetLink>(txtLinkName, ChangesetLink.NameProperty, "Text");
        }

        public override void DataBind() {
            AddControlBinding(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddControlBinding(txtComment, Model, ChangesetWriterEntity.ChangeCommentProperty);
            AddControlBinding(chkAlwaysCreateChangeset, Model, ChangesetWriterEntity.AlwaysCreateProperty);
            AddControlBinding(chkLinkOnMenu, Model.Link.OnMenu, NullableBool.BoolValueProperty);
            AddControlBinding(txtLinkName, Model.Link, ChangesetLink.NameProperty);
            AddControlBinding(txtLinkUrl, Model.Link, ChangesetLink.UrlProperty);

            BindHelpStrings();
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(txtComment, Model, ChangesetWriterEntity.ChangeCommentProperty);
            AddHelpSupport(chkAlwaysCreateChangeset, Model, ChangesetWriterEntity.AlwaysCreateProperty);
            AddHelpSupport(chkLinkOnMenu, Model.Link, ChangesetLink.OnMenuProperty);
            AddHelpSupport(txtLinkName, Model.Link, ChangesetLink.NameProperty);
            AddHelpSupport(txtLinkUrl, Model.Link, ChangesetLink.UrlProperty);
        }
    }
}