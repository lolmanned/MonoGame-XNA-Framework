using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class ScoreManager
    {
        private int m_Score;
        private SpriteFont m_Font;
        private Vector2 m_DrawingPosition;
        private Color m_Color;

        /// <summary>
        /// Constructs a new ScoreManager containing 0 score points. SpriteFont must be initialized manually.
        /// </summary>
        public ScoreManager()
        {
            Reset();
        }

        /// <summary>
        /// /// Constructs a new ScoreManager containing 0 score points and the specified drawing color. SpriteFont must be initialized manually.
        /// </summary>
        /// <param name="i_Color">Color font color.</param>
        public ScoreManager(Color i_Color)
        {
            m_Color = i_Color;
            Reset();
        }

        private void Reset()
        {
            m_Score = 0;
        }

        public void ResetScore()
        {
            m_Score = 0;
        }

        public void IncrementScore()
        {
            m_Score++;
        }

        /// <summary>
        /// The current score points.
        /// </summary>
        public int Score
        {
            get { return m_Score; }
        }

        public SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }

        public Vector2 DrawingPosition
        {
            get { return m_DrawingPosition; }
            set { m_DrawingPosition = value; }
        }


        /// <summary>
        /// Returns the drawing color. If not set, its default value is white.
        /// </summary>
        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public void Draw(SpriteBatch i_SpriteBatch)
        {
            i_SpriteBatch.DrawString(m_Font, m_Score.ToString(), m_DrawingPosition, m_Color);
        }
    }
}
