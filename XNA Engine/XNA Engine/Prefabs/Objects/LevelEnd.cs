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
    class LevelEnd : Component
    {
        public LevelEnd(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            gameObject.AddComponent<Transform>(new Transform(_objectManager, Id));
            gameObject.transform.position = new Vector3(-4032f + 12 * 128f, 0f, 0.5f);
            gameObject.transform.layer = 1;
            gameObject.AddComponent<Trigger>(new Trigger(_objectManager, Id, new Rectangle(0, -50, 20, 100)));
        }

        public void End()
        {
            ProfileManager.CurrentProfile.Checkpoint = 1;
            gameObject.AddComponent<LevelTransition>(new LevelTransition(_objectManager, Id, "mainMenu", true));
            (gameObject.GetComponent<LevelTransition>() as LevelTransition).startTransition();
        }
    }
}
