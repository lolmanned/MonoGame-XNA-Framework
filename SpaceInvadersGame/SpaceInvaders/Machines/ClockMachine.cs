using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Machines
{
    internal class ClockMachine
    {
        private static readonly Random sr_Random = new Random();
        private double m_AlarmTimeInSeconds;
        private double m_ElapsedTime = 0;
        private bool m_IsAlarmTimeReached = false;

        /// <summary>
        /// Constructs a new ClockMachine with the specified alarm time in seconds.
        /// </summary>
        /// <param name="i_AlarmTimeInSeconds">Once the specified seconds elapses, IsClockTimeReached is being set to true.</param>
        public ClockMachine(double i_AlarmTimeInSeconds)
        {
            AlarmTimeInSeconds = i_AlarmTimeInSeconds;
        }

        /// <summary>
        /// Constructs a new ClockMachine with a random alarm in a given range according to the specified params.
        /// </summary>
        /// <param name="i_MinNumOfSeconds">Indicates the minimum number of seconds to be elapsed.</param>
        /// <param name="i_MaxNumOfSeconds">Indicates the maximum number of seconds to be elapsed.</param>
        public ClockMachine(int i_MinNumOfSeconds, int i_MaxNumOfSeconds)
        {
            SetRandomAlarmTime(i_MinNumOfSeconds, i_MaxNumOfSeconds);
        }

        /// <summary>
        /// Returns the current clock time in total seconds.
        /// </summary>
        public double ElapsedTime
        {
            get { return m_ElapsedTime; }
        }

        public bool IsClockTimeReached
        {
            get { return m_IsAlarmTimeReached; }
        }

        public double AlarmTimeInSeconds
        {
            get
            {
                return m_AlarmTimeInSeconds;
            }

            set
            {
                resetElapsedTimeClock();
                m_AlarmTimeInSeconds = value;
            }
        }

        private void resetElapsedTimeClock()
        {
            m_ElapsedTime = 0;
        }

        /// <summary>
        /// Adds to the clock timer the specified value, the timer will be reset once it reaches the clock time.
        /// </summary>
        /// <param name="i_NumOfSecondsToAddToTimer">The value to add to the timer clock.</param>
        public void IncrementClockTimer(double i_NumOfSecondsToAddToTimer)
        {
            m_ElapsedTime += i_NumOfSecondsToAddToTimer;
            if (m_ElapsedTime >= m_AlarmTimeInSeconds)
            {
                resetElapsedTimeClock();
                m_IsAlarmTimeReached = true;
            }
            else
            {
                m_IsAlarmTimeReached = false;
            }
        }

        /// <summary>
        /// Sets random time in seconds in a specified range which indicates when should the next alarm occur.
        /// </summary>
        /// <param name="i_MinNumOfSeconds">Indicates the minimum number of seconds to be elapsed.</param>
        /// <param name="i_MaxNumOfSeconds">Indicates the maximum number of seconds to be elapsed.</param>
        /// <returns></returns>
        public void SetRandomAlarmTime(int i_MinNumOfSeconds, int i_MaxNumOfSeconds)
        {
            int randomNumOfSecondsInRange = sr_Random.Next(i_MinNumOfSeconds, i_MaxNumOfSeconds);
            double randomDouble = sr_Random.NextDouble();

            resetElapsedTimeClock();
            m_AlarmTimeInSeconds = randomNumOfSecondsInRange + randomDouble;
        }
    }
}
