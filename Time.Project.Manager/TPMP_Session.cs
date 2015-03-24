using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time.Project.Manager
{
    class TPMP_Session
    {
        bool finished = false;
        bool started = false;
        public DateTime startTime;
        public DateTime endTime;
        public DateTime pauseTime;
        public TimeSpan elapsedTime;
        TimeSpan pausedTime;

        public Globals.Session_Status status;

        int effectivity_rating;
        int progress_rating;

        public TPMP_Session()
        {
            status = Globals.Session_Status.awaiting;
        }

        public void Clear()
        {
            finished = false;
            started = false;
            elapsedTime = TimeSpan.FromSeconds(0);
            pausedTime = TimeSpan.FromSeconds(0);
            progress_rating = 0;
            effectivity_rating = 0;
        }
        public void StartNew()
        {
            startTime = DateTime.Now;
            started = true;
            status = Globals.Session_Status.started;
        }

        public void Stop()
        {
            endTime = DateTime.Now;
            status = Globals.Session_Status.finished;
        }

        public void Pause()
        {
            pauseTime = DateTime.Now;
            status = Globals.Session_Status.paused;
        }
        public void Unpause()
        {
            pausedTime += DateTime.Now - pauseTime;
            status = Globals.Session_Status.started;
        }
        public void CalculateElapsedTime()
        {
            if (status == Globals.Session_Status.started) elapsedTime = DateTime.Now - startTime - pausedTime;
            else if (status == Globals.Session_Status.finished) elapsedTime = endTime - startTime - pausedTime;
        }
        public string ElapsedTime()
        {
            CalculateElapsedTime();
            if (status == Globals.Session_Status.started || status == Globals.Session_Status.finished || status == Globals.Session_Status.paused)
            {
                string elapsed = "";
                elapsed = Functions.TimeToTextTimeFormat(elapsedTime);
                return elapsed;
            }
            else return "0:0";
          
        }

        public bool is_started()
        {
            return started;
        }
    }
}
