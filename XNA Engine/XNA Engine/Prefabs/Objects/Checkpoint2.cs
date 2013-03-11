using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LameChicken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace hikari_game.Prefabs.Objects
{
    class Checkpoint2 : Checkpoint
    {
        public Checkpoint2(GameObjectManager objMan, int id)
            : base(objMan, id, 2)
        {

        }

        public override void LoadCheckpoint()
        {
            GameObject obj = _objectManager.CreateObject("StartScript", "Script");
            obj.AddComponent<LevelStartScript>(new LevelStartScript(_objectManager, obj.Id, 1f));
            obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(-3995f, 13f, 0.5f), new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            base.LoadCheckpoint();
        }
    }
}
