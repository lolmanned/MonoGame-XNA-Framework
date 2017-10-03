using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.DrawableObjects
{
    class PlayerData : Sprite
    {
        private static int s_InstanceIndex;
        private static float s_LastInstanceDrawingHeight;
        private readonly int r_PlayerIndex;
        private readonly PlayerSpaceShip r_Player;
        private readonly TextPrinter r_ScorePrinter;
        private readonly Color r_TextColor;
        protected float m_PlayerDataPrintingHeight;

        public PlayerData(Game i_Game, PlayerSpaceShip i_Player, Color i_TextColor) : base(i_Player.AssetName, i_Game)
        {
            if (i_Player != null)
            {
                r_Player = i_Player;
                Texture = i_Player.Texture;
                r_ScorePrinter = new TextPrinter(i_Game);
                r_TextColor = i_TextColor;
                r_PlayerIndex = ++s_InstanceIndex;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        protected virtual void DrawSouls()
        {
            int soulsCount = r_Player.Souls;
            float playerWidth = r_Player.Texture.Width;
            float x = Game.GraphicsDevice.Viewport.Width - Width;
            float y = m_PlayerDataPrintingHeight;
            Vector2 drawPosition = new Vector2(x, y);

            for (int i = 0; i < soulsCount; i++)
            {
                m_SpriteBatch.Draw(Texture, drawPosition,
                 SourceRectangle, TintColor,
                Rotation, RotationOrigin, Scales,
                SpriteEffects.None, LayerDepth);

                x -= Width + (0.5f * Width);
                drawPosition = new Vector2(x, y);
            }
        }

        protected virtual void InitDrawingSettings()
        {
            Scales = r_Player.Scales / 2;
            Opacity = r_Player.Opacity / 2;
            TintColor = new Color(0.5f, 0.5f, 0.5f);
        }

        protected virtual void DrawScore()
        {
            r_ScorePrinter.TextData = string.Format("P{0} Score: {1}", r_PlayerIndex, r_Player.Points.ToString());
            r_ScorePrinter.Print();
        }

        protected virtual void InitPlayerDataPrintingHeight()
        {
            const float x = 0;

            m_PlayerDataPrintingHeight = s_LastInstanceDrawingHeight > 0 ? s_LastInstanceDrawingHeight : 0;
            s_LastInstanceDrawingHeight += r_Player.Height;
            r_ScorePrinter.TextPosition = new Vector2(x, m_PlayerDataPrintingHeight);
            r_ScorePrinter.TextColor = r_TextColor;
        }

        public override void Initialize()
        {
            base.Initialize();
            InitPlayerDataPrintingHeight();
            InitDrawingSettings();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            r_ScorePrinter.LoadContent();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            r_ScorePrinter.TextData = r_Player.Points.ToString();
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            DrawSouls();
            DrawScore();

            if (!UseSharedBatch)
            {
                m_SpriteBatch.End();
            }
        }
    }
}
