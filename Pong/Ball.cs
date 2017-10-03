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
    public class Ball : Sprite
    {
        private Vector2 m_DirectionVector;
        private readonly DirectionsUnit r_DirectionsUnit;
        private bool m_IsGoal;
        private readonly float r_BallInitialSpeed;
        private readonly float r_BallSpeedOffsetOnCollision;

        /// <summary>
        /// Constructs a new Ball in a random vector direction.
        /// </summary>
        /// <param name="i_Sprite">Sprite Ball</param>
        public Ball(Sprite i_Sprite) : base(i_Sprite)
        {
            r_DirectionsUnit = new DirectionsUnit();
            m_DirectionVector = r_DirectionsUnit.DirectionVector;
            r_BallInitialSpeed = this.Speed;
            r_BallSpeedOffsetOnCollision = 0;
        }

        /// <summary>
        /// Constructs a new Ball in a random vector direction containing speed raising value on sprites collision.
        /// </summary>
        /// <param name="i_Sprite">Sprite Ball</param>
        /// <param name="i_BallSpeedOffsetOnCollision">float speed offset.</param>
        public Ball(Sprite i_Sprite, float i_BallSpeedOffsetOnCollision) : base(i_Sprite)
        {
            r_DirectionsUnit = new DirectionsUnit();
            m_DirectionVector = r_DirectionsUnit.DirectionVector;
            r_BallInitialSpeed = this.Speed;
            r_BallSpeedOffsetOnCollision = i_BallSpeedOffsetOnCollision;
        }

        /// <summary>
        /// Constructs a new Ball in the specified direction.
        /// </summary>
        /// <param name="i_Sprite">Sprite Ball</param>
        /// <param name="i_DirectionIndex">int DirectionIndex (Range: 0 to 4).</param>
        public Ball(Sprite i_Sprite, int i_DirectionIndex) : base(i_Sprite)
        {
            r_DirectionsUnit = new DirectionsUnit(i_DirectionIndex);
            m_DirectionVector = r_DirectionsUnit.DirectionVector;
            r_BallInitialSpeed = this.Speed;
            r_BallSpeedOffsetOnCollision = 0;
        }

        /// <summary>
        /// Constructs a new Ball in the specified direction containing speed raising value on sprites collision.
        /// </summary>
        /// <param name="i_Sprite">Sprite Ball</param>
        /// <param name="i_DirectionIndex">int DirectionIndex (Range: 0 to 4).</param>
        /// <param name="i_BallSpeedOffset">float speed offset.</param>
        public Ball(Sprite i_Sprite, int i_DirectionIndex, float i_BallSpeedOffsetOnCollision) : base(i_Sprite)
        {
            r_DirectionsUnit = new DirectionsUnit(i_DirectionIndex);
            m_DirectionVector = r_DirectionsUnit.DirectionVector;
            r_BallInitialSpeed = this.Speed;
            r_BallSpeedOffsetOnCollision = i_BallSpeedOffsetOnCollision;
            Reset();
        }

        private void Reset()
        {
            m_DirectionVector = r_DirectionsUnit.DirectionVector;
            m_IsGoal = false;
        }

        public override void Initialize()
        {
            Reset();
        }

        public bool IsGoal
        {
            get { return m_IsGoal; }
        }

        private Vector2 HandleOnWallCollision(GraphicsDevice i_Graphics)
        {
            Rectangle selfCollisionRectangle = this.CollisionRectangle;
            bool v_IsLeftRightWallDetected = selfCollisionRectangle.X == i_Graphics.Viewport.X || selfCollisionRectangle.X == i_Graphics.Viewport.Width - selfCollisionRectangle.Width;
            bool v_IsCeilingFloorDetected = selfCollisionRectangle.Y == i_Graphics.Viewport.Y || selfCollisionRectangle.Y == i_Graphics.Viewport.Height - selfCollisionRectangle.Height;
            Vector2 newPosition;

            m_DirectionVector.X *= v_IsLeftRightWallDetected ? -1 : 1;
            m_DirectionVector.Y *= v_IsCeilingFloorDetected ? -1 : 1;
            newPosition = new Vector2(this.Position.X + (m_DirectionVector.X * this.Speed), this.Position.Y + (m_DirectionVector.Y * this.Speed));

            m_IsGoal = v_IsLeftRightWallDetected ? true : false;

            return newPosition;
        }

        private void RaiseSpeedOnSpriteCollision(Sprite i_Sprite)
        {
            this.Speed += r_BallSpeedOffsetOnCollision;
        }

        private void HandlingOnSpriteCollision(List<Sprite> i_Sprites)
        {
            foreach (Sprite sprite in i_Sprites)
            {
                if (sprite != this)
                {
                    eAxis detectedCollisionAxis = this.DetectCollisionAxis(sprite);
                    if (detectedCollisionAxis.HasFlag(eAxis.X))
                    {
                        m_DirectionVector.X = -m_DirectionVector.X;
                    }
                    if (detectedCollisionAxis.HasFlag(eAxis.Y))
                    {
                        m_DirectionVector.Y = -m_DirectionVector.Y;
                    }

                    if (detectedCollisionAxis > eAxis.None)
                    {
                        RaiseSpeedOnSpriteCollision(sprite);
                    }
                }
            }
        }

        private void Move(GraphicsDevice i_Graphics, List<Sprite> i_Sprites, Vector2 i_NewPosition)
        {
            Vector2 newPosition;

            newPosition.X = MathHelper.Clamp(i_NewPosition.X, i_Graphics.Viewport.X, i_Graphics.Viewport.Width - this.Texture.Width);
            newPosition.Y = MathHelper.Clamp(i_NewPosition.Y, i_Graphics.Viewport.Y, i_Graphics.Viewport.Height - this.Texture.Height);
            this.Position = newPosition;
        }

        public override void Update(GameTime i_GameTime, GraphicsDevice i_Graphics, List<Sprite> i_Sprites)
        {
            Vector2 newPosition;

            HandlingOnSpriteCollision(i_Sprites);
            newPosition = HandleOnWallCollision(i_Graphics);
            Move(i_Graphics, i_Sprites, newPosition);
        }
    }
}
