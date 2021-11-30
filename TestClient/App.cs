using System;
using tray.net;

namespace tray
{
    internal class App
    {
        TrayClient trayClient;

        public void Start()
        {
            trayClient = new TrayClient("127.0.0.1",3000);
            trayClient.Start();
        }

        public void Update()
        {
            while (true)
            {
                trayClient.Update();

                BufferObject obj = new BufferObject();
                var data1 = BitConverter.GetBytes(12312312321321);

                obj.bufferWrite.Write(data1);
                trayClient.SendData(obj);
            }
        }

        public void Exit()
        {

        }
    }
}
