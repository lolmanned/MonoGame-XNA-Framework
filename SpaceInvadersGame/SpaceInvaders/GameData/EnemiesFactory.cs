using Microsoft.Xna.Framework;
using SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.GameData
{
    public static class EnemiesFactory
    {
        private static int getAssetStartingIndex(int i_EnemyLineIndex)
        {
            int index = i_EnemyLineIndex == 2 || i_EnemyLineIndex == 4 ? 1 : 0;

            return index;
        }

        public static ColoredEnemy CreateEnemy(Game i_Game, int i_EnemyLineIndex)
        {
            ColoredEnemy generatedEnemy;
            eLevelEntities enemyLineEntity = (eLevelEntities)LevelEntitiesMapper.LineIndexToLevelEnemy(i_EnemyLineIndex);
            Color enemyColor = (Color)LevelEntitiesMapper.EntityToColor(enemyLineEntity);
            string firstAssetName;
            int assetStartingIndex = getAssetStartingIndex(i_EnemyLineIndex);

            if (enemyColor == Color.LightPink)
            {
                firstAssetName = SpritesAssetsNamesMapper.EnemyAssetNames(Color.LightPink)[assetStartingIndex];
            }
            else if (enemyColor == Color.LightBlue)
            {
                firstAssetName = SpritesAssetsNamesMapper.EnemyAssetNames(Color.LightBlue)[assetStartingIndex];
            }
            else
            {
                firstAssetName = SpritesAssetsNamesMapper.EnemyAssetNames(Color.LightYellow)[assetStartingIndex];
            }

            generatedEnemy = new ColoredEnemy(i_Game, enemyColor, firstAssetName, assetStartingIndex);
            generatedEnemy.TintColor = enemyColor;
            generatedEnemy.Velocity = new Vector2(0, 0);

            return generatedEnemy;
        }
    }
}
