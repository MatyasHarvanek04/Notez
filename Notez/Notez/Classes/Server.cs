using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTcp;

namespace BetterNotePad
{
    class Server
    {
        
        public Server()
        {
            if (Form1._Form1.Mode == 0)
            {
                SimpleTcpServer server;
                server = new SimpleTcpServer(GetLocalIPAddress(), 25565);
                server.Events.DataReceived += Events_DataReceived;
                server.Start();
            }
            if(Form1._Form1.Mode == 1)
            {
                SimpleTcpClient client = new SimpleTcpClient(GetLocalIPAddress(), 25565);
                client.Connect();
                client.Send(Environment.GetCommandLineArgs()[1]);
                
                Form1._Form1.Invoke((MethodInvoker)delegate
                {
                    Form1._Form1.Dispose();
                });
            }
        }

        private void Events_DataReceived(object sender, SimpleTcp.DataReceivedEventArgs e)
        {
            Form1._Form1.Invoke((MethodInvoker)delegate
            {
                TabManager.AddTab(2, Encoding.UTF8.GetString(e.Data));
            });

        }


       

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
