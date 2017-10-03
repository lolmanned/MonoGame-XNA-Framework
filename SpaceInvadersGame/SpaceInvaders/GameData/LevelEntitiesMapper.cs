using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.GameData
{
    internal static class LevelEntitiesMapper
    {
        /// <summary>
        /// Maps line index in the enemies matrix to the enemy entity its inhabited by.
        /// </summary>
        private static Dictionary<int, eLevelEntities> s_ColoredEnemiesLinesIndexes = new Dictionary<int, eLevelEntities>()
        {
            { 0, eLevelEntities.PinkEnemy },
            { 1, eLevelEntities.BlueEnemy },
            { 2, eLevelEntities.BlueEnemy },
            { 3, eLevelEntities.YellowEnemy },
            { 4, eLevelEntities.YellowEnemy }
        };

        /// <summary>
        /// Maps enemy entitiy to its Color.
        /// </summary>
        private static Dictionary<eLevelEntities?, Color> s_EntitiesColors = new Dictionary<eLevelEntities?, Color>()
        {
            { eLevelEntities.PinkEnemy, Color.LightPink },
            { eLevelEntities.BlueEnemy, Color.LightBlue },
            { eLevelEntities.YellowEnemy, Color.LightYellow },
            { eLevelEntities.Background, Color.White },
            { eLevelEntities.ShipBullet, Color.Red },
            { eLevelEntities.EnemyBullet, Color.Blue },
            { eLevelEntities.MotherShip, Color.Red },
            { eLevelEntities.Ship, Color.White }
        };

        private static Dictionary<Color, eLevelEntitiesPoints> s_EnemyColorToEnemyEntityPoints = new Dictionary<Color, eLevelEntitiesPoints>()
        {
            { Color.LightPink, eLevelEntitiesPoints.PinkEnemy },
            { Color.LightBlue, eLevelEntitiesPoints.BlueEnemy },
            { Color.LightYellow, eLevelEntitiesPoints.YellowEnemy }
        };

        private static Dictionary<eLevelEntities?, eLevelEntitiesPoints> s_EntitiesToPoints = new Dictionary<eLevelEntities?, eLevelEntitiesPoints>()
        {
            { eLevelEntities.PinkEnemy, eLevelEntitiesPoints.PinkEnemy },
            { eLevelEntities.BlueEnemy, eLevelEntitiesPoints.BlueEnemy },
            { eLevelEntities.YellowEnemy, eLevelEntitiesPoints.YellowEnemy },
            { eLevelEntities.MotherShip, eLevelEntitiesPoints.MotherShip },
            { eLevelEntities.Ship, eLevelEntitiesPoints.Ship }
        };

        public static eLevelEntitiesPoints EnemyColorToLifeWorth(Color i_Color)
        {
            bool v_IsEnemyIndexFound = s_EnemyColorToEnemyEntityPoints.ContainsKey(i_Color);
            eLevelEntitiesPoints mappedEnemyPoints;

            if (v_IsEnemyIndexFound)
            {
                mappedEnemyPoints = s_EnemyColorToEnemyEntityPoints[i_Color];
            }
            else
            {
                string msg = string.Format("No such enemy which color is {0}", i_Color);
                throw new Exception(msg);
            }

            return mappedEnemyPoints;
        }

        public static eLevelEntitiesPoints? LevelEntityToPoints(eLevelEntities? i_Entity)
        {
            bool v_IsEnemyIndexFound = s_EntitiesToPoints.ContainsKey(i_Entity);
            eLevelEntitiesPoints? mappedEntityPoints = null;

            if (v_IsEnemyIndexFound)
            {
                mappedEntityPoints = s_EntitiesToPoints[i_Entity];
            }

            return mappedEntityPoints;
        }

        public static Color? EntityToColor(eLevelEntities? i_EnemyEntity)
        {
            bool v_IsEnemyIndexFound = s_EntitiesColors.ContainsKey(i_EnemyEntity);
            Color? mappedEnemyColor = null;

            if (v_IsEnemyIndexFound)
            {
                mappedEnemyColor = s_EntitiesColors[i_EnemyEntity];
            }

            return mappedEnemyColor;
        }

        public static eLevelEntities? LineIndexToLevelEnemy(int i_LineIndex)
        {
            bool v_IsLineIndexFound = s_ColoredEnemiesLinesIndexes.ContainsKey(i_LineIndex);
            eLevelEntities? mappedLevelEnemy = null;

            if (v_IsLineIndexFound)
            {
                mappedLevelEnemy = s_ColoredEnemiesLinesIndexes.ElementAt(i_LineIndex).Value;
            }

            return mappedLevelEnemy;
        }
    }
}
