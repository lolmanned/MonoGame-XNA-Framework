using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class DirectionsUnit
    {
        private readonly Vector2 r_DirectionVector;
        private readonly Random r_RandomInstance;
        private const int k_FirstDirectionOption = 0;
        private const int k_TotalDirections = 4;

        /// <summary>
        /// Constructs a new DirectionsUnit instance containing a random direction vector.
        /// </summary>
        public DirectionsUnit()
        {
            r_RandomInstance = new Random();
            r_DirectionVector = GetDirectionVectorByIndex(r_RandomInstance.Next(k_FirstDirectionOption, k_TotalDirections));
        }

        /// <summary>
        /// Constructs a new DirectionsUnit instance containing the specified direction vector.
        /// </summary>
        /// <param name="i_DirectionIndex">0 for (1,1). 1 for (1,-1). 2 for (-1,1). 3 for (-1,-1).</param>
        public DirectionsUnit(int i_DirectionIndex)
        {
            if (i_DirectionIndex < 0 || i_DirectionIndex > 3)
            {
                throw new Exception("Value is out of range.");
            }
            else
            {
                r_DirectionVector = GetDirectionVectorByIndex(i_DirectionIndex);
            }
        }

        /// <summary>
        /// Returns the direction vector of this instance.
        /// </summary>
        public Vector2 DirectionVector
        {
            get { return r_DirectionVector; }
        }

        /// <summary>
        /// Returns a direction vector according to a specified index.
        /// </summary>
        /// <param name="i_Index">0 for (1,1). 1 for (1,-1). 2 for (-1,1). 3 for (-1,-1).</param>
        /// <returns></returns>
        private Vector2 GetDirectionVectorByIndex(int i_Index)
        {
            Vector2 returnedDirectionVector = new Vector2();

            switch (i_Index)
            {
                case 0:
                    returnedDirectionVector = new Vector2(1, 1);
                    break;

                case 1:
                    returnedDirectionVector = new Vector2(1, -1);
                    break;

                case 2:
                    returnedDirectionVector = new Vector2(-1, 1);
                    break;

                case 3:
                    returnedDirectionVector = new Vector2(-1, -1);
                    break;
            }

            return returnedDirectionVector;
        }
    }
}
