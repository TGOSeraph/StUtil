using System.Diagnostics;

namespace StUtil.Native.Monitor
{
    public class ProcessActivityData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool JustStarted { get; set; }
        public string ExecutablePath { get; set; }

        public ProcessActivityData(int id, string name, string executablePath)
        {
            this.ID = id;
            this.Name = name;
            this.ExecutablePath = executablePath;
        }

        public ProcessActivityData(int id, string name, bool justStarted, string executablePath)
            : this(id, name, executablePath)
        {
            this.JustStarted = justStarted;
        }

        public Process GetProcess()
        {
            return Process.GetProcessById(ID);
        }
    }
}
