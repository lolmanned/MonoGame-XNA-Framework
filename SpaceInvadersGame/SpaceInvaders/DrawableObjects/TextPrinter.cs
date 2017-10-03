using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.DrawableObjects
{
    public class TextPrinter
    {
        private readonly Game r_Game;
        private SpriteBatch m_SpriteBatch;
        public string TextData { get; set; }
        public Vector2 TextPosition { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont SpriteTextFont { get; protected set; }

        public TextPrinter(Game i_Game)
        {
            r_Game = i_Game;
        }

        public void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(r_Game.GraphicsDevice);
            SpriteTextFont = r_Game.Content.Load<SpriteFont>(SpritesAssetsNamesMapper.k_ConsolasFont);
        }

        public void Print()
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(SpriteTextFont, TextData, TextPosition, TextColor);
            m_SpriteBatch.End();
        }
    }
}
