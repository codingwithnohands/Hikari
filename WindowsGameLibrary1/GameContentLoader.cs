using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LameChicken
{
    public static class GameContent
    {
        private static ContentManager _conMan;

        public static T Load<T>(string name)
        {
            return _conMan.Load<T>(name);
        }

        public static void Initialize(ContentManager conMan)
        {
            _conMan = conMan;
        }
    }
}
