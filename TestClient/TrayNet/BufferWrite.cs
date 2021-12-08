using System;
using System.Collections.Generic;


namespace tray.net
{
    public class BufferWrite
    {
        private MemoryStream memoryStream;

        public void Write(bool data)
        {
            var bytes = BitConverter.GetBytes(data);
            memoryStream.Write(bytes);
        }

       

        public void Write(byte[] buffer,int size)
        {
            int pos = (int)memoryStream.Position;
            memoryStream.Write(buffer, pos, size);
        }

        public void Write(byte[] buffer)
        {
            memoryStream.Write(buffer);
        }

        public BufferWrite(MemoryStream memoryStream)
        {
            this.memoryStream = memoryStream;
        }        
    }
}
