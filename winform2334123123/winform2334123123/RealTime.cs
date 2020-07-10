using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winform2334123123
{
    class RealTime
    {
        long totalMs1;
        long totalMs2;
        long duration;

        public void StartTime()
        {
            string time1hr = string.Format("{0:HH}", DateTime.Now);
            string time1mi = string.Format("{0:mm}", DateTime.Now);
            string time1se = string.Format("{0:ss}", DateTime.Now);
            string time1ms = string.Format("{0:fff}", DateTime.Now);

            totalMs1 = ToMs(time1hr, time1mi, time1se, time1ms);
        }
        public void EndTime()
        {
            string time2hr = string.Format("{0:HH}", DateTime.Now);
            string time2mi = string.Format("{0:mm}", DateTime.Now);
            string time2se = string.Format("{0:ss}", DateTime.Now);
            string time2ms = string.Format("{0:fff}", DateTime.Now);

            totalMs2 = ToMs(time2hr, time2mi, time2se, time2ms);
            TimeTaken(totalMs1, totalMs2);
        }
        public void ShowTime()
        {
            MessageBox.Show(Format(duration));
        }

        private long ToMs(string hr, string mi, string se, string ms)
        {
            long totalMs = 0;
            totalMs += (Convert.ToInt32(mi) + (Convert.ToInt32(hr) * 60)) * 60000;
            totalMs += Convert.ToInt32(se) * 1000;
            totalMs += Convert.ToInt32(ms);

            return totalMs;
        }
        private void TimeTaken(long startTime, long endTime)
        {
            duration = endTime - startTime;
        }
        private string Format(long duration)
        {
            int milliseconds = Convert.ToInt32(duration);

            int msR = milliseconds % 1000;
            int seconds = milliseconds / 1000;
            int secondsR = seconds % 60;
            int mins = seconds / 60;

            string strMs = msR.ToString();
            string strSeconds = secondsR.ToString();
            string strMinutes = mins.ToString();

            while (strMs.Length < 3)
            {
                strMs = "0" + strMs;
            }
            while (strSeconds.Length < 2)
            {
                strSeconds = "0" + strSeconds;
            }

            return $"{strMinutes}:{strSeconds}:{strMs}";
        }
    }
}
