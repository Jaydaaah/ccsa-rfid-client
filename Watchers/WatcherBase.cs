using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ccsa_rfid_client.Watchers
{
    abstract internal class WatcherBase
    {
        protected Thread? Thread { get; set; }

        internal bool IsAlive { get; set; } = false;
        internal bool ShouldScan { get; set; } = false;

        abstract internal void ThreadStartFunc();

        virtual internal void ThreadStart()
        {
            IsAlive = true;
            if (Thread != null)
            {
                Thread.Start();
            }
            else
            {
                throw new NotImplementedException("Please provide Thread");
            }
        }

        internal void StartScan() => ShouldScan = true;

        internal void StopScan() => ShouldScan = false;
    }
}
