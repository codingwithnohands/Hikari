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

namespace hikari_game.Prefabs.Menus
{
    class newProfileMenu : UIMenu
    {
        #region Fields

        private int _targetProfile;
        private string _activePicture;
        private bool _textbox;

        #endregion

        #region Constructor

        public newProfileMenu(GameObjectManager objMan, int id, int profileId)
            : base(objMan, id, new Rectangle(0, 0, 50, 50), alignment.Center,
            objMan.CreateObject("newProfileTitle", "menuTitle"),
            objMan.CreateObject("newProfileBg", "menuBg"),
            objMan.CreateObject("nameField", "textbox"),
            objMan.CreateObject("textBoxBg", "button"),
            objMan.CreateObject("textBoxBorder", "solid"),
            objMan.CreateObject("okButton", "button"),
            objMan.CreateObject("cancelButton", "button"),
            objMan.CreateObject("pic1Button", "button"),
            objMan.CreateObject("pic2Button", "button"),
            objMan.CreateObject("okText", "buttonText"),
            objMan.CreateObject("cancelText", "buttonText"))
        {
            _targetProfile = profileId;

            GameObject obj = objMan.GetObjectByName("newProfileTitle");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 5, 0.8f), new Vector3(28f, 28f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Create New Profile", GameContent.Load<SpriteFont>("test"), Color.White, 90, 30, alignment.UpperCenter));

            obj = objMan.GetObjectByName("newProfileBg");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(0, 0, 0.99f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 100, 100), new Color(25, 25, 25, 230), alignment.UpperLeft));

            obj = objMan.GetObjectByName("okButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(95, 95, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 30, 10), Color.Black, alignment.LowerRight));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.Black, Color.DeepSkyBlue, Color.LightSkyBlue));

            obj = objMan.GetObjectByName("cancelButton");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(5, 90f, 0.3f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 30, 10), Color.Black, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.Black, Color.DeepSkyBlue, Color.LightSkyBlue));

            obj = objMan.GetObjectByName("textBoxBorder");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50f, 25f, 0.71f), new Vector3(1.025f, 1.15f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 40, 10), Color.Black, alignment.Center));

            obj = objMan.GetObjectByName("textBoxBg");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50f, 20f, 0.7f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Solid>(new Solid(objMan, obj.Id, new Rectangle(0, 0, 40, 10), Color.Black, alignment.UpperCenter));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, new Color(80, 80, 80, 150), Color.LightSteelBlue, Color.LightSkyBlue));

            obj = objMan.GetObjectByName("nameField");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(33f, 25f, 0.6f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Name", GameContent.Load<SpriteFont>("test"), Color.White, 40, 14, alignment.CenterLeft));

            obj = objMan.GetObjectByName("pic1Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(25, 40f, 0.6f), new Vector3(0.65f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilePic01"), 5, alignment.UpperCenter));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.SlateGray, Color.LightSteelBlue, Color.White));

            obj = objMan.GetObjectByName("pic2Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(75, 57f, 0.6f), new Vector3(0.65f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilePic02"), 5, alignment.Center));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.SlateGray, Color.LightSteelBlue, Color.White));

            obj = objMan.GetObjectByName("okText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(93f, 95f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "OK", GameContent.Load<SpriteFont>("test"), Color.White, 30, 10, alignment.LowerRight));

            obj = objMan.GetObjectByName("cancelText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(7, 95f, 0.2f), new Vector3(23f, 23f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Cancel", GameContent.Load<SpriteFont>("test"), Color.White, 30, 10, alignment.LowerLeft));

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
                            case "cancelButton":
                                _textbox = false;
                                gameObject.DeleteObject();
                                break;
                            case "okButton":
                                if(_activePicture != null)
                                {
                                    if(_activePicture == "pic1Button")
                                        _activePicture = "UI/Profile/profilePic01";
                                    else if(_activePicture == "pic2Button")
                                        _activePicture = "UI/Profile/profilePic02";
                                    if(ProfileManager.addProfile(_targetProfile, findElementByName("nameField").uitext.Text, _activePicture))
                                    {
                                        ProfileManager.SetActive(_targetProfile);
                                        ProfileManager.saveProfile(_targetProfile);
                                        gameObject.DeleteObject();
                                    }
                                }
                                _textbox = false;
                                break;
                            case "textBoxBg":
                                _textbox = true;
                                break;
                            case "pic1Button":
                                _activePicture = pair.Value.gameObject.name;
                                _textbox = false;
                                break;
                            case "pic2Button":
                                _activePicture = pair.Value.gameObject.name;
                                _textbox = false;
                                break;
                            default:
                                break;
                        }
                        pair.Value.State = UIButtonState.inactive;
                    }
                }
            }
            base.Update(Delta, game);
            if (findElementByName("pic1Button").name == _activePicture)
            {
                if (findElementByName("pic2Button").uitexture.Color == Color.White)
                    findElementByName("pic2Button").uitexture.Color = Color.SlateGray;
                if (findElementByName("pic1Button").uitexture.Color == Color.SlateGray)
                    findElementByName("pic1Button").uitexture.Color = Color.White;
            }
            else if (findElementByName("pic2Button").name == _activePicture)
            {
                if (findElementByName("pic1Button").uitexture.Color == Color.White)
                    findElementByName("pic1Button").uitexture.Color = Color.SlateGray;
                if (findElementByName("pic2Button").uitexture.Color == Color.SlateGray)
                    findElementByName("pic2Button").uitexture.Color = Color.White;
            }
            if (_textbox)
            {
                if (InputHandler.KeyDown(Keys.Back))
                   findElementByName("nameField").uitext.RemoveLast();
                if (findElementByName("nameField").uitext.Text.Length < 12)
                    findElementByName("nameField").uitext.AddAtEnd(InputHandler.GetTextEntry().ToString());
            }
        }

        #endregion
    }
}