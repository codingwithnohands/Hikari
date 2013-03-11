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

namespace hikari_game.Prefabs.Menus
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
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 11.5f, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/uibutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.Yellow, Color.LightSlateGray));

            obj = objMan.GetObjectByName("continueText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 12f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Continue", GameContent.Load<SpriteFont>("test"), Color.Black, 100, 20, alignment.Center));

            obj = objMan.GetObjectByName("newGameButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 36.5f, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/uibutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.Yellow, Color.LightSlateGray));

            obj = objMan.GetObjectByName("newGameText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 37f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "New Game", GameContent.Load<SpriteFont>("test"), Color.Black, 100, 20, alignment.Center));

            obj = objMan.GetObjectByName("profileButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 61.5f, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/uibutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.Yellow, Color.LightSlateGray));

            obj = objMan.GetObjectByName("profileText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 62f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Profiles", GameContent.Load<SpriteFont>("test"), Color.Black, 100, 20, alignment.Center));

            obj = objMan.GetObjectByName("exitButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 86.5f, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/uibutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.Yellow, Color.LightSlateGray));

            obj = objMan.GetObjectByName("exitText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 87f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Exit", GameContent.Load<SpriteFont>("test"), Color.Black, 100, 20, alignment.Center));

            UpdateElementGroups();
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            if (InputHandler.MButtonUp("LMB") || InputHandler.GButtonUp("A"))
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
                                if (ProfileManager.CurrentProfile != null)
                                {
                                    ProfileManager.CurrentProfile.Checkpoint = 1;
                                    LevelManager.Main.requestLevelLoad("Level01");
                                }
                                break;
                            case "continueButton":
                                if (LevelManager.Main.GameRunning)
                                    LevelManager.Main.requestSwitchToPrev();
                                else if (ProfileManager.CurrentProfile != null)
                                {
                                    if (ProfileManager.CurrentProfile.Checkpoint <= 4)
                                        LevelManager.Main.requestLevelLoad("Level01");
                                }
                                else
                                {

                                }
                                break;
                            case "profileButton":
                                if (_objectManager.GetObjectByName("profileMenu") != null)
                                {
                                    _objectManager.DeleteObject(_objectManager.GetObjectByName("profileMenu"));
                                }
                                else
                                {
                                    GameObject obj = _objectManager.CreateObject("profileMenu", "menu");
                                    obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(2, 40, 0.2f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
                                    obj.AddComponent<UIGroup>(new ProfileMenu(_objectManager, obj.Id));
                                }
                                break;
                            default:
                                break;
                        }
                        pair.Value.State = UIButtonState.inactive;
                    }
                }
            }
            base.Update(Delta, game);
        }

        #endregion
    }
}