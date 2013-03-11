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
    public class Player : Component
    {
        public Player(GameObjectManager objMan, int id)
            : base(objMan, id)
        {

        }

        public override void OnTriggerEnter(GameObject other)
        {
            if(other.GetComponent<Checkpoint>() != null)
            {
                      (other.GetComponent<Checkpoint>() as Checkpoint).SetCheckpointState();
            }
            if (other.GetComponent<KillArea>() != null)
            {
                gameObject.DeleteObject();
                LevelManager.Main.requestLevelLoad(LevelManager.Main.Current.Name);
            }
            if (other.GetComponent<LevelTransition>() != null)
                (other.GetComponent<LevelTransition>() as LevelTransition).startTransition();
            if (other.GetComponent<Artifact>() != null)
            {
                GameObject obj = _objectManager.CreateObject("xButton", "button");
                GameObject button = _objectManager.CreateObject("button", "button");
                button.AddComponent<Transform>(new Transform(_objectManager, button.Id, new Vector3(50f, 80f, 0.5f), new Vector3(0.8f, 1f, 1f), Vector3.Zero, 1));
                button.AddComponent<UITexture>(new UITexture(_objectManager, button.Id, GameContent.Load<Texture2D>("UI/Xbox360_Button_X"), 6, alignment.Center));

                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, new Vector3(0, 0, 0.5f), new Vector3(1f, 1f, 1f), Vector3.Zero, 1));
                obj.AddComponent<UIGroup>(new UIGroup(_objectManager, obj.Id, new Rectangle(0, 0, 100, 100), alignment.UpperLeft, button));
                
            }
            if (other.GetComponent<LevelEnd>() != null)
            {
                gameObject.DeleteObject();
            }
            base.OnTriggerEnter(other);
        }

        public override void OnTriggerStay(GameObject other)
        {
            if (other.GetComponent<Artifact>() != null)
            {
                if(InputHandler.KeyDown(Keys.X) || InputHandler.GButtonDown("X"))
                {
                    (other.GetComponent<Artifact>() as Artifact).DestroyArtifact();
                    GameObject obj = _objectManager.GetObjectByName("xButton");
                    if (obj != null)
                        obj.DeleteObject();
                }
            }
            base.OnTriggerEnter(other);
        }

        public override void OnTriggerLeave(GameObject other)
        {
            if (other.GetComponent<Artifact>() != null)
            {
                GameObject obj = _objectManager.GetObjectByName("xButton");
                if (obj != null)
                    obj.DeleteObject();
            }
            base.OnTriggerLeave(other);
        }

        public override void OnCollision(GameObject other)
        {
            if (other.GetComponent<Enemy>() != null)
            {
                gameObject.DeleteObject();

                GameObject obj = _objectManager.CreateObject("respawnScript", "script");
                obj.AddComponent<DelayedRespawn>(new DelayedRespawn(_objectManager, obj.Id, 2f));
            }
            if (other.GetComponent<Boulder>() != null)
            {
                gameObject.DeleteObject();

                GameObject obj = _objectManager.CreateObject("respawnScript", "script");
                obj.AddComponent<DelayedRespawn>(new DelayedRespawn(_objectManager, obj.Id, 2f));
            }
            if (other.GetComponent<Tree>() != null )
            {
                if (_objectManager.GetObjectByName("artifact") == null)
                {
                   Tree tree = other.GetComponent<Tree>() as Tree;
                   if (tree != null)
                       tree.Fall();
                }
            }
            if (other.name == "shelf")
            {
                GameObject obj = _objectManager.CreateObject("woodchipper", "emitter");
                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, other.transform.position + (Vector3.Up *10f), new Vector3 (1f, 1f, 1f), Vector3.Zero, 1));
                obj.AddComponent<Emitter>(new Emitter(_objectManager,
                obj.Id, GameContent.Load<Texture2D>("wood_particle"),
                Color.White,
                0,
                1,
                new Rectangle(0, 0, 15, 40),
                0.25f,
                20f,
                7,
                1.5f));
                obj.AddComponent<Temporary>(new Temporary(_objectManager, obj.Id, 1f));

                obj = _objectManager.GetObjectByName("boulder02");
                if (obj != null)
                {
                    obj.AddComponent<Physic>(new Physic(_objectManager, obj.Id, true, true, 100, true, Vector3.Zero));
                }

                other.DeleteObject();
            }
            base.OnCollision(other);
        }
    }
}
