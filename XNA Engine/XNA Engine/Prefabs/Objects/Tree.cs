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
    class Tree : Component
    {
        float _elapsed = 0f;
        float _duration = 3f;
        bool _fall = false;

        public Tree(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            gameObject.transform.position = new Vector3(-4032f + 10 * 128f, 50f, 0.5f);
        }

        public void Fall()
        {
            _fall = true;
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            if (_fall)
            {
                if (_elapsed < _duration)
                {
                    _elapsed += Delta;

                    gameObject.transform.position = Vector3.Lerp(new Vector3(-4032f + 10 * 128f, 50f, 0.5f), new Vector3(-4032f + 10 * 128f + 25f, 6f, 0.5f), _elapsed / _duration);
                    gameObject.transform.rotation = Vector3.Lerp(Vector3.Zero, new Vector3(0f, 0f, -MathHelper.Pi * 0.5f), _elapsed / _duration);
                }
            }
            base.FixedUpdate(Delta, game);
        }
    }
}
