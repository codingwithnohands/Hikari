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
    class Artifact : Component
    {
        public Artifact(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            gameObject.AddComponent<Trigger>(new Trigger(_objectManager, Id, new Rectangle(-5, -10, 10, 20)));
        }

        public void DestroyArtifact()
        {
            spawnEnemies();
            gameObject.DeleteObject();
        }

        private void spawnEnemies()
        {
            Vector3 pos = gameObject.transform.position;
            pos -= new Vector3(128f, 10f, -0.1f);
            for (int i = 0; i < 3; i++)
                Enemy.SpawnEnemy(pos - (i * new Vector3(10f, 0f, 0f)));
        }
    }
}
