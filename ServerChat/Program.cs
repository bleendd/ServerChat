using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using ServerChat;

namespace ConsoleApplication1
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();

            Console.WriteLine("U startua!");

            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                byte[] bitatPrej = new byte[4096];
                string dataPrejKlientit = null;

                NetworkStream networkStream = clientSocket.GetStream();
                int bytesRead = networkStream.Read(bitatPrej, 0, bitatPrej.Length);
                dataPrejKlientit = System.Text.Encoding.ASCII.GetString(bitatPrej, 0, bytesRead);
                dataPrejKlientit = dataPrejKlientit.Substring(0, dataPrejKlientit.IndexOf("$"));
                clientsList.Add(dataPrejKlientit, clientSocket);
                handleClient klienti = new handleClient();
                broadcast(dataPrejKlientit + " u lidh!", dataPrejKlientit, false);
                Console.WriteLine(dataPrejKlientit + " hyri ne chat!");
                klienti.startClient(clientSocket, dataPrejKlientit, clientsList);

            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        public static void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(uName + " thot: " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                }
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
    }




}
