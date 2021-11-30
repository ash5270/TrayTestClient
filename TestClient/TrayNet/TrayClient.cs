using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace tray.net
{
    public class TrayClient
    {
        private Socket socket;
        private MutexQueue<BufferObject> readBuffers;
        private IPEndPoint endPoint;
        private Session session;

        private Thread thread;

        public bool IsConnected;

        public TrayClient(string ip, int port)
        {
            Console.WriteLine("Create Client");
            endPoint=new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);


            readBuffers = new MutexQueue<BufferObject>();   
            socket.NoDelay= true;

        }

        public void Start()
        {
            socket.Connect(endPoint);
            session = new Session(socket, readBuffers);

            IsConnected = true;
            thread = new Thread(session.Dispatch);
            thread.Start();
        }

        public void Update()
        {
            while (readBuffers.Count() > 0)
            {
                var data = readBuffers.Dequeue();
                OnMessage(data);
            }
        }

        public void DisConnect()
        {
            IsConnected = false;
            session.DisConnect();   
            session.isThreadStop = true;
            thread.Join();
        }

        public void OnMessage(BufferObject obj)
        {

        }

        public void SendData(BufferObject buffer)
        {
            session.SendData(buffer);
        }
    }
}
