using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Helpers
{
    public static class ValidityHelper
    {
        public static void ThrowExcpetionIfObjectNotMatchingType(Type i_Type, object i_Object)
        {
            bool v_IsObjectOfSpecifiedType = IsOfType(i_Type, i_Object);

            if (!v_IsObjectOfSpecifiedType)
            {
                string errorMsg = string.Format("{0} is not of {1} type.", i_Object, i_Type);

                throw new ArgumentException(errorMsg);
            }
        }

        /// <summary>
        /// Determines whether a given object is of a specified type.
        /// </summary>
        /// <param name="i_Type">The type to test against.</param>
        /// <param name="i_Object">The object to test whether its of the specified type.</param>
        /// <returns></returns>
        public static bool IsOfType(Type i_Type, object i_Object)
        {
            Type objectType = i_Object.GetType();
            Type neededType = i_Type;

            return objectType == neededType;
        }

        /// <summary>
        /// Determines whether a given input is in range.
        /// </summary>
        /// <typeparam name="T">The type of the params.</typeparam>
        /// <param name="i_Input">Determines whether the input is in range.</param>
        /// <param name="i_MaxValue">The maximum value in the range.</param>
        /// <param name="i_MinValue">The minimum value in the range.</param>
        /// <returns></returns>
        public static bool IsInRange<T>(T i_Input, T i_MaxValue, T i_MinValue)
        {
            Comparer<T> comparerObj = Comparer<T>.Default;
            int inputVSMax = comparerObj.Compare(i_Input, i_MaxValue);
            int inputVSMin = comparerObj.Compare(i_Input, i_MinValue);
            bool v_BelowEqualMaxValue = inputVSMax <= 0;
            bool v_AboveEqualMinValue = inputVSMin >= 0;
            bool v_IsInRange = v_AboveEqualMinValue && v_BelowEqualMaxValue;

            return v_IsInRange;
        }
    }
}
