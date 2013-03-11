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
    class Level01 : Level
    {
        public Level01(LevelManager lvlMan, string name)
            : base(lvlMan, name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            GameObject obj;

            #region TrashObject
            obj = _objMan.CreateObject("trash", "trash");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3995f, 13f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Characters/jap_idle"), 4));
            obj.AddComponent<Enemy>(new Enemy(_objMan, obj.Id));
            obj.AddComponent<LevelTransition>(new LevelTransition(_objMan, obj.Id, "mainMenu", true));
            obj.AddComponent<Animation>(new Animation(_objMan, obj.Id));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/jap_walk"), "walk", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/jap_idle"), "idle", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).SetAnimation("idle");
            (obj.GetComponent<Animation>() as Animation).FPS = 15;
            obj.AddComponent<CharacterController>(new CharacterController(_objMan, obj.Id));
            obj.AddComponent<Audio>(new Audio(_objMan, obj.Id, "footstep", true, 0.5f));
            obj.AddComponent<CharacterMover>(new CharacterMover(_objMan, obj.Id,10f, 10f, 10f));
            obj.AddComponent<UIGroup>(new UIGroup(_objMan, obj.Id, new Rectangle(0, 0, 100, 100), alignment.UpperLeft, null));
            obj.AddComponent<UITexture>(new UITexture(_objMan, obj.Id, GameContent.Load<Texture2D>("UI/Xbox360_Button_X"), 6, alignment.LowerCenter));
            obj.AddComponent<LightTexture>(new LightTexture(_objMan,
                obj.Id,
                new Rectangle(0, 0, flatRender.DeviceManager.PreferredBackBufferWidth, flatRender.DeviceManager.PreferredBackBufferHeight),
                Color.Black));
            obj.RemoveComponent<LightTexture>();
            obj.RemoveComponent<CharacterController>();
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, true, 0, true, Vector3.Zero));
            obj.AddComponent<Emitter>(new Emitter(_objMan,
                obj.Id, GameContent.Load<Texture2D>("smoke"),
                Color.White,
                9,
                3,
                new Rectangle(0, 0, 20, 20),
                0.6f,
                10f,
                Vector3.Down,
                0.0f,
                true,
                false,
                5,
                (1f / 3f) * 9f));
            obj.AddComponent<Temporary>(new Temporary(_objMan, obj.Id, 0.001f));
            #endregion

            #region Player

            obj = _objMan.CreateObject();
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3995f, 10f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.name = "Character";
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Characters/jap_idle"), 4));
            obj.AddComponent<Animation>(new Animation(_objMan, obj.Id));
            obj.AddComponent<CharacterMover>(new CharacterMover(_objMan, obj.Id, 120f, 220f, 750f));
            obj.AddComponent<Audio>(new Audio(_objMan, obj.Id, "footstep", true, 0.5f));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/jap_walk"), "walk", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/jap_idle"), "idle", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).SetAnimation("idle");
            (obj.GetComponent<Animation>() as Animation).FPS = 15;
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, true, 10, true, Vector3.Zero));
            obj.AddComponent<Player>(new Player(_objMan, obj.Id));
            obj.sprite.Initialize();

            #endregion

            #region background

            for (int i = 0; i < 8192f / 128f; i++)
            {
                obj = _objMan.CreateObject("bg" + i.ToString(), "background");
                obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + (i * 640f), 0f, 0.99f), new Vector3(5f, 5f, 5f), Vector3.Zero, 2));
                obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("bg"), 15));
                obj.sprite.Initialize();
            }

            #endregion

            #region cam and stuff

            obj = _objMan.CreateObject("lightScript", "script");
            obj.AddComponent<LightScript>(new LightScript(_objMan, obj.Id));

            obj = _objMan.CreateObject("mainCamera", "camera");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3995f, 13f, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, false, false, 0, false, Vector3.Zero));
            obj.AddComponent<Camera>(new Camera(_objMan, obj.Id));

            #endregion

            #region Checkpoints

            obj = _objMan.CreateObject("checkpoint01", "checkpoint");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3995f, 12f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Checkpoint>(new Checkpoint1(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-10, -10, 20, 20)));

            obj = _objMan.CreateObject("checkpoint02", "checkpoint");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4000f + 4*128f, 12f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Checkpoint>(new Checkpoint2(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-10, -10, 20, 20)));

            obj = _objMan.CreateObject("checkpoint03", "checkpoint");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 7 * 128f + 95f, +28f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Checkpoint>(new Checkpoint3(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-10, -10, 20, 20)));

            obj = _objMan.CreateObject("checkpoint04", "checkpoint");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-2785, 28f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Checkpoint>(new Checkpoint4(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-10, -10, 20, 20)));

            #endregion

            #region KillAreas

            obj = _objMan.CreateObject("killArea01", "killArea");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 4*128f, -80, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<KillArea>(new KillArea(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-150, -15, 300, 30)));

            obj = _objMan.CreateObject("killArea02", "killArea");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 96f + 9 * 128f, -80, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<KillArea>(new KillArea(_objMan, obj.Id));
            obj.AddComponent<Trigger>(new Trigger(_objMan, obj.Id, new Rectangle(-150, -15, 300, 30)));

            obj = _objMan.CreateObject("levelEnd", "levelEnd");
            obj.AddComponent<LevelEnd>(new LevelEnd(_objMan, obj.Id));

            #endregion

            #region interactiveobjects

            #region ambient

            obj = _objMan.CreateObject("waterfall", "emitter");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4000f + 4 * 128f + 85f, 80f, 0.85f), new Vector3(0.2f, 0.2f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Emitter>(new Emitter(_objMan,
                obj.Id, GameContent.Load<Texture2D>("smoke"),
                Color.PowderBlue,
                9,
                1,
                new Rectangle(0, 0, 20, 20),
                0.6f,
                10f,
                Vector3.Down,
                0.8f,
                true,
                false,
                300,
                (1f / 1f) * 9f));
            obj.AddComponent<Audio>(new Audio(_objMan, obj.Id, "ambient", false, 0f));
            obj.audio.Play = true;

            #endregion

            obj = _objMan.CreateObject("boulder", "interactiveObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3980f, 13f, 0.5f), new Vector3(0.2f, 0.2f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("boulder"), 5));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, true, 0, true, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("rock_01", "blockage");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-3980f +128f, 0f, 0.5f), new Vector3(0.45f, 0.45f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/rock_02"), 10));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("artifact", "interactiveObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 7 * 128f + 101f, 26f, 0.55f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("artifact"), 10));
            obj.AddComponent<Artifact>(new Artifact(_objMan, obj.Id));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("tree", "interactiveObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 10 * 128f, 50f, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("tree"), 10));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.AddComponent<Tree>(new Tree(_objMan, obj.Id));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("shelf", "interactiveObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 11 * 128f - 45f, 35f, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("shelf"), 10));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("boulder02", "interactiveObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 11 * 128f - 45f, 50f, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("boulder"), 10));
            obj.AddComponent<Boulder>(new Boulder(_objMan, obj.Id));
            obj.sprite.Initialize();

            #endregion

            #region foreground

            obj = _objMan.CreateObject("bg_00_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_00_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_00_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_00_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_01_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_01_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_01_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_01_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_02_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_02_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_02_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_03_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_03_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_03_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_03_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_03_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_03_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_04_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_04_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("FG_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 0));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/FG_05_01"), 4));
            obj.sprite.Initialize();

            #endregion

            #region background objects

            obj = _objMan.CreateObject("mountain01", "bgObject");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 128f, +32f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/mountain"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_00_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_00_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_00_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_00_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_00_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_00_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_01_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_01_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_01_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_01_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_01_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_01_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_02_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_02_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_02_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_02_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_03_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_03_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_03_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_03_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_03_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_03_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_05_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_05_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_05_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_05_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_06_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_06_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_06_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_06_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_06_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_06_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_07_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_07_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_07_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_07_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_07_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_07_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_08_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_08_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_08_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_08_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_09_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_09_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_09_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_09_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_09_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_09_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_10_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_10_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_10_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_10_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_11_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_11_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_11_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_11_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_11_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_11_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_12_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_12_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_12_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_12_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("bg_12_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/BG_12_02"), 4));
            obj.sprite.Initialize();

            #endregion

            #region base Texture way back

            obj = _objMan.CreateObject("base_back_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_05_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 128f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_05_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_05_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, -128f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_05_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_06_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 128f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_06_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_06_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 0f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_06_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_back_06_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, -128f, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_back_06_02"), 4));
            obj.sprite.Initialize();

            #endregion

            #region base tex int back

            obj = _objMan.CreateObject("base_00_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_00_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_00_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_00_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_00_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_00_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_01_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_01_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_01_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_01_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_01_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_01_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_02_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_02_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_02_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_02_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_03_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_03_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_03_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_03_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_03_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_03_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_05_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_05_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_06_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_06_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_06_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_06_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_06_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_06_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_07_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_07_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_07_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_07_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_07_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_07_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_08_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_08_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_08_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_08_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_08_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_08_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_09_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_09_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_09_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_09_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_09_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_09_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_10_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_10_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_10_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_10_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_10_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_10_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_11_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_11_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_11_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_11_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_11_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_11_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_12_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_12_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_12_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_12_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_12_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_12_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_13_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1536f, 128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_13_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_13_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1536f, 0f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_13_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_13_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1536f, -128f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_base_13_02"), 4));
            obj.sprite.Initialize();

            #endregion

            #region base tex front

            obj = _objMan.CreateObject("base_front_00_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_00_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_00_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f - 128f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_00_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_01_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_01_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_01_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_01_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_02_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_02_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_02_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_03_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_03_00"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_03_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_03_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_03_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_03_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_04_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_04_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_05_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_06_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_06_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_07_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_07_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_07_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_07_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_08_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_08_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_08_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_08_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_09_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_09_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_09_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_09_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_10_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_10_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_12_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_12_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_12_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_12_02"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_13_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1536f, 0f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_13_01"), 4));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("base_front_13_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1536f, -128f, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_int_fore_13_02"), 4));
            obj.sprite.Initialize();

            #endregion

            #region LevelBaseColliders

            obj = _objMan.CreateObject("ground_00_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f -128f, 128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_00_00"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_01_00", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_01_00"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_01_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_01_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_02_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 128f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_02_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_03_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 256f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_03_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_04_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 384f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_04_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_05_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_05_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_05_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 512f, -128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_05_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_06_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_06_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_06_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 640f, -128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_06_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_07_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 768f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_07_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_08_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 896f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_08_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_09_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1024f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_09_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_10_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_10_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_10_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1152f, -128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_10_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_11_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_11_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_11_02", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1280f, -128f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_11_02"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("ground_12_01", "baseCollider");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(-4032f + 64f + 1408f, 0f, 0.98f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("Level/01/Level01_12_01"), 4));
            obj.AddComponent<Physic>(new Physic(_objMan, obj.Id, true, false, 0f, new Vector3(1f, 1f, 1f), false, Vector3.Zero));
            obj.sprite.Initialize();

            List<Component> comps = ObjectManager.GetComponents<Checkpoint>();
            foreach (Component comp in comps)
            {
                if ((comp as Checkpoint).CheckpointID == ProfileManager.CurrentProfile.Checkpoint)
                    (comp as Checkpoint).LoadCheckpoint();
            }
            #endregion
        }

        public override void Update(float Delta, Game game)
        {

            if (InputHandler.KeyDown(Keys.Escape) || InputHandler.GButtonDown("start"))
                _lvlMan.requestLevelSwitch("mainMenu");

            base.Update(Delta, game);

        }
    }
}
