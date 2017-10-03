using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class InputHandler
    {
        private readonly Dictionary<string, Keys> r_KeysDict;
        
        public InputHandler(Dictionary<string, Keys> i_KeysDict)
        {
            r_KeysDict = i_KeysDict;
        }

        public Keys Key(string i_KeyNickame)
        {
            return r_KeysDict[i_KeyNickame];
        }
    }
}
