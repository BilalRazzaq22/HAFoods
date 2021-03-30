using System;
using System.Timers;
using System.Windows.Interop;

namespace PrizeBondChecker.WpfClient.Controls.Helpers
{
    public class AutoLogOffHelper
    {
        private static Timer _timer = null;
        private static int _logOffTime;

        public static int LogOffTime
        {
            get { return _logOffTime; }
            set { _logOffTime = (value <= 0 ? 10 : value); }
        }

        public delegate void MakeAutoLogOff();
        public static event MakeAutoLogOff MakeAutoLogOffEvent;

        public static void StartAutoLogoffOption()
        {
            ComponentDispatcher.ThreadIdle += new EventHandler(DispatcherQueueEmptyHandler);
        }

        private static void _timer_Tick(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                ComponentDispatcher.ThreadIdle -= new EventHandler(DispatcherQueueEmptyHandler);
                _timer.Stop();
                if (MakeAutoLogOffEvent != null)
                {
                    MakeAutoLogOffEvent();
                }
            }
        }

        private static void DispatcherQueueEmptyHandler(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = LogOffTime * 60 * 1000;
                _timer.Elapsed += _timer_Elapsed;
                //_timer.Elapsed += new EventHandler(_timer_Tick);
                _timer.Enabled = true;
            }
            else if (_timer.Enabled == false)
            {
                _timer.Enabled = true;
            }
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_timer != null)
            {
                ComponentDispatcher.ThreadIdle -= new EventHandler(DispatcherQueueEmptyHandler);
                _timer.Stop();
                if (MakeAutoLogOffEvent != null)
                {
                    MakeAutoLogOffEvent();
                }
            }
        }

        public static void ResetLogoffTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Enabled = true;
                _timer.Start();

            }
        }

    }
}
