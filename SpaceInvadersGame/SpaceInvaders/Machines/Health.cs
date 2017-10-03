using Infrastructure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Machines
{
    /// <summary>
    /// Defining the number of remaining lives a living entity in the game has left.
    /// </summary>
    internal class Health
    {
        private readonly int r_InitialSouls;
        private int m_SoulsCounter;
        private bool m_IsGodModOn = false;

        /// <summary>
        /// </summary>
        /// <param name="i_Lives">The number of initial lives a living entity object has.</param>
        public Health(int i_Souls)
        {
            if (i_Souls > 0)
            {
                r_InitialSouls = i_Souls;
                m_SoulsCounter = i_Souls;
            }
            else
            {
                throw new Exception("Initial Souls value must be a positive integer.");
            }
        }

        public void ResetSouls()
        {
            m_SoulsCounter = r_InitialSouls;
        }

        public void EarnSoul()
        {
            m_SoulsCounter++;
        }

        public void LooseSoul()
        {
            bool isAnySoulsLeft = m_SoulsCounter > 0;
            bool isGodModOff = !m_IsGodModOn;
            bool isLoosingSoulValid = isAnySoulsLeft && isGodModOff;

            if (isLoosingSoulValid)
            {
                m_SoulsCounter--;
            }
            else
            {
                if (!isAnySoulsLeft)
                {
                    throw new Exception("No more souls left.");
                }
                else
                {
                    throw new Exception("Cannot loose a soul on God Mod");
                }
            }
        }

        public int Souls
        {
            get { return m_SoulsCounter; }
        }

        public bool GodMod
        {
            get { return m_IsGodModOn; }
        }

        public void TurnOnGodMod()
        {
            m_IsGodModOn = true;
        }

        public void TurnOffGodMod()
        {
            m_IsGodModOn = false;
        }
    }
}
