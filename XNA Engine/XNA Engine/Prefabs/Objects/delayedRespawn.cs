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
using LameChicken;

namespace hikari_game.Prefabs.Objects
{
    class DelayedRespawn : Component
    {

        private float _delay;
        private float _elapsed = 0f;

        public DelayedRespawn(GameObjectManager objMan, int id, float delay)
            : base(objMan, id)
        {
            _delay = delay;
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            _elapsed += Delta;
            if(_elapsed >= _delay)
                LevelManager.Main.requestLevelLoad(LevelManager.Main.Current.Name);
            base.FixedUpdate(Delta, game);
        }
    }
}
