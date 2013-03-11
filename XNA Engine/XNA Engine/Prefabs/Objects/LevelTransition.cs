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
    public class LevelTransition : Component
    {
        private string _level;
        private bool _blend;
        private float _duration = 0f;
        private float _elapsed = 0f;
        private bool _start = false;

        public LevelTransition(GameObjectManager objMan, int id, string level, bool createBlend)
            : base(objMan, id)
        {
            _level = level;
            _blend = createBlend;
            if (createBlend)
                _duration = 4f;
        }

        public void startTransition()
        {
            _elapsed = 0f;
            _start = true;
            if (_blend)
            {
                gameObject.AddComponent<LightTexture>(new LightTexture(_objectManager, 
                Id, 
                new Rectangle(0, 0, flatRender.DeviceManager.PreferredBackBufferWidth, flatRender.DeviceManager.PreferredBackBufferHeight), 
                Color.Transparent));
                (gameObject.GetComponent<LightTexture>() as LightTexture).BlendTo(Color.Black, _duration);
            }

        }

        public override void Update(float Delta, Game game)
        {
            if (_start)
            {
                _elapsed += Delta;
                if (_elapsed >= _duration)
                    LevelManager.Main.requestLevelLoad(_level);
            }
            base.Update(Delta, game);
        }

    }
}
