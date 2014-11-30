using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StUtil.Native
{
    public class WndProcOverride : NativeWindow, IDisposable
    {
        /// <summary>
        /// Gets the target to override the wndproc of.
        /// </summary>
        /// <value>
        /// The target to override the wndproc of.
        /// </value>
        public Control Target { get; private set; }

        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>
        /// The handlers.
        /// </value>
        public List<WndProcHandler> Handlers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WndProcOverride"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public WndProcOverride(Control target, params WndProcHandler[] handlers)
        {
            this.Target = target;
            this.Handlers = new List<WndProcHandler>();
            if (handlers.Length > 0)
            {
                this.Handlers.AddRange(handlers);
            }
            if (Target.IsHandleCreated)
            {
                this.AssignHandle(Target.Handle);
            }
            Target.HandleCreated += Control_HandleCreated;
        }

        public WndProcOverride(IntPtr hWnd, params WndProcHandler[] handlers)
        {
            Control target = Control.FromHandle(hWnd);
            this.Target = target;
            this.Handlers = new List<WndProcHandler>();
            if (handlers.Length > 0)
            {
                this.Handlers.AddRange(handlers);
            }
            if (Target.IsHandleCreated)
            {
                this.AssignHandle(Target.Handle);
            }
            Target.HandleCreated += Control_HandleCreated;
        }

        /// <summary>
        /// Handle the creation of the control
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Empty</param>
        private void Control_HandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(Target.Handle);
        }

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message.</param>
        protected override void WndProc(ref Message m)
        {
            bool stopProp = false;
            foreach (WndProcHandler handler in Handlers)
            {
                handler.Handle(ref m, out stopProp);
                if (stopProp) return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Releases the window and stops overriding the WndProc
        /// </summary>
        public void Dispose()
        {
            this.ReleaseHandle();
            this.Target.HandleCreated -= Control_HandleCreated;
        }

        /// <summary>
        /// Gets a click through handler.
        /// </summary>
        public BaseWndProcHandler CreateClickThroughHandler()
        {
            const int HTTRANSPARENT = -1;
            return new WndProcHandler(delegate(ref Message msg, out bool stopPropagation)
            {
                msg.Result = (IntPtr)HTTRANSPARENT;
                stopPropagation = true;
            })
            {
                MessageId = (int)Internal.NativeEnums.WM.NCHITTEST
            };
        }

        /// <summary>
        /// Creates a title bar handler.
        /// </summary>
        /// <param name="titleBar">The title bar.</param>
        /// <returns></returns>
        public BaseWndProcHandler CreateTitleBarHandler(Control titleBar)
        {
            const int HTCAPTION = 2;
            return new WndProcHandler(delegate(ref Message msg, out bool stopPropagation)
            {
                if (titleBar.ClientRectangle.Contains(titleBar.PointToClient(Cursor.Position)))
                {
                    msg.Result = new IntPtr(HTCAPTION);
                    stopPropagation = true;
                }
                stopPropagation = false;
            })
            {
                MessageId = (int)Internal.NativeEnums.WM.NCHITTEST
            };
        }
    }
}