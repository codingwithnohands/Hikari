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
    public class LevelManager
    {
        #region Fields

        private Level _curLevel;
        private Dictionary <int, Level> _loadedLvls = new Dictionary<int,Level>();
        private int _queuedLoad;
        private int _queuedSwitch;
        private int _queuedReplace;
        private List<int> _toRemove = new List<int>();
        private int _lastLvl = 0;

        private static LevelManager _main;

        #endregion

        #region Constructor

        public LevelManager()
        {

        }

        #endregion

        #region Properties

        public Level Current
        {
            get { return _curLevel; }
        }

        public static LevelManager Main
        {
            get { return _main; }
            set
            {
                if (_main == null)
                    _main = value;
            }
        }

        public bool GameRunning
        {
            get { return _lastLvl != 0; }
        }

        #endregion

        #region Methods

        public bool AddLevel(Level level)
        {
            if (_loadedLvls.ContainsKey(level.Id))
                return false;

            _loadedLvls.Add(level.Id, level);
            return true;
        }

        public bool LoadLevel(string name)
        {
            _queuedLoad = 0;
            if (_loadedLvls.TryGetValue(name.GetHashCode(), out _curLevel))
            {
                _curLevel.Initialize();
                return true;
            }
            return false;
        }

        public void UnloadGameLevels()
        {
            _lastLvl = 0;
            foreach (var pair in _loadedLvls)
            {
                if (pair.Key != "mainMenu".GetHashCode())
                    _toRemove.Add(pair.Key);
            }
        }

        public bool UnloadLevel(string name)
        {
            if(_loadedLvls.ContainsKey(name.GetHashCode()))
            {
                _toRemove.Add(name.GetHashCode());
                return true;
            }
            return false;
        }

        public void RemoveLevels()
        {
            foreach (int i in _toRemove)
            {
                _loadedLvls.Remove(i);
            }
        }

        public bool LoadLevel(int id)
        {
            _queuedLoad = 0;
            if (_loadedLvls.TryGetValue(id, out _curLevel))
            {
                _curLevel.Initialize();
                return true;
            }
            return false;
        }

        public bool SwitchLevel(string name)
        {
            _queuedSwitch = 0;
            int buffer = _curLevel.Name.GetHashCode();
            if (_loadedLvls.TryGetValue(name.GetHashCode(), out _curLevel))
            {
                _lastLvl = buffer;
                return true;
            }
            return false;
        }

        public bool SwitchLevel(int id)
        {
            _queuedSwitch = 0;
            int buffer = _curLevel.Name.GetHashCode();
            if (_loadedLvls.TryGetValue(id, out _curLevel))
            {
                _lastLvl = buffer;
                return true;
            }
            return false;
        }

        public bool ReplaceLevel(string name)
        {
            _queuedReplace = 0;
            int buffer = _curLevel.Name.GetHashCode();
            if (_loadedLvls.TryGetValue(name.GetHashCode(), out _curLevel))
            {
                _loadedLvls.Remove(buffer);
                _curLevel.Initialize();
                return true;
            }
            return false;
        }

        public bool ReplaceLevel(int id)
        {
            _queuedReplace = 0;
            int buffer = _curLevel.Name.GetHashCode();
            if (_loadedLvls.TryGetValue(id, out _curLevel))
            {
                _loadedLvls.Remove(buffer);
                _curLevel.Initialize();
                return true;
            }
            return false;
        }

        public void requestLevelSwitch(string name)
        {
            if(_loadedLvls.ContainsKey(name.GetHashCode()))
                _queuedSwitch = name.GetHashCode();
        }

        public void requestSwitchToPrev()
        {
            if (_loadedLvls.ContainsKey(_lastLvl))
                _queuedSwitch = _lastLvl;
        }

        public void requestLevelLoad(string name)
        {
            if (_loadedLvls.ContainsKey(name.GetHashCode()))
                _queuedLoad = name.GetHashCode();
        }

        public void requestLevelReplace(string name)
        {
            if (_loadedLvls.ContainsKey(name.GetHashCode()))
                _queuedReplace = name.GetHashCode();
        }

        public void Update(GameTime gameTime, Game game)
        {
                if (_toRemove.Count != 0)
                    RemoveLevels();
                if (_queuedSwitch != 0)
                    SwitchLevel(_queuedSwitch);
                else if (_queuedLoad != 0)
                    LoadLevel(_queuedLoad);
                else if (_queuedReplace != 0)
                    ReplaceLevel(_queuedReplace);

                _curLevel.UpdateFixedTime((float)gameTime.ElapsedGameTime.TotalSeconds);
                InputHandler.Update((float)gameTime.ElapsedGameTime.TotalSeconds, game);
                _curLevel.Update((float)gameTime.ElapsedGameTime.TotalSeconds, game);
                _curLevel.ObjectManager.ObjectUpdate((float)gameTime.ElapsedGameTime.TotalSeconds, game);
                FixedUpdate(game);
        }

        public void FixedUpdate(Game game)
        {
            if (_curLevel.FixedDeltaTime >= _curLevel.FixedTimestep)
            {
                _curLevel.FixedUpdate(_curLevel.FixedDeltaTime, game);
                _curLevel.ObjectManager.FixedObjectUpdate(_curLevel.FixedDeltaTime, game);
            }

            _curLevel.ObjectManager.startVolumeUpdate();
            _curLevel.ObjectManager.startDeletion();

            if (_curLevel.FixedDeltaTime >= _curLevel.FixedTimestep)
            {
                PhysicsManager.Update(_curLevel.FixedDeltaTime, _curLevel.ObjectManager);
                //reset elapsedFixedTime
                _curLevel.ResetFixedTime();
            }
            Camera main = _curLevel.ObjectManager.GetComponents<Camera>().FirstOrDefault() as Camera;
            main.ToPosition();
            #region testregion

            /*if (InputHandler.KeyDown(Keys.Enter))
                {
                    if (_graphics.PreferredBackBufferWidth == 800)
                    {
                        _graphics.PreferredBackBufferWidth = 1024;
                        _graphics.PreferredBackBufferHeight = 768;
                        _graphics.ApplyChanges();
                    }
                    else if (_graphics.PreferredBackBufferWidth == 1024)
                    {
                        _graphics.PreferredBackBufferWidth = 1920;
                        _graphics.PreferredBackBufferHeight = 1080;
                        _graphics.ApplyChanges();
                    }

                    else if (_graphics.PreferredBackBufferWidth == 1920)
                    {
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.PreferredBackBufferHeight = 720;
                        _graphics.ApplyChanges();
                    }
                    else if (_graphics.PreferredBackBufferWidth == 1280)
                    {
                        _graphics.PreferredBackBufferWidth = 800;
                        _graphics.PreferredBackBufferHeight = 480;
                        _graphics.ApplyChanges();
                    }
                }

                if (InputHandler.KeyDown(Keys.F))
                {
                    _graphics.ToggleFullScreen();
                    _graphics.ApplyChanges();
                }*/

            #endregion
        }

        #endregion
    }

    public class Level
    {
        #region Fields

        protected LevelManager _lvlMan;
        protected string _name;
        protected GameObjectManager _objMan = new GameObjectManager();
        protected float _elapsedFixedTime;
        protected float _fixedTimeStep;

        #endregion

        #region Constructor

        public Level(LevelManager lvlMan, string name)
        {
            _lvlMan = lvlMan;
            _name = name;
        }

        #endregion

        #region Properties

        public GameObjectManager ObjectManager
        {
            get { return _objMan; }
        }

        public int Id
        {
            get { return _name.GetHashCode(); }
        }

        public string Name
        {
            get { return _name; }
        }

        public float FixedDeltaTime
        {
            get { return _elapsedFixedTime; }
        }

        public float FixedTimestep
        {
            get { return _fixedTimeStep; }
        }

        #endregion

        #region Methods

        public virtual void Initialize()
        {
            _objMan = new GameObjectManager();
            _elapsedFixedTime = 0f;
            _fixedTimeStep = 0.04f;
        }

        public void lookUpCheckpoint()
        {
            List<Component> comps = ObjectManager.GetComponents<Checkpoint>();
            foreach(Component comp in comps)
            {
                if ((comp as Checkpoint).CheckpointID == ProfileManager.CurrentProfile.Checkpoint)
                    (comp as Checkpoint).LoadCheckpoint();
            }
        }

        public void UpdateFixedTime(float Delta)
        {
            _elapsedFixedTime += Delta;
        }

        public void ResetFixedTime()
        {
            _elapsedFixedTime = 0f;
        }

        public virtual void Update(float Delta, Game game)
        {

        }

        public virtual void FixedUpdate(float Delta, Game game)
        {
            
        }

        public void Draw(flatRender renderer)
        {
            renderer.RenderScene(_objMan);
        }

        #endregion
    }

    public class Checkpoint : Component
    {
        #region Fields

        private  int _checkpointNumber;

        #endregion

        #region Constructor

        public Checkpoint(GameObjectManager objMan, int id, int checkpointNumber)
            : base(objMan, id)
        {
            _checkpointNumber = checkpointNumber;
        }

        #endregion

        #region Properties

        public int CheckpointID
        {
            get { return _checkpointNumber; }
        }

        #endregion

        #region methods

        public void SetCheckpointState()
        {
            for (int i = 0; ProfileManager.CurrentProfile.Checkpoint < _checkpointNumber; i++)
                ProfileManager.CurrentProfile.increaseCheckpoint();
        }

        public virtual void LoadCheckpoint()
        {
            _objectManager.GetObjectByName("Character").transform.position = gameObject.transform.position;
        }

        #endregion
    }
}
