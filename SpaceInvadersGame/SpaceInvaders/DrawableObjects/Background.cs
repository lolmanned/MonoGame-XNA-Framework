using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.DrawableObjects
{
    public class Background : Sprite
    {
        private const string k_BackgroundAssetName = SpritesAssetsNamesMapper.k_BackgroundAssetName;

        public Background(Game i_Game) : base(k_BackgroundAssetName, i_Game, int.MinValue)
        {
        }
    }
}
