using System.Linq;
using System;
using System.Collections;
using System.Windows.Forms;
using StUtil.Extensions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace StUtil.UI.Controls.Explorer
{
    public abstract class ExplorerTreeViewWnd : TreeView
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        public ExplorerTreeViewWnd()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            UpdateStyles();
        }

        #region Constants

        private const Int32 PRF_CLIENT = 4;
        private const Int32 TV_FIRST = 0x1100;
        private const Int32 TVM_GETEXTENDEDSTYLE = TV_FIRST + 45;
        private const Int32 TVM_SETAUTOSCROLLINFO = TV_FIRST + 59;
        private const Int32 TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
        private const Int32 TVS_EX_AUTOHSCROLL = 0x0020;
        private const Int32 TVS_EX_FADEINOUTEXPANDOS = 0x0040;
        private const Int32 TVS_NOHSCROLL = 0x8000;
        private const Int32 WM_ERASEBKGND = 0x0014;
        private const Int32 WM_PRINTCLIENT = 0x0318;

        #endregion

        /// <summary>
        /// Encapsulates the information needed when creating a control
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= TVS_NOHSCROLL;
                return cp;
            }
        }

        /// <summary>
        /// Overrides the base OnHandleCreated 
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            Int32 dw = (Int32)SendMessage(Handle, TVM_GETEXTENDEDSTYLE,
                IntPtr.Zero, IntPtr.Zero);

            dw |= TVS_EX_AUTOHSCROLL;
            dw |= TVS_EX_FADEINOUTEXPANDOS;

            SendMessage(Handle, TVM_SETEXTENDEDSTYLE, IntPtr.Zero, new IntPtr(dw));

            Int32 hresult = SetWindowTheme(Handle, "explorer", null);
            Tag = hresult;
        }

        /// <summary>
        /// Overrides the base OnPaint
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint))
            {
                Message m = new Message();
                m.HWnd = Handle;
                m.Msg = WM_PRINTCLIENT;
                m.WParam = e.Graphics.GetHdc();
                m.LParam = (IntPtr)PRF_CLIENT;
                DefWndProc(ref m);
                e.Graphics.ReleaseHdc(m.WParam);
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Overrides the base WndProc
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ERASEBKGND)
            {
                m.Result = IntPtr.Zero;
                return;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
     
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            // Remove the placeholder node.
            e.Node.Nodes.Clear();

            // We stored the ShellItem object in the node's Tag property - hah!
            ShellItem shNode = (ShellItem)e.Node.Tag;
            ArrayList arrSub = shNode.GetSubFolders();
            foreach (ShellItem shChild in arrSub)
            {
                TreeNode tvwChild = new TreeNode();
                tvwChild.Text = shChild.DisplayName;
                tvwChild.ImageIndex = shChild.IconIndex;
                tvwChild.SelectedImageIndex = shChild.IconIndex;
                tvwChild.Tag = shChild;

                // If this is a folder item and has children then add a place holder node.
                if (shChild.IsFolder && shChild.HasSubFolder)
                    tvwChild.Nodes.Add("PH");
                e.Node.Nodes.Add(tvwChild);
            }

            base.OnBeforeExpand(e);
        }
    }
}
