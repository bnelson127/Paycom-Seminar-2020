using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           try
            {
                // set the TcpListener on port 13000
                int port = 9999;
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests
                server.Start();

                //Enter the listening loop
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread listenThread = new Thread( ()=>handleClient(client) );
                    listenThread.Start();

                    
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("Hit enter to continue...");
            Console.Read();
        }

        public static void handleClient(TcpClient client)
        {
            // Buffer for reading data
            byte[] bytes = new byte[1024];
            string data;
            Console.WriteLine("newThread");
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            i = stream.Read(bytes, 0, bytes.Length);

            while (i != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine(String.Format("Received: {0}", data));

                // Process the data sent by the client.
                data = data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine(String.Format("Sent: {0}", data));

                i = stream.Read(bytes, 0, bytes.Length);
            }

            // Shutdown and end connection
            client.Close();
        }
    }
}