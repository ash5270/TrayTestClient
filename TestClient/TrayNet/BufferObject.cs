using System;
using System.Net.Sockets;

namespace tray
{
    namespace net
    {
        public class BufferObject
        {
            public BufferWrite bufferWrite;
            public BufferRead bufferRead;
            
            private MemoryStream memoryStream;
            private Socket? socket;

            public BufferObject()
            {
                socket = null;  
                memoryStream = new MemoryStream();
                bufferWrite =  new BufferWrite(memoryStream);
                bufferRead = new BufferRead(memoryStream);  
            }
            
            public byte[] GetBuffer()
            {
                return memoryStream.GetBuffer();
            }

            public long BufferSize()
            {
                return memoryStream.Length;
            }
        }
    }
}
