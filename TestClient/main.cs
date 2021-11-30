using System;

namespace tray
{
    public class Programe
    {
        public static void Main()
        {
            App app = new App();
            app.Start();

            while (true)
            {
                app.Update();
            }

            app.Exit(); 
        }
    }
}
