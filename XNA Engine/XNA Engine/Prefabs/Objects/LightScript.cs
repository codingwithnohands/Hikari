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
    class LightScript : Component
    {
        private Color[] _lights = new Color[4];

        public LightScript(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            _lights[0] = new Color(60, 60, 60, 40);
            _lights[1] = new Color(34, 12, 0, 50);
            _lights[2] = new Color(140, 70, 45, 70);
            _lights[3] = new Color(0, 0, 0, 130);

            gameObject.AddComponent<LightTexture>(new LightTexture(objMan,
                id,
                new Rectangle(0, 0, flatRender.DeviceManager.PreferredBackBufferWidth, flatRender.DeviceManager.PreferredBackBufferHeight),
                Color.Black));
            gameObject.AddComponent<Transform>(new Transform(_objectManager, Id));
            gameObject.transform.layer = 2;
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            GameObject Char = _objectManager.GetObjectByName("Character");
            if (Char != null)
            {
                float pos = 0f;
                Color left = Color.Transparent;
                Color right = Color.Transparent;
                for(int i = 0; i < 3; i++)
                {
                    pos = -4032f + ((i + 1) * 510);
                    if (Char.transform.position.X <= pos)
                    {
                        left = _lights[i];
                        right = _lights[i + 1];
                        pos = (500 - (pos - Char.transform.position.X)) / 500;
                        break;
                    }
                }
                
                (gameObject.GetComponent<LightTexture>() as LightTexture).Color = Color.Lerp(left, right, pos);
            }
            base.FixedUpdate(Delta, game);
        }
    }
}
