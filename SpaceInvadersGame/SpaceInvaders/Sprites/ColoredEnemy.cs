using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Sprites
{
    public class ColoredEnemy : Enemy
    {
        private readonly Color r_Color;

        public ColoredEnemy(Game i_Game, Color i_EnemyColor, string i_FirstAssetName, int i_StartingAssetIndex) : base(i_Game, i_EnemyColor, i_FirstAssetName, i_StartingAssetIndex)
        {
            r_Color = i_EnemyColor;
        }

        public override void Initialize()
        {
            string[] texturesNamesPaths = SpritesAssetsNamesMapper.EnemyAssetNames(r_Color);

            m_AlternatingTextures[0] = LoadTexture(texturesNamesPaths[0]);
            m_AlternatingTextures[1] = LoadTexture(texturesNamesPaths[1]);
            base.Initialize();
        }
    }
}
