using System;
using System.Net.Sockets;

namespace tray.net
{
    public class Session
    {
        public bool isThreadStop;

        private Socket socket;
        private readonly int bufferSize =1024;

        private MutexQueue<BufferObject> sendBuffers;
        private MutexQueue<BufferObject> readBuffers;
        public Session(Socket socket,MutexQueue<BufferObject> readBuffer)
        {
            Console.WriteLine("Create Session");

            this.socket = socket;
            this.readBuffers = readBuffer;

            isThreadStop= false; 
            sendBuffers = new MutexQueue<BufferObject>();
        }

        public void SendData(byte[] data)
        {
            BufferObject obj=new BufferObject();
            obj.bufferWrite.Write(data);
            sendBuffers.Enqueue(obj);
        }

        public void SendData(BufferObject obj)
        {
            sendBuffers.Enqueue(obj);
        }

        public void DisConnect()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        public void Dispatch()
        {
            while (socket != null && !isThreadStop)
            {
                DispatchRead();
                DispatchSend();
                Thread.Sleep(5);
            }
        }

        private void DispatchSend()
        {
            try
            {
                if (socket.Poll(0, SelectMode.SelectWrite))
                {
                    if (!(sendBuffers.Count() == 0))
                    {
                        BufferObject buffer = sendBuffers.Dequeue();
                        socket.Send(buffer.GetBuffer());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DispatchRead()
        {
            try
            {
                while(socket.Poll(0, SelectMode.SelectRead))
                {
                    var buffer = new byte[bufferSize];
                    int received = socket.Receive(buffer);
                    if (received > 0)
                    {
                        BufferObject obj=new BufferObject();    
                        obj.bufferWrite.Write(buffer, received);
                        readBuffers.Enqueue(obj);
                    }
                    else
                    {
                        DisConnect();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
