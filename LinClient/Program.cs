using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace LinClient
{
    class Program
    {
        static void Main(string[] args)
        {

            string strHostName = new String("");
            if (args.Length == 0)
            {
                // Getting Ip address of local machine...

                // First get the host name of local machine.

                strHostName = System.Net.Dns.GetHostName();
                Console.WriteLine("Local Machine's Host Name: " + strHostName);
            }
            else
            {
                strHostName = args[0];
            }

            // Then using host name, get the IP address list..

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            string addr="255.255.255.255";
            foreach (var address in ipEntry.AddressList)
            {
                if (IPAddress.Parse(address.ToString()).AddressFamily == AddressFamily.InterNetwork)
                {
                    addr = address.ToString();
                    break;
                }
            }
            


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Connect("192.168.1.58", $"Hello I'm Device 1 from IP:{addr}");
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Connect("0.0.0.0", $"Hello I'm Device 2 from IP:{addr}");
            }).Start();
            Console.ReadLine();
        }
        static void Connect(String server, String message)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                int count = 0;
                while (count++ < 30000)
                {
                    // Translate the Message into ASCII.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);
                    // Bytes Array to receive Server Response.
                    data = new Byte[256];
                    String response = String.Empty;
                    // Read the Tcp Server Response Bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", response);
                    Thread.Sleep(2000);
                }
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            Console.Read();
        }
    }
}