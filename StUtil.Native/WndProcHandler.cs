using System.Windows.Forms;

namespace StUtil.Native
{
    public class WndProcHandler : BaseWndProcHandler
    {
        public delegate void WndProcHandlerDelegate(ref Message message, out bool stopPropagation);

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        public WndProcHandlerDelegate Handler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WndProcHandler"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public WndProcHandler(WndProcHandlerDelegate handler)
        {
            this.Handler = handler;
        }

        protected override void HandleMessage(ref Message msg, out bool stopPropagation)
        {
            stopPropagation = false;
            if (Handler != null)
            {
                Handler(ref msg, out stopPropagation);
            }
        }
    }

    public abstract class BaseWndProcHandler
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the message id that this handler manages.
        /// </summary>
        /// <value>
        /// The message id that this handler manages.
        /// </value>
        public int? MessageId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWndProcHandler"/> class.
        /// </summary>
        public BaseWndProcHandler()
        {
            this.IsActive = true;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="stopPropagation">if set to <c>true</c> stop propagation of the message.</param>
        public virtual void Handle(ref Message msg, out bool stopPropagation)
        {
            stopPropagation = false;

            if (IsActive)
            {
                if (!MessageId.HasValue || MessageId.Value == msg.Msg)
                {
                    HandleMessage(ref msg, out stopPropagation);
                }
            }
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="msg">The message.</param>
        protected abstract void HandleMessage(ref Message msg, out bool stopPropagation);
    }
}