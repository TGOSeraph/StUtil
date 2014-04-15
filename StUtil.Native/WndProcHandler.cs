using StUtil.Extensions;
using StUtil.Internal.Native;
using StUtil.Native.Controls;
using StUtil.Reflection;
using StUtil.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Native
{
    /// <summary>
    /// Class to hook the WndProc of a control
    /// </summary>
    public class WndProcHandler : NativeWindow, IDisposable
    {
        /// <summary>
        /// The control whos WndProc should be hooked
        /// </summary>
        public Control Control { get; set; }

        public object Tag { get; set; }

        /// <summary>
        /// List of handlers for messages
        /// </summary>
        public Dictionary<int, List<MessageHandlerProc>> MessageHandlers { get; private set; }

        /// <summary>
        /// Delegate function for handling a message, return true to prevent propagating the message
        /// </summary>
        /// <param name="handler">The handler</param>
        /// <param name="message">The message</param>
        /// <returns>If further processing of this message should be stopped</returns>
        public delegate bool MessageHandlerProc(WndProcHandler handler, ref Message message);

        /// <summary>
        /// Create a new WndProc handler
        /// </summary>
        /// <param name="control">The control to hook the WndProc of</param>
        /// <param name="handlers">An array of message handlers</param>
        public WndProcHandler(Control control, params MessageHandler[] handlers)
        {
            this.MessageHandlers = new Dictionary<int, List<MessageHandlerProc>>();
            this.Control = control;

            foreach (MessageHandler handler in handlers)
            {
                AddMessageHandler(handler);
            }

            if (Control.IsHandleCreated)
            {
                this.AssignHandle(Control.Handle);
            }
            Control.HandleCreated += Control_HandleCreated;
        }

        /// <summary>
        /// Handle the creation of the control
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Empty</param>
        private void Control_HandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(Control.Handle);
        }

        /// <summary>
        /// Add a new message handler
        /// </summary>
        /// <param name="handler">The handler to add</param>
        public void AddMessageHandler(MessageHandler handler)
        {
            AddMessageHandler(handler.Message, handler.Handler);
        }

        /// <summary>
        /// Add a new message handler
        /// </summary>
        /// <param name="message">The id of the message to handle</param>
        /// <param name="handler">The delegate to invoke to process the message</param>
        public void AddMessageHandler(int message, MessageHandlerProc handler)
        {
            if (this.MessageHandlers.ContainsKey(message))
            {
                this.MessageHandlers[message].Add(handler);
            }
            else
            {
                this.MessageHandlers.Add(message, new List<MessageHandlerProc>(new MessageHandlerProc[] { handler }));
            }
        }

        /// <summary>
        /// Override the WndProc method to pass any hooked messages to the specific handler
        /// </summary>
        /// <param name="m">The message recieved</param>
        protected override void WndProc(ref Message m)
        {
            //Pass it on
            base.WndProc(ref m);

            //Loop through each handler and check if we have one for this message
            if (this.MessageHandlers.ContainsKey(m.Msg))
            {
                foreach (MessageHandlerProc handler in this.MessageHandlers[m.Msg])
                {
                    if (handler(this, ref m))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Class containing the message handler info
        /// </summary>
        public class MessageHandler
        {
            /// <summary>
            /// The ID of the message to handle
            /// </summary>
            public int Message { get; set; }

            /// <summary>
            /// The handler function
            /// </summary>
            public MessageHandlerProc Handler { get; set; }

            /// <summary>
            /// Any data to tag this message handler with
            /// </summary>
            public object Tag { get; set; }

            /// <summary>
            /// Create a new handler 
            /// </summary>
            /// <param name="message">The message id to handle</param>
            /// <param name="handler">The handler function</param>
            public MessageHandler(int message, MessageHandlerProc handler)
            {
                this.Message = message;
                this.Handler = handler;
            }

            /// <summary>
            /// Create a new empty handler
            /// </summary>
            public MessageHandler()
            {
            }
        }

        /// <summary>
        /// Cleanup the object
        /// </summary>
        public void Dispose()
        {
            this.ReleaseHandle();
            this.Control.HandleCreated -= Control_HandleCreated;
        }

        #region Example WndProc Overrides

        /// <summary>
        /// Create a handler that will override messages to set the control as transparent
        /// </summary>
        /// <returns>A MessageHandler that when added to a WndProc handler will treat the control as transparent</returns>
        public static MessageHandler CreateTransparentWindowHandler()
        {
            const int HTTRANSPARENT = -1;
            return new MessageHandler
            (
                NativeConsts.WM_NCHITTEST,
                new MessageHandlerProc(delegate(WndProcHandler w, ref Message m)
                {
                    m.Result = (IntPtr)HTTRANSPARENT;
                    return false;
                })
            );
        }

        /// <summary>
        /// Create a handler that will override messages to mark the control as the title bar
        /// </summary>
        /// <param name="titleBar">The control that is to be used as the title bar</param>
        /// <returns>A MessageHandler that when added to a WndProc handler will treat the titleBar control as the forms title bar</returns>
        public static MessageHandler CreateTitleBarHandler(Control titleBar)
        {
            return new MessageHandler
            (
                NativeConsts.WM_NCHITTEST,
                new MessageHandlerProc(delegate(WndProcHandler w, ref Message m)
                {
                    const int HTCAPTION = 2;
                    if (titleBar.ClientRectangle.Contains(titleBar.PointToClient(Cursor.Position)))
                    {
                        m.Result = new IntPtr(HTCAPTION);
                    }
                    return true;
                })
            );
        }

        /// <summary>
        /// Create a handler that will allow the form to be resized using the outer border
        /// </summary>
        /// <typeparam name="T">The type of object containing info on the padding to use</typeparam>
        /// <param name="control">The Control to mark as resizable</param>
        /// <param name="borders">The outer border area to use as the resizable border</param>
        /// <returns>A MessageHandler that when added to a WndProc handler will treat the control as resizable</returns>
        public static MessageHandler CreateResizeHandler<T>(Control control, PropertyPointer<T, Padding> borders)
        {
            return new MessageHandler
            (
                NativeConsts.WM_NCHITTEST,
                new MessageHandlerProc(delegate(WndProcHandler w, ref Message m)
                {
                    Point pt = control.PointToClient(Cursor.Position);
                    Padding Borders = borders.Value;
                    if (pt.X <= Borders.Left)
                    {
                        if (pt.Y <= Borders.Top)
                        {
                            m.Result = (IntPtr)NativeConsts.HTTOPLEFT;
                        }
                        else if (pt.Y >= w.Control.Height - Borders.Bottom)
                        {
                            m.Result = (IntPtr)NativeConsts.HTBOTTOMLEFT;
                        }
                        else
                        {
                            m.Result = (IntPtr)NativeConsts.HTLEFT;
                        }
                    }
                    else if (pt.X >= w.Control.Width - Borders.Right)
                    {
                        if (pt.Y <= Borders.Top)
                        {
                            m.Result = (IntPtr)NativeConsts.HTTOPRIGHT;
                        }
                        else if (pt.Y >= w.Control.Height - Borders.Bottom)
                        {
                            m.Result = (IntPtr)NativeConsts.HTBOTTOMRIGHT;
                        }
                        else
                        {
                            m.Result = (IntPtr)NativeConsts.HTRIGHT;
                        }
                    }
                    else if (pt.Y <= Borders.Top)
                    {
                        m.Result = (IntPtr)NativeConsts.HTTOP;
                    }
                    else if (pt.Y >= w.Control.Height - Borders.Bottom)
                    {
                        m.Result = (IntPtr)NativeConsts.HTBOTTOM;
                    }
                    return true;
                })
            );
        }

        /// <summary>
        /// Create a resize handler that will fire after resizing has finished
        /// </summary>
        /// <param name="handler">The event to fire</param>
        /// <returns>A MessageHandler that wehn added to a WndProc handler will fire a resize end event</returns>
        public static MessageHandler CreateResizeEndEventHandler(EventHandler handler, int timeout = 50)
        {
            MessageHandler mhandler = new MessageHandler
            {
                Message = 0x05
            };

            mhandler.Handler = new MessageHandlerProc(delegate(WndProcHandler w, ref Message m)
            {
                if (mhandler.Tag != null && !((DelayedInvokeState)mhandler.Tag).HasReturned)
                {
                    ((DelayedInvokeState)mhandler.Tag).Restart = true;
                }
                else
                {
                    mhandler.Tag = new Action<Control>((c) =>
                    {
                        try
                        {
                            handler(c, EventArgs.Empty);
                        }
                        catch (Exception)
                        {
                        }
                    }).DelayedInvoke(timeout, new[] { w.Control });
                }
                return false;
            });
            return mhandler;
        }

        public static MessageHandler CreateFlickerFreeHandler(Control control)
        {
            return new MessageHandler
          (
              NativeConsts.WM_ERASEBKGND,
              new MessageHandlerProc(delegate(WndProcHandler w, ref Message m)
              {
                  return true;
              }));
        }


        #endregion
    }
}
