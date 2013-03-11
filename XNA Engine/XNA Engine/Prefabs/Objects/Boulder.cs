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
    class Boulder : Component
    {
        public Boulder(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
           
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            if (gameObject.physic != null)
                roll(Delta);
            base.FixedUpdate(Delta, game);
        }

        private void roll(float Delta)
        {
            if (gameObject.physic.HasGroundContact)
            {
                gameObject.physic.AddForce(Vector3.Right * 400f);
                gameObject.physic.AddForce(Vector3.Down * 50f);
                gameObject.transform.rotation = new Vector3(0, 0, gameObject.transform.rotation.Z - (MathHelper.PiOver2 * Delta));
            }
        }

        public override void OnTriggerEnter(GameObject other)
        {
            if (other.GetComponent<LevelEnd>() != null)
            {
                if (_objectManager.GetObjectByName("Character") == null && _objectManager.GetObjectByName("respawnScript") == null)
                    (other.GetComponent<LevelEnd>() as LevelEnd).End();
                else
                    LevelManager.Main.requestLevelLoad(LevelManager.Main.Current.Name);
            }
            base.OnTriggerEnter(other);
        }
    }
}
