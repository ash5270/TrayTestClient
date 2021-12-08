using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace tray.net
{
    public enum PacketID: UInt16
    {
        message=0,
    }

    public class TrayClient
    {
        private Socket socket;
        private MutexQueue<BufferObject> readBuffers;
        private IPEndPoint endPoint;
        private Session session;

        private Thread sesstionThread;

        public bool IsConnected;

        public TrayClient(string ip, int port)
        {
            //Console.WriteLine("Create Client");
            endPoint=new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            readBuffers = new MutexQueue<BufferObject>();   
            socket.NoDelay= true;

        }

        public void Start()
        {
            socket.Connect(endPoint);
            session = new Session(socket, readBuffers);

            sesstionThread = new Thread(session.Dispatch);
            sesstionThread.Start();
            IsConnected = true;
        }

        public void Update()
        {
            while (readBuffers.Count() > 0 && IsConnected)
            {
                var data = readBuffers.Dequeue();
                OnMessage(data);
            }
        }

        public void DisConnect()
        {
            IsConnected = false;
            session.isThreadStop = true;
            sesstionThread.Join();
        }

        public void OnMessage(BufferObject obj)
        {
            var id= BitConverter.ToInt16(obj.GetBuffer());
            var size = BitConverter.ToInt16(obj.GetBuffer(), sizeof(UInt16));
            var data = BitConverter.ToInt16(obj.GetBuffer(),sizeof(UInt16)+sizeof(UInt16)); 
            Console.WriteLine($"{id} : {size} : {data}");
        }

        public void SendData(BufferObject buffer)
        {
            if(IsConnected)
            {
                session.SendData(buffer);
            }
        }
    }
}
