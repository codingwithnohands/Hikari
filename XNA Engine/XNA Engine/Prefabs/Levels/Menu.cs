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
using hikari_game.Prefabs.Menus;

namespace hikari_game.Prefabs.Levels
{
    class Menu : Level
    {
        public Menu(LevelManager lvlMan, string name)
            : base(lvlMan, name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            GameObject obj;
            obj = _objMan.CreateObject();
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id));
            obj.name = "Character";
            obj.transform.position += new Vector3(0f, 0f, 0.5f);
            obj.transform.scale = new Vector3(0.25f, 0.25f, 1f);
            obj.AddComponent<Temporary>(new Temporary(_objMan, obj.Id, 0.001f));

            obj = _objMan.CreateObject("mainMenuBg", "background");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, Vector3.Zero, new Vector3(1f, 1f, 1f), Vector3.Zero, 2));
            obj.AddComponent<Sprite>(new Sprite(_objMan, obj.Id, GameContent.Load<Texture2D>("bg"), 5));
            obj.sprite.Initialize();

            obj = _objMan.CreateObject("mainCamera", "camera");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id));
            obj.AddComponent<Camera>(new Camera(_objMan, obj.Id));

            obj = _objMan.CreateObject("mainMenu","menu");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(98, 40, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIGroup>(new MainMenu(_objMan, obj.Id));

            GameObject uititle = _objMan.CreateObject("titleTexture", "UItexture");
            uititle.AddComponent<Transform>(new Transform(_objMan, uititle.Id, new Vector3(50, 0, 0.8f), new Vector3(0.9f, 0.9f, 1f), Vector3.Zero, 1));
            uititle.AddComponent<UITexture>(new UITexture(_objMan, uititle.Id, GameContent.Load<Texture2D>("UI/Hikari"), 7 ,alignment.UpperCenter));

            obj = _objMan.CreateObject("gameTitle", "UIframe");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(0, 0, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIGroup>(new UIGroup(_objMan, obj.Id, new Rectangle(0, 0, 100, 100), alignment.UpperLeft, uititle));

            obj = _objMan.CreateObject("profileMenu", "menu");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(2, 40, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIGroup>(new ProfileMenu(_objMan, obj.Id));
            obj.DeleteObject();

            obj = _objMan.CreateObject("profileMenu", "menu");
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id, new Vector3(2, 40, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIGroup>(new newProfileMenu(_objMan, obj.Id, 1));
            obj.DeleteObject();

            obj = _objMan.CreateObject();
            obj.AddComponent<Transform>(new Transform(_objMan, obj.Id));
            obj.name = "menuCursor";
            obj.tag = "cursor";
            obj.AddComponent<UITexture>(new UITexture(_objMan, obj.Id, GameContent.Load<Texture2D>("cursor"), 20, alignment.UpperRight));
            obj.AddComponent<Cursor>(new Cursor(_objMan, obj.Id));
        }

        public override void Update(float Delta, Game game)
        {
            base.Update(Delta, game);
        }
    }
}
