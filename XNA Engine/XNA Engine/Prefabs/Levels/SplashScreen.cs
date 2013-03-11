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
using hikari_game.Prefabs.Objects;

namespace hikari_game.Prefabs.Levels
{
    class SplashScreen : Level
    {

        #region Fields

        private int _state = 0;
        private float _elapsed = 0f;

        #endregion

        public SplashScreen(LevelManager lvlMan, string name)
            : base(lvlMan, name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            GameObject obj;
            obj = _objMan.CreateObject("Chicken", "animation");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, Vector3.Zero, new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/Splashscreen/LameChicken_idle"), 4));
            obj.AddComponent<Animation>(new Animation(_objMan, obj.Id));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Level/Splashscreen/LameChicken_idle"), "idle", new Rectangle(0, 0, 300, 200));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Level/Splashscreen/LameChicken01"), "walk", new Rectangle(0, 0, 170, 131));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Level/Splashscreen/LameChicken02"), "walk2", new Rectangle(0, 0, 170, 131));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Level/Splashscreen/LameChicken_framework"), "idle2", new Rectangle(0, 0, 300, 300));
            (obj.GetComponent<Animation>() as Animation).SetAnimation("idle");
            (obj.GetComponent<Animation>() as Animation).FPS = 12;

            obj = _objMan.CreateObject("light", "blendover");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, Vector3.Zero, new Vector3(1f, 1f,1f), Vector3.Zero, 1 ));
            obj.AddComponent<LightTexture>(new LightTexture(_objMan,
                obj.Id,
                new Rectangle(0, 0, flatRender.DeviceManager.PreferredBackBufferWidth, flatRender.DeviceManager.PreferredBackBufferHeight),
                Color.Black));
            (obj.GetComponent<LightTexture>() as LightTexture).BlendTo(new Color(0, 0, 0, 0), 1f);

            obj = _objMan.CreateObject("mainCamera", "camera");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(0f, 0f, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, false, false, 0, false, Vector3.Zero));
            obj.AddComponent<Camera>(new Camera(_objMan, obj.Id));
        }

        public override void Update(float Delta, Game game)
        {
            _elapsed += Delta;
            switch(_state)
            {
                case 0:
                    //4 seconds chicken walk
                    _objMan.GetObjectByName("Chicken").transform.position = Vector3.Lerp(new Vector3(128f + 40f, 0f, 0f), Vector3.Zero, _elapsed / 4f);
                    if(_elapsed >= 1f)
                    {
                        _objMan.GetObjectByName("Chicken").animation.SetAnimation("walk2");
                        _state = 1;
                    }
                    break;
                case 1:
                    //4 seconds chicken walk
                    _objMan.GetObjectByName("Chicken").transform.position = Vector3.Lerp(new Vector3(128f + 40f, 0f, 0f), Vector3.Zero, _elapsed / 4f);
                    if(_elapsed >= 2f)
                    {
                        _objMan.GetObjectByName("Chicken").animation.SetAnimation("walk1");
                        _state = 2;
                    }
                    break;
                case 2:
                    //4 seconds chicken walk
                    _objMan.GetObjectByName("Chicken").transform.position = Vector3.Lerp(new Vector3(128f + 40f, 0f, 0f), Vector3.Zero, _elapsed / 4f);
                    if(_elapsed >= 3f && _elapsed <= 3.1f)
                    {
                        _objMan.GetObjectByName("Chicken").animation.SetAnimation("walk2");
                    }
                    else if(_elapsed >= 4f)
                    {
                        _state = 3;
                        _elapsed = 0f;
                    }
                    break;
                case 3:
                    //blend over 1 sec
                    if (_elapsed <= 0.1f)
                    {
                        _objMan.GetObjectByName("Chicken").animation.SetAnimation("idle");
                        _objMan.GetObjectByName("Chicken").transform.scale = (new Vector3(0.8f, 0.8f, 0.8f));
                        (_objMan.GetObjectByName("light").GetComponent<LightTexture>() as LightTexture).BlendTo(new Color(230, 230, 230, 0), 0.6f);
                    }
                    else if (_elapsed >= 0.6f && _elapsed <= 0.61f)
                    {
                        _objMan.GetObjectByName("Chicken").animation.SetAnimation("idle2");
                        _objMan.GetObjectByName("Chicken").transform.scale = (new Vector3(0.6f, 0.6f, 0.6f));
                        (_objMan.GetObjectByName("light").GetComponent<LightTexture>() as LightTexture).BlendTo(new Color(0, 0, 0, 0), 1f);
                    }
                    //play audio
                    if(_elapsed >= 2f)
                    {
                        _elapsed = 0f;
                        _state = 4;
                    }
                    break;
                case 4:
                    //wait 3 secs
                    if(_elapsed > 4f)
                    {
                        (_objMan.GetObjectByName("light").GetComponent<LightTexture>() as LightTexture).BlendTo(Color.Black, 2f);
                        _elapsed = 0f;
                        _state = 5;
                    }
                    break;
                case 5:
                    //levelchange
                    if(_elapsed >= 2f)
                        LevelManager.Main.requestLevelReplace("mainMenu");
                    break;
            }
            base.Update(Delta, game);
        }
    }
}
