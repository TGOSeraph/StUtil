using StUtil.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public class PipeWriter : DisposableBase
    {
        public PipeWriter()
        {
                //var server = new NamedPipeServerStream("PipesOfPiece");
                //server.WaitForConnection();
                //StreamReader reader = new StreamReader(server);
                //StreamWriter writer = new StreamWriter(server);
                //while (true)
                //{
                //    var line = reader.ReadLine();
                //    writer.WriteLine(String.Join("", line.Reverse()));
                //    writer.Flush();
                //}
            String[] listOfPipes = System.IO.Directory.GetFiles(@"\\.\pipe\");

            try
            {
                var client = new NamedPipeClientStream("PipesOfPiece");
                client.Connect();
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }
    }
}
