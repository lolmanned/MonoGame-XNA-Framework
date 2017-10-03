using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    /// <summary>
    /// A class which provides methods to detect collisions with its sprite's representing rectangle.
    /// </summary>
    public class CollisionManager
    {
        private readonly Sprite r_Sprite;
        private readonly Rectangle r_SelfCollisionRectangle;
        private readonly float r_SelfSpeed;
           
        public CollisionManager(Sprite i_Sprite)
        {
            r_Sprite = i_Sprite;
            r_SelfCollisionRectangle = i_Sprite.CollisionRectangle;
            r_SelfSpeed = i_Sprite.Speed;
        }

        public bool IsIntersecting(Rectangle i_Rectangle)
        {
            return r_SelfCollisionRectangle.Intersects(i_Rectangle);
        }

        public bool IsTouchingRightwardSprite(Rectangle i_Rectangle)
        {
            return r_SelfCollisionRectangle.Left < i_Rectangle.Right &&
                   r_SelfCollisionRectangle.Right + r_SelfSpeed >= i_Rectangle.Left &&
                   r_SelfCollisionRectangle.Top < i_Rectangle.Bottom &&
                   r_SelfCollisionRectangle.Bottom > i_Rectangle.Top;
        }

        public bool IsTouchingLeftwardSprite(Rectangle i_Rectangle)
        {
            return r_SelfCollisionRectangle.Right > i_Rectangle.Left &&
                   r_SelfCollisionRectangle.Left + r_SelfSpeed < i_Rectangle.Right &&
                   r_SelfCollisionRectangle.Top < i_Rectangle.Bottom &&
                   r_SelfCollisionRectangle.Bottom > i_Rectangle.Top;
        }

        public bool IsTouchingDownwardSprite(Rectangle i_Rectangle)
        {
            return r_SelfCollisionRectangle.Top + r_SelfSpeed < i_Rectangle.Bottom &&
                   r_SelfCollisionRectangle.Left < i_Rectangle.Right &&
                   r_SelfCollisionRectangle.Right > i_Rectangle.Left &&
                   r_SelfCollisionRectangle.Bottom > i_Rectangle.Bottom;
        }

        public bool IsTouchingTopwardSprite(Rectangle i_Rectangle)
        {
            return r_SelfCollisionRectangle.Bottom + r_SelfSpeed >= i_Rectangle.Top &&
                   r_SelfCollisionRectangle.Top < i_Rectangle.Top &&
                   r_SelfCollisionRectangle.Right > i_Rectangle.Left &&
                   r_SelfCollisionRectangle.Left < i_Rectangle.Right;
        }
    }
}
