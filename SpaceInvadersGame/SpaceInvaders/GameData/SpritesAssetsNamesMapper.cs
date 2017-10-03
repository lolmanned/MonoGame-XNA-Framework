using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.GameData
{
    /// <summary>
    /// Holds the names of eLevelEntities assets names to be loaded by content manager.
    /// </summary>
    public static class SpritesAssetsNamesMapper
    {
        private const int k_AlternatingTexturesPerEnemyCount = 2;
        private const string k_FontsFolderName = @"Fonts\";
        public const string k_ConsolasFont = k_FontsFolderName + @"Consolas";
        private const string k_SpritesFolderName = @"Sprites\";
        public const string k_FirstShipAssetName = k_SpritesFolderName + @"Ship01_32x32";
        public const string k_SecondShipAssetName = k_SpritesFolderName + @"Ship02_32x32";
        public const string k_MotherShipAssetName = k_SpritesFolderName + @"MotherShip_32x120";
        private const string k_PinkEnemyFirstAssetName = k_SpritesFolderName + @"Enemy0101_32x32";
        private const string k_PinkEnemySecondAssetName = k_SpritesFolderName + @"Enemy0102_32x32";
        private const string k_BlueEnemyFirstAssetName = k_SpritesFolderName + @"Enemy0201_32x32";
        private const string k_BlueEnemySecondAssetName = k_SpritesFolderName + @"Enemy0202_32x32";
        private const string k_YellowEnemyFirstAssetName = k_SpritesFolderName + @"Enemy0301_32x32";
        private const string k_YellowEnemySecondAssetName = k_SpritesFolderName + @"Enemy0302_32x32";
        public const string k_BulletAssetName = k_SpritesFolderName + @"Bullet";
        public const string k_BarrierAssetName = k_SpritesFolderName + @"Barrier_44x32";
        public const string k_BackgroundAssetName = k_SpritesFolderName + @"BG_Space01_1024x768";

        /// <summary>
        /// Maps an enemy color to its asset name.
        /// </summary>
        private static Dictionary<Color, string[]> s_EnemyColorToAssetNames = new Dictionary<Color, string[]>()
        {
            { Color.LightBlue, new string[] { k_BlueEnemyFirstAssetName, k_BlueEnemySecondAssetName } },
            { Color.LightPink, new string[] { k_PinkEnemyFirstAssetName, k_PinkEnemySecondAssetName } },
            { Color.LightYellow, new string[] { k_YellowEnemyFirstAssetName, k_YellowEnemySecondAssetName } }
        };

        public static string[] EnemyAssetNames(Color i_EnemyColor)
        {
            bool isEntityExists = s_EnemyColorToAssetNames.ContainsKey(i_EnemyColor);
            string[] assetNames = new string[k_AlternatingTexturesPerEnemyCount];

            if (isEntityExists)
            {
                assetNames = s_EnemyColorToAssetNames[i_EnemyColor];
            }
            else
            {
                string msg = string.Format("There is no such enemy which color is {0}", i_EnemyColor);
                throw new Exception(msg);
            }

            return assetNames;
        }
    }
}
