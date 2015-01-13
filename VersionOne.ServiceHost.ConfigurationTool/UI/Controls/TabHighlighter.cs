using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public class TabHighlighter : Component {
        private const int ErrorImageIndex = 0;
        private const int EmptyImageIndex = -1;

        private readonly ImageList imageList = new ImageList();
        private readonly TabControl tabControl;

        public TabHighlighter(TabControl tabControl) {
            this.tabControl = tabControl;
            imageList.ImageSize = new Size(8, 8);
            imageList.Images.Add(Resources.ErrorMarkImage, Color.White);
            tabControl.ImageList = imageList;
        }

        public void SetTabPageValidationMark(TabPage page, bool isValid) {
            if(tabControl.TabPages.Contains(page)) {
                page.ImageIndex = isValid ? EmptyImageIndex : ErrorImageIndex;
            }
        }
    }
}