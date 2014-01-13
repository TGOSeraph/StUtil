using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StUtil.Generic;
using StUtil.Extensions;
using System.Windows.Forms.Design;

namespace StUtil.UI.Controls
{
    public partial class FileSystemSelector : UserControl
    {
        public event EventHandler<EventArgs<string>> PathChanged;

        public enum FileSystemObjectType
        {
            Directory,
            File
        }

        [Browsable(true)]
        [DefaultValue(FileSystemObjectType.Directory)]
        public FileSystemObjectType FileSystemType { get; set; }

        public FileSystemSelector()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (this.FileSystemType == FileSystemObjectType.Directory)
            {
                FolderBrowserDialog dialog = GetFolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = dialog.SelectedPath;
                    PathChanged.RaiseEvent(this, pathTextBox.Text);
                }
                dialog.Dispose();
            }
            else if (this.FileSystemType == FileSystemObjectType.File)
            {
                OpenFileDialog dialog = GetOpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = dialog.FileName;
                    PathChanged.RaiseEvent(this, pathTextBox.Text);
                }
                dialog.Dispose();
            }
        }

        protected virtual FolderBrowserDialog GetFolderBrowserDialog()
        {
            return new FolderBrowserDialog();
        }

        protected virtual OpenFileDialog GetOpenFileDialog()
        {
            return new OpenFileDialog();
        }
    }
}
