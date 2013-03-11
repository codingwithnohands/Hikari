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
    class Plank : Component
    {
        public Plank(GameObjectManager objMan, int id)
            : base(objMan, id)
        {

        }

        private void DestroyPlank()
        {
            //createEmitters wooddebris
            gameObject.DeleteObject();
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
        }
    }
}
