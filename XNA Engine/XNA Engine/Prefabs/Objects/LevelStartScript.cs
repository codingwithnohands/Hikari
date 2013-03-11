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
    class LevelStartScript : Component
    {
        private float _duration;
        private float _elapsed = 0f;

        public LevelStartScript(GameObjectManager objMan, int id, float fadeTime)
            : base(objMan, id)
        {
            gameObject.AddComponent<LightTexture>(new LightTexture(objMan, 
                id, 
                new Rectangle(0, 0, flatRender.DeviceManager.PreferredBackBufferWidth, flatRender.DeviceManager.PreferredBackBufferHeight), 
                Color.Black));
            (gameObject.GetComponent<LightTexture>() as LightTexture).BlendTo(new Color(0, 0, 0, 0), fadeTime);
            _duration = fadeTime;
        }

        public override void Update(float Delta, Game game)
        {
            _elapsed += Delta;
            if (_elapsed >= _duration)
            {
                GameObject obj = _objectManager.GetObjectByName("Character");
                obj.AddComponent<CharacterController>(new CharacterController(_objectManager, obj.Id));
                gameObject.RemoveComponent<LevelStartScript>();
            }
            base.Update(Delta, game);
        }
    }
}
