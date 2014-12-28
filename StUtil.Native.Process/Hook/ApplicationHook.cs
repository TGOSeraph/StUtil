using StUtil.Native.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hook
{
    public abstract class ApplicationHook
    {
        public System.Diagnostics.Process Process { get; private set; }

        public ApplicationHook(System.Diagnostics.Process targetProcess)
        {
            this.Process = targetProcess;
        }

        public virtual void Hook()
        {
            Hook("");
        }

        public virtual void Hook(string args)
        {
            if (this.Process.Id == System.Diagnostics.Process.GetCurrentProcess().Id)
            {
                ApplyHook(args);
            }
            else
            {
                RemoteProcess p = new RemoteProcess(this.Process);
                p.Open();
                p.LoadDotNetModule(Assembly.GetExecutingAssembly().Location, typeof(ApplicationHook).FullName, "Implant", this.GetType().Assembly.Location + ";" + this.GetType().FullName + ";" + (args ?? ""));
            }
        }

        protected abstract void ApplyHook(string args);

        public static int Implant(string args)
        {
            string[] split = args.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Assembly asm = Assembly.LoadFrom(split[0]);
            Type t = asm.GetType(split[1]);
            ApplicationHook hook;
            if (split.Length > 2)
            {
                hook = (ApplicationHook)Activator.CreateInstance(t, System.Diagnostics.Process.GetCurrentProcess(), split.Skip(2));
            }
            else
            {
                hook = (ApplicationHook)Activator.CreateInstance(t, System.Diagnostics.Process.GetCurrentProcess());
            }
            hook.Hook();
            return 1;
        }
    }
}
