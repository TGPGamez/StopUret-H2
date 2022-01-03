using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StopUret_H2
{
    public class Timer
    {
        private double lastStartValue;
        public double FullTime { get; private set; }
        private Thread countdown;
        public TimeUpdateEvent TimeUpdate;
        public bool isStarted { get; private set; } = false;
        public bool isStopped { get; private set; } = false;
        private SoundPlayer sp = new SoundPlayer();

        public Timer(double fullTime, bool startNow, string soundPath = "./Wave/finishSound.wav")
        {
            sp.SoundLocation = soundPath;
            FullTime = fullTime;
            lastStartValue = fullTime;
            countdown = new Thread(Countdown);
            countdown.Name = "Countdown thread";
            countdown.Priority = ThreadPriority.Normal;
            if (startNow)
            {
                isStarted = true;
                countdown.Start();
            }
        }

        public Timer()
        {
            sp.SoundLocation = "./Wave/finishSound.wav";
            countdown = new Thread(Countdown);
            countdown.Name = "Countdown thread";
            countdown.Priority = ThreadPriority.Normal;
        }

        public void SetTime(double hours = 0, double minutes = 0, double seconds = 0)
        {
            double newTime = (hours * 3600) + (minutes * 60) + seconds;
            lastStartValue = newTime;
            FullTime = newTime;
        }

        /// <summary>
        /// Used to countdown every second and updates a TimeUpdateEvent
        /// </summary>
        private void Countdown()
        {
            //Checks if the thread is started
            while (isStarted)
            {
                //Checks if timer is stopped
                if (!isStopped)
                {
                    TimeUpdate?.Invoke(convertToTimeDisplay());
                    Thread.Sleep(1000);
                    //If timer is done, then play finish sound
                    if (FullTime == 0)
                    {
                        isStopped = true;
                        TimeUpdate?.Invoke(convertToTimeDisplay());
                        sp.PlaySync();
                    } 
                    else
                    {
                        FullTime--;
                    }
                }
            }
        }

        public void Stop()
        {
            if (!isStopped)
            {
                isStopped = true;
            }
        }

        public void Start()
        {
            if (!isStarted)
            {
                isStarted = true;
                countdown.Start();
            }
            else
            {
                isStopped = false;
            }
        }

        /// <summary>
        /// Used to convert fullTime into timespan format display
        /// </summary>
        /// <returns>Time in display form</returns>
        private string convertToTimeDisplay()
        {
            int hoursLeft = 0;
            int minutesLeft = 0;
            int fullTime = (int)FullTime;
            if (fullTime >= 3600)
            {
                //Find how many hours is left 
                //3600 is 1 hour in seconds
                //Alternative you could do modulus
                hoursLeft = fullTime / 3600;
                fullTime -= hoursLeft * 3600;
            }
            if (fullTime >= 60)
            {
                //Find how many minutes left
                //Alternative you could do modulus
                minutesLeft = fullTime / 60;
                fullTime -= minutesLeft * 60;
            }
            //Rest is seconds
            int secondsLeft = fullTime;
            
            return fullTime >= 0 && !isStopped
                ? $"{AddZeroToNumber(hoursLeft)}:{AddZeroToNumber(minutesLeft)}:{AddZeroToNumber(secondsLeft)}"
                : "Done";
        }

        /// <summary>
        /// Used to add a 0 infront of numb if under 10
        /// </summary>
        /// <param name="numb"></param>
        /// <returns></returns>
        private string AddZeroToNumber(int numb)
        {
            return numb > 9 ? "" + numb : "0" + numb;
        }

        /// <summary>
        /// Checks if inputted hours, minutes and seconds together
        /// is greater than current fullTime, in order to set new time
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public bool CheckForNewTime(double hours = 0, double minutes = 0, double seconds = 0)
        {
            if (FullTime <= 0 && isStopped)
            {
                return true;
            }
            double time = (hours * 3600) + (minutes * 60) + seconds;
            return lastStartValue != time;
        }
    }
}
