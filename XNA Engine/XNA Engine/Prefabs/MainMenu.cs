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

namespace hikari_game.Prefabs
{
    public class MainMenu : UIMenu
    {
        #region Constructor

        public MainMenu(GameObjectManager objMan, int id)
            : base(objMan, id, new Rectangle(0, 0, 40, 60), alignment.UpperRight,
            objMan.CreateObject("continueButton", "button"),
            objMan.CreateObject("newGameButton", "button"),
            objMan.CreateObject("profileButton", "button"),
            objMan.CreateObject("exitButton", "button"),
            objMan.CreateObject("continueText", "buttonText"),
            objMan.CreateObject("newGameText", "buttonText"),
            objMan.CreateObject("profileText", "buttonText"),
            objMan.CreateObject("exitText", "buttonText"))
        {
            GameObject obj = objMan.GetObjectByName("continueButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 0, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 100, 20), Color.Black, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.LightSlateGray, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("continueText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 0, 0.2f), new Vector3(14f, 14f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Continue", GameContent.Load<SpriteFont>("test"), Color.White, 100, 20, alignment.UpperCenter));

            obj = objMan.GetObjectByName("newGameButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 25, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 100, 20), Color.Black, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.LightSlateGray, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("newGameText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 25, 0.2f), new Vector3(14f, 14f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "New Game", GameContent.Load<SpriteFont>("test"), Color.White, 100, 20, alignment.UpperCenter));

            obj = objMan.GetObjectByName("profileButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 50, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 100, 20), Color.Black, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.LightSlateGray, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("profileText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 50, 0.2f), new Vector3(14f, 14f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Profiles", GameContent.Load<SpriteFont>("test"), Color.White, 100, 20, alignment.UpperCenter));

            obj = objMan.GetObjectByName("exitButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 75, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 100, 20), Color.Black, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.LightSlateGray, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("exitText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 75, 0.2f), new Vector3(14f, 14f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Exit", GameContent.Load<SpriteFont>("test"), Color.White, 100, 20, alignment.UpperCenter));

            UpdateElementGroups();
        }

        #endregion

        #region Methods

        public override void FixedUpdate(float Delta, Game game)
        {
            if (!InputHandler.MButtonPressed("LMB") || !InputHandler.GButtonPressed("A"))
            {
                foreach (var pair in _sorted)
                {
                    if (pair.Value.State == UIButtonState.pressed)
                    {
                        switch (pair.Value.gameObject.name)
                        {
                            case "exitButton":
                                game.Exit();
                                break;
                            case "newGameButton":
                                game.
                            default:
                                break;
                        }
                        pair.Value.State = UIButtonState.inactive;
                    }
                }
            }
            base.FixedUpdate(Delta, game);
        }

        #endregion
    }
}
