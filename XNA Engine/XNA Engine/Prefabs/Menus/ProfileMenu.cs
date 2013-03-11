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
    class ProfileMenu : UIMenu
    {
        #region Fields

        #endregion

        #region Constructor

        public ProfileMenu(GameObjectManager objMan, int id)
            : base(objMan, 
            id, 
            new Rectangle(0, 0, 50, 57), 
            alignment.UpperLeft,
            objMan.CreateObject("profileBg", "menuBG"),
            objMan.CreateObject("profile1Button", "button"),
            objMan.CreateObject("profile2Button", "button"),
            objMan.CreateObject("profile3Button", "button"),
            objMan.CreateObject("profile1Pic", "uitex"),
            objMan.CreateObject("profile2Pic", "uitex"),
            objMan.CreateObject("profile3Pic", "uitex"),
            objMan.CreateObject("erase1Button", "button"),
            objMan.CreateObject("erase2Button", "button"),
            objMan.CreateObject("erase3Button", "button"),
            objMan.CreateObject("profile1Text", "buttonText"),
            objMan.CreateObject("profile2Text", "buttonText"),
            objMan.CreateObject("profile3Text", "buttonText"),
            objMan.CreateObject("profileMenuText", "buttonText"))
        {
            GameObject obj = objMan.GetObjectByName("profileBg");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(-7f, 0, 0.9f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilesBg"), 3, alignment.UpperLeft));

            obj = objMan.GetObjectByName("profile1Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(12f, 28.5f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilebutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.Black, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("profile2Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(12f, 55.5f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilebutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.Black, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("profile3Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(12, 81.5f, 0.8f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profilebutton"), 3, alignment.CenterLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.Black, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("profile1Pic");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(3, 25, 0.7f), new Vector3(0.7f, 1f, 1f), Vector3.Zero, 1));
            if (ProfileManager.ProfileExists(1))
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, ProfileManager.GetProfile(1).ProfilePic, 7, alignment.CenterLeft));
            else
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profDef"), 7, alignment.CenterLeft));

            obj = objMan.GetObjectByName("profile2Pic");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(3, 52, 0.7f), new Vector3(0.7f, 1f, 1f), Vector3.Zero, 1));
            if (ProfileManager.ProfileExists(2))
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, ProfileManager.GetProfile(2).ProfilePic, 7, alignment.CenterLeft));
            else
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profDef"), 7, alignment.CenterLeft));

            obj = objMan.GetObjectByName("profile3Pic");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(3, 78, 0.7f), new Vector3(0.7f, 1f, 1f), Vector3.Zero, 1));
            if (ProfileManager.ProfileExists(3))
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, ProfileManager.GetProfile(3).ProfilePic, 7, alignment.CenterLeft));
            else
                obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/profDef"), 7, alignment.CenterLeft));

            obj = objMan.GetObjectByName("erase1Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(74f, 21f, 0.6f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/resetProf"), 4, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("erase2Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(74f, 48f, 0.6f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/resetProf"), 4, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("erase3Button");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(74f, 74f, 0.6f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UITexture>(new UITexture(objMan, obj.Id, GameContent.Load<Texture2D>("UI/Profile/resetProf"), 4, alignment.UpperLeft));
            obj.AddComponent<UIButton>(new UIButton(objMan, obj.Id, Color.White, Color.LightSkyBlue, Color.LightSeaGreen));

            obj = objMan.GetObjectByName("profile1Text");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(25, 28.5f, 0.3f), new Vector3(18f, 18f, 1f), Vector3.Zero, 1));
            string name = "empty";
            if (ProfileManager.ProfileExists(1))
                name = ProfileManager.GetProfile(1).Name;
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, name, GameContent.Load<SpriteFont>("test"), Color.Black, 60, 20, alignment.CenterLeft));

            obj = objMan.GetObjectByName("profile2Text");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(25, 55.5f, 0.3f), new Vector3(18f, 18f, 1f), Vector3.Zero, 1));
            name = "empty";
            if (ProfileManager.ProfileExists(2))
                name = ProfileManager.GetProfile(2).Name;
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, name, GameContent.Load<SpriteFont>("test"), Color.Black, 60, 20, alignment.CenterLeft));

            obj = objMan.GetObjectByName("profile3Text");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(25, 81.5f, 0.3f), new Vector3(18f, 18f, 1f), Vector3.Zero, 1));
            name = "empty";
            if (ProfileManager.ProfileExists(3))
                name = ProfileManager.GetProfile(3).Name;
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, name, GameContent.Load<SpriteFont>("test"), Color.Black, 60, 20, alignment.CenterLeft));

            obj = objMan.GetObjectByName("profileMenuText");
            obj.AddComponent<Transform>(new Transform(objMan, obj.Id, new Vector3(50, 4, 0.3f), new Vector3(20f, 20f, 1f), Vector3.Zero, 1));
            obj.AddComponent<UIText>(new UIText(objMan, obj.Id, "Profiles", GameContent.Load<SpriteFont>("test"), Color.Black, 100, 19, alignment.UpperCenter));

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
                            case "profile1Button":
                                if (!ProfileManager.SetActive(1))
                                {
                                    GameObject obj = _objectManager.CreateObject("profileMenu", "menu");
                                    obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(50, 50, 0.1f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
                                    obj.AddComponent<UIGroup>(new newProfileMenu(_objectManager, obj.Id, 1));
                                    (obj.GetComponent<UIGroup>() as UIMenu).GrabFocus();
                                }
                                break;
                            case "profile2Button":
                                if (!ProfileManager.SetActive(2))
                                {
                                    GameObject obj = _objectManager.CreateObject("profileMenu", "menu");
                                    obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(50, 50, 0.1f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
                                    obj.AddComponent<UIGroup>(new newProfileMenu(_objectManager, obj.Id, 2));
                                    (obj.GetComponent<UIGroup>() as UIMenu).GrabFocus();
                                }
                                break;
                            case "profile3Button":
                                if (!ProfileManager.SetActive(3))
                                {
                                    GameObject obj = _objectManager.CreateObject("profileMenu", "menu");
                                    obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(50, 50, 0.1f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
                                    obj.AddComponent<UIGroup>(new newProfileMenu(_objectManager, obj.Id, 3));
                                    (obj.GetComponent<UIGroup>() as UIMenu).GrabFocus();
                                }
                                break;
                            case "erase1Button":
                                if (ProfileManager.ProfileExists(1))
                                {
                                    if (ProfileManager.SetActive(1))
                                    {
                                        (LevelManager.Main).UnloadGameLevels();
                                    }
                                    ProfileManager.removeProfile(1);
                                }
                                break;
                            case "erase2Button":
                                if (ProfileManager.ProfileExists(2))
                                {
                                    if (ProfileManager.SetActive(2))
                                    {
                                        (LevelManager.Main).UnloadGameLevels();
                                    }
                                    ProfileManager.removeProfile(2);
                                }
                                break;
                            case "erase3Button":
                                if (ProfileManager.ProfileExists(3))
                                {
                                    if (ProfileManager.SetActive(3))
                                    {
                                        (LevelManager.Main).UnloadGameLevels();
                                    }
                                    ProfileManager.removeProfile(3);
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
            if (ProfileManager.CurrentProfile != null)
            {
                switch (ProfileManager.CurrentProfile.ProfileID)
                {
                    case 1:
                        if (findElementByName("profile1Button").uitexture.Color != Color.Green)
                            findElementByName("profile1Button").uitexture.Color = Color.Green;
                        if (findElementByName("profile2Button").uitexture.Color == Color.Green)
                            findElementByName("profile2Button").uitexture.Color = Color.Black;
                        if (findElementByName("profile3Button").uitexture.Color == Color.Green)
                            findElementByName("profile3Button").uitexture.Color = Color.Black;
                        break;
                    case 2:
                        if (findElementByName("profile1Button").uitexture.Color == Color.Green)
                            findElementByName("profile1Button").uitexture.Color = Color.Black;
                        if (findElementByName("profile2Button").uitexture.Color != Color.Green)
                            findElementByName("profile2Button").uitexture.Color = Color.Green;
                        if (findElementByName("profile3Button").uitexture.Color == Color.Green)
                            findElementByName("profile3Button").uitexture.Color = Color.Black;
                        break;
                    case 3:
                        if (findElementByName("profile1Button").uitexture.Color == Color.Green)
                            findElementByName("profile1Button").uitexture.Color = Color.Black;
                        if (findElementByName("profile2Button").uitexture.Color == Color.Green)
                            findElementByName("profile2Button").uitexture.Color = Color.Black;
                        if (findElementByName("profile3Button").uitexture.Color != Color.Green)
                            findElementByName("profile3Button").uitexture.Color = Color.Green;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (findElementByName("profile1Button").uitexture.Color == Color.Green)
                    findElementByName("profile1Button").uitexture.Color = Color.Black;
                if (findElementByName("profile2Button").uitexture.Color == Color.Green)
                    findElementByName("profile2Button").uitexture.Color = Color.Black;
                if (findElementByName("profile3Button").uitexture.Color == Color.Green)
                    findElementByName("profile3Button").uitexture.Color = Color.Black;
            }
            if (ProfileManager.ProfileExists(1))
            {
                if (findElementByName("profile1Text").uitext.Text != ProfileManager.GetProfile(1).Name)
                {
                    findElementByName("erase1Button").uitexture.Color = Color.White;
                    findElementByName("profile1Text").uitext.Text = ProfileManager.GetProfile(1).Name;
                    findElementByName("profile1Pic").uitexture.texture2D = ProfileManager.GetProfile(1).ProfilePic;
                }
            }
            else
            {
                if (findElementByName("profile1Text").uitext.Text != "empty")
                {
                    findElementByName("profile1Text").uitext.Text = "empty";
                    findElementByName("profile1Pic").uitexture.texture2D = GameContent.Load<Texture2D>("UI/Profile/profDef");
                }
                findElementByName("erase1Button").uitexture.Color = new Color(0, 0, 0, 25);
            }
            if (ProfileManager.ProfileExists(2))
            {
                if (findElementByName("profile2Text").uitext.Text != ProfileManager.GetProfile(2).Name)
                {
                    findElementByName("erase2Button").uitexture.Color = Color.White;
                    findElementByName("profile2Text").uitext.Text = ProfileManager.GetProfile(2).Name;
                    findElementByName("profile2Pic").uitexture.texture2D = ProfileManager.GetProfile(2).ProfilePic;
                }
            }
            else
            {
                if (findElementByName("profile2Text").uitext.Text != "empty")
                {
                    findElementByName("profile2Text").uitext.Text = "empty";
                    findElementByName("profile2Pic").uitexture.texture2D = GameContent.Load<Texture2D>("UI/Profile/profDef");
                }
                findElementByName("erase2Button").uitexture.Color = new Color(0, 0, 0, 25);
            }
            if (ProfileManager.ProfileExists(3))
            {
                if (findElementByName("profile3Text").uitext.Text != ProfileManager.GetProfile(3).Name)
                {
                    findElementByName("erase3Button").uitexture.Color = Color.White;
                    findElementByName("profile3Text").uitext.Text = ProfileManager.GetProfile(3).Name;
                    findElementByName("profile3Pic").uitexture.texture2D = ProfileManager.GetProfile(3).ProfilePic;
                }
            }
            else
            {
                if (findElementByName("profile3Text").uitext.Text != "empty")
                {
                    findElementByName("profile3Text").uitext.Text = "empty";
                    findElementByName("profile3Pic").uitexture.texture2D = GameContent.Load<Texture2D>("UI/Profile/profDef");
                }
                findElementByName("erase3Button").uitexture.Color = new Color(0, 0, 0, 25);
            }
        }
        #endregion


    }
}
