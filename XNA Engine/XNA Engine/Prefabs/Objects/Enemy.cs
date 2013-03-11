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
    class Enemy : Component
    {
        public Enemy(GameObjectManager objMan, int id)
            : base(objMan, id)
        {

        }

        public override void FixedUpdate(float Delta, Game game)
        {
            List<Component> comps = _objectManager.GetComponents<Player>();
            Vector3 distance =  Vector3.Zero;
            if (comps != null)
                if(comps.Count != 0)
                    distance = comps[0].gameObject.transform.position - gameObject.transform.position;

            if (distance.LengthSquared() <= 256f * 256f)
            {
                if (distance.X > 0)
                    gameObject.characterMover.MoveRight(Delta, true);
                else
                    gameObject.characterMover.MoveLeft(Delta, true);
            }

            base.FixedUpdate(Delta, game);
        }

        public override void OnTriggerEnter(GameObject other)
        {
            if (other.GetComponent<KillArea>() != null)
            {
                gameObject.DeleteObject();
                GameObject obj = _objectManager.CreateObject("deathparticle", "emitter");
                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, gameObject.transform.position, new Vector3(1f, 1f, 1f), Vector3.Zero, gameObject.transform.layer));
                obj.AddComponent<Emitter>(new Emitter(_objectManager, obj.Id, GameContent.Load<Texture2D>("smoke"), Color.Black, 9, 4, new Rectangle(0, 0, 20, 20), 3f, 5f, Vector3.Up, 1f, 5, (1f / 4f) * 9f));
                obj.AddComponent<Temporary>(new Temporary(_objectManager, obj.Id, 1.5f));
            }
            base.OnTriggerEnter(other);
        }

        public override void OnCollision(GameObject other)
        {
            if (other.GetComponent<Player>() != null)
            {
                gameObject.DeleteObject();
                GameObject obj = _objectManager.CreateObject("deathparticle", "emitter");
                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, gameObject.transform.position, new Vector3(1f, 1f, 1f), Vector3.Zero, gameObject.transform.layer));
                obj.AddComponent<Emitter>(new Emitter(_objectManager, obj.Id, GameContent.Load<Texture2D>("smoke"), Color.Black, 9, 4, new Rectangle(0, 0, 20, 20), 3f, 5f, Vector3.Up, 1f, 5, (1f / 4f) * 9f));
                obj.AddComponent<Temporary>(new Temporary(_objectManager, obj.Id, 1.5f));
                //audio Handler play cue
            }
            if (other.GetComponent<Boulder>() != null)
            {
                gameObject.DeleteObject();
                GameObject obj = _objectManager.CreateObject("deathparticle", "emitter");
                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id, gameObject.transform.position, new Vector3(1f, 1f, 1f), Vector3.Zero, gameObject.transform.layer));
                obj.AddComponent<Emitter>(new Emitter(_objectManager, obj.Id, GameContent.Load<Texture2D>("smoke"), Color.Black, 9, 4, new Rectangle(0, 0, 20, 20), 3f, 5f, Vector3.Up, 1f, 5, (1f / 4f) * 9f));
                obj.AddComponent<Temporary>(new Temporary(_objectManager, obj.Id, 1.5f));
                //audio Handler play cue
            }
            base.OnCollision(other);
        }

        public static void SpawnEnemy(Vector3 position)
        {
            GameObject obj = LevelManager.Main.Current.ObjectManager.CreateObject("enemy", "enemy");
            obj.AddComponent<Transform>(new Transform(LevelManager.Main.Current.ObjectManager, obj.Id, position, new Vector3(0.25f, 0.25f, 1f), Vector3.Zero, 1));
            obj.AddComponent<Sprite>(new Sprite(LevelManager.Main.Current.ObjectManager, obj.Id, GameContent.Load<Texture2D>("Characters/walker_idle"), 4));
            obj.AddComponent<Animation>(new Animation(LevelManager.Main.Current.ObjectManager, obj.Id));
            obj.AddComponent<CharacterMover>(new CharacterMover(LevelManager.Main.Current.ObjectManager, obj.Id, 140f, 240f, 750f));
            obj.AddComponent<Audio>(new Audio(LevelManager.Main.Current.ObjectManager, obj.Id, "footstep", true, 0.5f));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/walker_walk"), "walk", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).AddAnimation(GameContent.Load<Texture2D>("Characters/walker_idle"), "idle", new Rectangle(0, 0, 150, 355));
            (obj.GetComponent<Animation>() as Animation).SetAnimation("idle");
            (obj.GetComponent<Animation>() as Animation).FPS = 15;
            obj.AddComponent<Physic>(new Physic(LevelManager.Main.Current.ObjectManager, obj.Id, true, true, 10, true, Vector3.Zero));
            obj.AddComponent<Enemy>(new Enemy(LevelManager.Main.Current.ObjectManager, obj.Id));
            obj.sprite.Initialize();
        }
    }
}
