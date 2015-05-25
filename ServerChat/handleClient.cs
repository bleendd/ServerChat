using ConsoleApplication1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChat
{
    public class handleClient
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;
        bool bClosed = false;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            byte[] bytesFrom = new byte[4096];
            string dataFromClient = null;

            while (clientSocket.Connected)
            {
                NetworkStream networkStream = clientSocket.GetStream();
                try
                {
                    networkStream.Read(bytesFrom, 0, 4096);
                }
                catch ( Exception ex)
                {
                    Console.WriteLine( clNo + " u shkeput nga lidhja!");
                    Program.clientsList.Remove(clNo);
                    break;
                }
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                Console.WriteLine("Nga - " + clNo + ": " + dataFromClient);

                Program.broadcast(dataFromClient, clNo, true);

            }
        }

    }
}
