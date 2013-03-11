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

namespace LameChicken
{
    public class Profile
    {
        #region Fields

        private string _name;
        private int _profileId;
        private int _checkpoint;
        private string _pictureName;

        #endregion

        #region Constructor

        public Profile(int id, string name, int checkpoint, string pictureName)
        {
            _name = name;
            _profileId = id;
            _checkpoint = checkpoint;
            _pictureName = pictureName;
        }

        #endregion

        #region Properties

        public Texture2D ProfilePic
        {
            get { return GameContent.Load<Texture2D>(_pictureName); }
        }

        public string TextureName
        {
            get { return _pictureName; }
        }

        public int ProfileID
        {
            get { return _profileId; }
        }

        public int Checkpoint
        {
            get { return _checkpoint; }
            set 
            { 
                _checkpoint = value;
                ProfileManager.saveProfile(_profileId);
            }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        public void increaseCheckpoint()
        {
            Checkpoint++;
        }
        #endregion
    }
}
