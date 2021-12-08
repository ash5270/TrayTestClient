using System;
using tray.net;

using System.Text;

namespace tray
{
    internal class App
    {
        List<TrayClient> clients;   

        public void Start()
        {
            clients = new List<TrayClient>();  
            
            for(int i=0; i<1; i++)
            {
                clients.Add(new TrayClient("175.195.20.82", 3000));
                clients[i].Start();
            }
        }

        public void Update()
        {
            while (true)
            {
                for(int i=0; i<clients.Count; i++)
                {
                    clients[i].Update();
                }
                BufferObject obj = new BufferObject();
                var id = BitConverter.GetBytes((UInt16)1204);
                var size = BitConverter.GetBytes((UInt16)4);
                string msg = "asd";
                var data = Encoding.ASCII.GetBytes(msg);
                var stringSize = (UInt16)msg.Length;
                var msgbyte = BitConverter.GetBytes(stringSize);

                Console.WriteLine($"id: {1024}, size: {4}, data: {msg}, len: {stringSize}");

                obj.bufferWrite.Write(id);
                obj.bufferWrite.Write(size);
                obj.bufferWrite.Write(msgbyte);
                obj.bufferWrite.Write(data);

                Console.WriteLine(obj.BufferSize());

                for (int i = 0; i < clients.Count; i++) 
                {
                    clients[i].SendData(obj);
                }
                Thread.Sleep(1000);
            }
        }

        public void Exit()
        {

        }
    }
}
