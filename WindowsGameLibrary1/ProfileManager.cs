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
using System.IO;

namespace LameChicken
{
    public static class ProfileManager
    {
        #region Fields

        private static Dictionary<int, Profile> _profiles = new Dictionary<int,Profile>();
        private static int _active = 0;

        #endregion

        #region Constructor

        public static void Initialize()
        {
            for (int i = 1; i <= 3; i++)
            {
                Profile prof = loadProfile(i);
                if (prof != null)
                    _profiles.Add(prof.ProfileID, prof);
            }
        }

        #endregion

        #region Properties

        public static Profile CurrentProfile
        {
            get
            {
                Profile profile = null;
                _profiles.TryGetValue(_active, out profile);
                return profile;
            }
        }

        #endregion

        #region Methods

        //addProfile(int number, string name)
        public static bool addProfile(int profileID, string name, string picture)
        {
            if (profileID != 0 && name != "" && picture != null)
            {
                if (_profiles.ContainsKey(profileID))
                {
                    _profiles.Remove(profileID);
                }
                _profiles.Add(profileID, new Profile(profileID, name, 1, picture));
                return true;
            }
            return false;
        }

        //loadfile
        public static Profile loadProfile(int number)
        {
            string file = "profile" + number + ".sav";
            Profile newProf = null;
            if(File.Exists(file))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    newProf = new Profile(reader.ReadInt32(),
                    reader.ReadString(),
                    reader.ReadInt32(),
                    reader.ReadString());
                }
            }
            return newProf;
        }

        //savefiles
        public static bool saveProfile(int number)
        {
            string file = "profile" + number + ".sav";
            Profile profile = null;
            if (!_profiles.TryGetValue(number, out profile))
                return false;
            using (BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.Create)))
            {
                    writer.Write(profile.ProfileID);
                    writer.Write(profile.Name);
                    writer.Write(profile.Checkpoint);
                    writer.Write(profile.TextureName);
            }
            return true;
        }

        //removeProfile
        public static bool removeProfile(int profileID)
        {
            if (profileID != 0)
            {
                if (_profiles.ContainsKey(profileID))
                {
                    _profiles.Remove(profileID);
                    if (_active == profileID)
                        _active = 0;
                    DeleteFile(profileID);
                    return true;
                }
            }
            return false;
        }

        public static bool ProfileExists(int id)
        {
            return _profiles.ContainsKey(id);
        }

        public static bool SetActive(int id)
        {
            if (_profiles.ContainsKey(id))
            {
                _active = id;
                return true;
            }
            return false;
        }

        public static Profile GetProfile(int id)
        {
                Profile profile = null;
                _profiles.TryGetValue(id, out profile);
                return profile;
        }

        private static void DeleteFile(int id)
        {
            string file = "profile" + id + ".sav";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        #endregion
    }
}
