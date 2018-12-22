using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Lab1_client
{
    class Client
    {
        static int port = 8080;
        static string address = "127.0.0.1";
        static Socket socket;
        static bool work = true;
        static System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(GetMessage));
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                thread.Start();
                byte[] data = new byte[256];
                int bytes = 0;
                while (work)
                {
                    string message = null;
                    bytes = socket.Receive(data, data.Length, 0);
                    message = (Encoding.UTF8.GetString(data, 0, bytes));
                    Print("Ответ сервера: " + message);
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public static void GetMessage()
        {
            string message = null;
            while (work)
            {
                Print("Введите сообщение: ");
                message = Console.ReadLine();
                Console.WriteLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);
                if (message == "close" || message == "exit") work = false;
            }
        }

        private static void Print(string str)
        {
            Console.WriteLine(str);
        }
    }
}

