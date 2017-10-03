using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Input
    {
        private readonly Keys r_Up;
        private readonly Keys r_Down;

        public Input(Keys i_Up, Keys i_Down)
        {
            r_Up = i_Up;
            r_Down = i_Down;
        }

        public Keys Up
        {
            get { return r_Up; }
        }

        public Keys Down
        {
            get { return r_Down; }
        }
    }
}
