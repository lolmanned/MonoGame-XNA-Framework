using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pong
{
    public class Paddle : Sprite
    {
        //private readonly Sprite r_Behavior;
        private readonly Input r_Inputs;

        public Input Input
        {
            get { return r_Inputs; }
        }

        /// <summary>
        /// Constructs a new paddle having left and right controllers.
        /// </summary>
        /// <param name="i_LeftController">The input key which moves the paddle to the left.</param>
        /// <param name="i_RightController">The input key which moves the paddle to the right.</param>
        public Paddle(Keys i_UpController, Keys i_DownController, Sprite i_Sprite) : base(i_Sprite)
        {
            r_Inputs = new Input(i_UpController, i_DownController);
        }

        private void Move(GraphicsDevice i_Graphics)
        {
            bool v_IsUpKeyPressed = Keyboard.GetState().IsKeyDown(r_Inputs.Up);
            bool v_IsDownKeyPressed = Keyboard.GetState().IsKeyDown(r_Inputs.Down);
            float newCoordY = this.Position.Y;

            newCoordY = v_IsUpKeyPressed ? newCoordY - this.Speed : newCoordY;
            newCoordY = v_IsDownKeyPressed ? newCoordY + this.Speed : newCoordY;
            newCoordY = MathHelper.Clamp(newCoordY, 0, i_Graphics.Viewport.Height - this.Texture.Height);

            this.Position = new Vector2(this.Position.X, newCoordY);
        }

        public override void Update(GameTime i_Game, GraphicsDevice i_Graphics, List<Sprite> i_Sprites)
        {
                Move(i_Graphics);
        }
    }
}
