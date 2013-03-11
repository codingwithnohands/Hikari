using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LameChicken.GameObjectSys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LameChicken.components
{

    #region transform

    class Transform : Component
    {
        #region Fields

        public Vector3 _position;
        public Vector3 _scale;
        public Vector3 _rotation;

        #endregion

        #region functions

        #endregion


    }

    #endregion

    #region sprite

    class Sprite : Component
    {

        #region Fields

        Texture2D _sprite;
        bool _useAlpha;

        #endregion

    }

    #endregion

    #region emitter

    class Emitter : Component
    {

        #region Fields

        Texture2D _particle;
        float _lifetime;
        float _velocity;

        #endregion
    }

    #endregion

    #region physics

    class Physics : Component
    {

        #region Fields

        Vector3 _velocity;
        bool _hasCollider;
        float _ColliderRange;
        bool _isTrigger;
        Vector3 _constraint;

        #endregion
    }

    #endregion

    #region audio

    class audio : Component
    {
    }

    #endregion
}
