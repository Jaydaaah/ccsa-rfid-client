using ccsa_rfid_client.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccsa_rfid_client.Watchers
{
    internal class ScreenWatcher : WatcherBase
    {
        public ScreenWatcher()
        {
            Thread = new Thread(ThreadStartFunc);
        }

        override internal void ThreadStartFunc()
        {
            while (IsAlive)
            {
                Thread.Sleep(1000);
                if (!ShouldScan)
                {
                    var screenshot = TesseractAction.TakeScreenshot();
                    if (screenshot != null)
                    {
                        var read = TesseractAction.readFromImage(screenshot);
                    }
                }
            }
        }
    }
}
