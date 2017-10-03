using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ObjectModel
{
    public class GameScreen : GameComponent
    {
        private List<IDrawable> r_DrawableComponents;
        private List<IUpdateable> r_UpdateableComponents;

        public GameScreen(Game i_Game) : base(i_Game)
        {
            r_DrawableComponents = new List<IDrawable>();
            r_UpdateableComponents = new List<IUpdateable>();
        }
    }
}
