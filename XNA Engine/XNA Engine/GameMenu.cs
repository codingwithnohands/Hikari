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

namespace XNA_Engine
{
    public class GameMenu : UIGroup
    {
        #region Fields

        Dictionary<int, int> _objectOrder =  new Dictionary<int, int>();
        protected int _active;

        #endregion

        #region Constructor

        public GameMenu(GameObjectManager objMan, int id, Rectangle range, params GameObject[] elements) 
            : base(objMan, id, range, elements)
        {

        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public override void Update(float Delta)
        {
            foreach (var pair in _objectOrder)
            {
                GameObject obj;
                if (_elements.TryGetValue(pair.Key, out obj))
                {
                    
                }
            }
            base.Update(Delta);
        }

        public override void FixedUpdate(float Delta)
        {
            base.FixedUpdate(Delta);
        }

        #endregion

    }
}
