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

namespace LameChicken
{
    public static class PhysicsManager
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void Update(float Delta, GameObjectManager objMan)
        {
            Camera main = objMan.GetComponents<Camera>().FirstOrDefault() as Camera;
            if (main != null)
            {
                List<Physic> phys = objMan.Quadtree.GetRange<Physic>(new Rectangle((int)(main.gameObject.transform.position.X - (main.vision.X * 1.5f)),
                    (int)(main.gameObject.transform.position.Y - (main.vision.Y * 1.5f)),
                    (int)(main.vision.X * 3),
                    (int)(main.vision.Y * 3)), 1);
                CheckCollisions(Delta, objMan, phys);
                UpdateTriggers(objMan);
            }
        }

        private static void MoveByVelocity(float Delta, GameObjectManager objMan, Physic p)
        {
            Vector3 buffer = p.velocity;

            if (p.constraint.X != 0f)
                buffer.X = p.constraint.X;
            if (p.constraint.Y != 0f)
                buffer.Y = p.constraint.Y;
            if (p.constraint.Z != 0f)
                buffer.Z = p.constraint.Z;

            Vector3 pos = p.gameObject.transform.position;
            p.gameObject.transform.position += p.velocity * Delta;
            objMan.Quadtree.UpdateObject(p.gameObject, pos);
        }

        private static void CheckCollisions(float Delta, GameObjectManager objMan, List<Physic> physics)
        {
            if (physics == null) return;
            foreach (Physic e in physics)
            {
                if (e.IsRigid && e.ActiveCol)
                {
                    CollisionData col = objMan.Quadtree.GetPotentialsContacts(e.gameObject);
                    col.doMovement(objMan, Delta);
                }
                else
                    MoveByVelocity(Delta, objMan, e);
            }
        }

        private static void UpdateTriggers(GameObjectManager objMan)
        {
            List <Component> comps = objMan.GetComponents<Trigger>();
            if (comps != null)
            {
                foreach (Component comp in comps)
                {
                    (comp as Trigger).UpdateLists(objMan.Quadtree.GetRange((comp as Trigger).Range, 1));
                }
            }
        }

        #endregion
    }
}
