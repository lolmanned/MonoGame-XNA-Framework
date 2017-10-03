using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Sprite
    {
        private Texture2D m_Texture;
        private Vector2 m_Position;
        private float m_Speed;
        private Color m_Color;

        public Sprite()
        {
        }

        public Sprite(Sprite i_Sprite)
        {
            m_Texture = i_Sprite.Texture;
            m_Speed = i_Sprite.Speed;
            m_Position = i_Sprite.Position;
            m_Color = i_Sprite.Color;
        }

        public Sprite(Texture2D i_Texture)
        {
            m_Texture = i_Texture;
        }

        public Sprite(Texture2D i_Texture, Vector2 i_Position, float i_Speed, Color i_Color)
        {
            m_Texture = i_Texture;
            m_Position = i_Position;
            m_Speed = i_Speed;
            m_Color = i_Color;
        }

        // TO DO: figure out how to validate the data given to the setters methods.

        /// <summary>
        /// Detectes the collision axis with a specified sprite.
        /// </summary>
        /// <param name="i_Sprite"></param>
        /// <returns>The collision eAxis or null if no collision detected.</returns>
        public eAxis DetectCollisionAxis(Sprite i_Sprite)
        {
            CollisionManager selfCollisionRectangle = new CollisionManager(this);
            //eAxis? returnedAxis = null;
            eAxis returnedAxis = eAxis.None;

            if (selfCollisionRectangle.IsTouchingLeftwardSprite(i_Sprite.CollisionRectangle) ||
                selfCollisionRectangle.IsTouchingRightwardSprite(i_Sprite.CollisionRectangle))
            {
                returnedAxis |= eAxis.X;
            }

            if (selfCollisionRectangle.IsTouchingTopwardSprite(i_Sprite.CollisionRectangle) ||
                selfCollisionRectangle.IsTouchingDownwardSprite(i_Sprite.CollisionRectangle))
            {
                returnedAxis |= eAxis.Y;
            }

            return returnedAxis;
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)m_Position.X, (int)m_Position.Y, m_Texture.Width, m_Texture.Height);
            }
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public virtual void Update(GameTime i_GameTime, GraphicsDevice i_Graphics)
        {
        }

        public virtual void Update(GameTime i_GameTime, GraphicsDevice i_Graphics, List<Sprite> i_Sprites)
        {
        }

        public virtual void Draw(SpriteBatch i_SpriteBatch)
        {
            i_SpriteBatch.Draw(m_Texture, m_Position, m_Color);
        }

        public virtual void Initialize()
        {
        }
    }
}
