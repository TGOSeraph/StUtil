using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Components
{
    public abstract class HostedComponent : Component
    {
        private ContainerControl parentControl;

        [DefaultValue(null), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ContainerControl ContainerControl
        {
            [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
            [UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
            get
            {
                return parentControl;
            }
            set
            {
                if (parentControl != value)
                {
                    parentControl = value;
                    OnInitialise();
                }
            }
        }

        public override ISite Site
        {
            set
            {
                base.Site = value;
                if (value == null)
                    return;

                IDesignerHost host = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (host != null)
                {
                    IComponent baseComp = host.RootComponent;

                    if (baseComp is ContainerControl)
                    {
                        this.ContainerControl = (ContainerControl)baseComp;
                    }
                }
            }
        }

        public HostedComponent()
        {
        }

        public HostedComponent(ContainerControl parentControl)
            : this()
        {
            this.parentControl = parentControl;
        }

        public HostedComponent(IContainer container)
            : this()
        {
            if (container == null)
            {
                return;
            }

            container.Add(this);
        }

        protected virtual void OnInitialise()
        {
            if (ContainerControl.IsHandleCreated)
            {
                OnControlCreated();
            }
            else
            {
                ContainerControl.HandleCreated += ContainerControl_HandleCreated;
            }
        }

        void ContainerControl_HandleCreated(object sender, EventArgs e)
        {
            OnControlCreated();
        }

        protected virtual void OnControlCreated()
        {
        }
    }
}
