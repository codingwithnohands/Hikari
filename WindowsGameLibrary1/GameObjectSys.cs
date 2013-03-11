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

    #region GameObjectManager

    public class GameObjectManager
    {
        #region Fields

        //current active gameObjects
        private Dictionary<int, GameObject> _gameObjects;
        private List<int> _deletion;
        //deleted available gameObjects
        private List<int> _availableID;

        private QuadTree _quadtree = new QuadTree();
        private Dictionary<int, Vector3> _updateVolumes = new Dictionary<int,Vector3>();
        //current active components sorted by typeId and by corresponding object id
        private Dictionary<int, Dictionary<int, Component>> _components;

        //ID Counter
        private int _count;

        private ComponentTypeManager _compTypes =  new ComponentTypeManager();

        #endregion

        #region Constructors

        public GameObjectManager()
        {
            _gameObjects = new Dictionary<int,GameObject>();
            _availableID = new List<int>();
            _deletion = new List<int>();
            _components = new Dictionary<int,Dictionary<int,Component>>();
            _count = 0;
        }

        #endregion

        #region Properties

        public QuadTree Quadtree
        {
            get { return _quadtree; }
        }

        #endregion

        #region Functions

        #region Object Functions

        public GameObject CreateObject()
        {
            int i = 0;
            if(_availableID.Count != 0)
            {
                i = _availableID[0];
                while(_availableID.Contains(i))
                    _availableID.Remove(i);
            }
            else
            {
                i = _count++;
            }
            GameObject obj = new GameObject(i, this);
            _gameObjects.Add(obj.Id, obj);
            return obj;
        }

        public GameObject CreateObject(string name, string tag)
        {
            int i = 0;
            if (_availableID.Count != 0)
            {
                i = _availableID[0];
                while (_availableID.Contains(i))
                    _availableID.Remove(i);
            }
            else
            {
                i = _count++;
            }
            GameObject obj = new GameObject(i, this);
            obj.name = name;
            obj.tag = tag;
            _gameObjects.Add(obj.Id, obj);
            return obj;
        }

        public GameObject GetObject(int id)
        {
            GameObject obj = null;
            _gameObjects.TryGetValue(id, out obj);
            return obj;
        }

        public void DeleteObject(int id)
        {
            _deletion.Add(id);
            List<Component> comps = GetComponents(id);
            if (comps != null)
            {
                foreach (Component comp in comps)
                    comp.OnDelete();
            }
        }

        public void DeleteObject(GameObject obj)
        {
            _deletion.Add(obj.Id);
            List<Component> comps = GetComponents(obj);
            if (comps != null)
            {
                foreach (Component comp in comps)
                    comp.OnDelete();
            }
        }

        public GameObject GetObjectByName(string name)
        {
            foreach (KeyValuePair<int, GameObject> pair in _gameObjects)
            {
                if (pair.Value.name == name) return pair.Value;
            }
            return null;
        }

        public List<GameObject> GetObjectsByTag(string tag)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (KeyValuePair<int, GameObject> pair in _gameObjects)
            {
                if (pair.Value.tag.GetHashCode() == tag.GetHashCode()) objects.Add(pair.Value);
            }
            return objects;
        }

        /// <summary>
        /// Deletes Objects after set for deletion
        /// throw catch here
        /// </summary>
        public void startDeletion()
        {
            foreach(int id in _deletion)
            {
                if(GetComponent<Sprite>(id) != null)
                    _quadtree.RemoveObject(GetObject(id));
                bool buffer = _gameObjects.Remove(id);
                if (buffer) ClearComponents(id);
                _availableID.Add(id);
            }
            _deletion.Clear();
        }

        public void addVolumeToUpdate(GameObject obj)
        {
            if (!_updateVolumes.ContainsKey(obj.Id) && obj.sprite != null)
                _updateVolumes.Add(obj.Id, obj.transform.position);
        }

        public void startVolumeUpdate()
        {
            foreach (KeyValuePair<int, Vector3> pair in _updateVolumes)
                _quadtree.UpdateObject(GetObject(pair.Key), pair.Value);
            _updateVolumes.Clear();
        }

        #endregion

        #region Component Functions

        public bool AddComponent<T>(GameObject obj, Component component) where T : Component
        {
            int CompID = _compTypes.GetId<T>();
            Dictionary<int, Component> components;
            if (!_components.TryGetValue(CompID, out components))
            {
                components = new Dictionary<int, Component>();
                _components.Add(CompID, components);
            }
            if (!components.ContainsKey(obj.Id))
            {
                components.Add(obj.Id, component);
                return true;
            }
            return false;
        }

        public bool AddComponent(GameObject obj, Component component)
        {
            int CompID = _compTypes.GetTypeFor(component.GetType()).Id;
            Dictionary<int, Component> components;
            if (!_components.TryGetValue(CompID, out components))
            {
                components = new Dictionary<int, Component>();
                _components.Add(CompID, components);
            }
            if (!components.ContainsKey(obj.Id))
            {
                components.Add(obj.Id, component);
                return true;
            }
            return false;
        }

        public Component GetComponent(GameObject obj, Type type)
        {
            Component component = null;
            int CompID = _compTypes.GetTypeFor(type).Id;
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                components.TryGetValue(obj.Id, out component);
            }
            return component;
        }

        public Component GetComponent<T>(GameObject obj) where T : Component
        {
            Component component = null;
            int CompID = _compTypes.GetId<T>();
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                components.TryGetValue(obj.Id, out component);
            }
            return component;
        }

        public Component GetComponent(int id, Type type)
        {
            Component component = null;
            int CompID = _compTypes.GetTypeFor(type).Id;
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                components.TryGetValue(id, out component);
            }
            return component;
        }

        public Component GetComponent<T>(int id) where T : Component
        {
            Component component = null;
            int CompID = _compTypes.GetId<T>();
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                components.TryGetValue(id, out component);
            }
            return component;
        }

        public List<Component> GetComponents(Type type)
        {
            Dictionary<int, Component> components = null;
            int CompID = _compTypes.GetTypeFor(type).Id;
            _components.TryGetValue(CompID, out components);
            return components.Values.ToList();
        }

        public List<Component> GetComponents(GameObject obj)
        {
            List<Component> components = new List<Component>();
            foreach (KeyValuePair<int, Dictionary<int, Component>> pair in _components)
            {
                Component comp = null;
                pair.Value.TryGetValue(obj.Id, out comp);
                if (comp != null) components.Add(comp);
            }
            return components;
        }

        public List<Component> GetComponents(int id)
        {
            List<Component> components = new List<Component>();
            foreach (KeyValuePair<int, Dictionary<int, Component>> pair in _components)
            {
                Component comp = null;
                pair.Value.TryGetValue(id, out comp);
                if (comp != null) components.Add(comp);
            }
            return components;
        }

        public List<Component> GetComponents<T>() where T : Component
        {
            Dictionary<int, Component> components = null;
            int CompID = _compTypes.GetId<T>();
            _components.TryGetValue(CompID, out components);
            if (components == null)
                return null;
            return components.Values.ToList();
        }

        public bool RemoveComponent(GameObject obj, Type type)
        {
            int CompID = _compTypes.GetTypeFor(type).Id;
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                if (components.Remove(obj.Id)) return true;
            }
            return false;
        }

        public bool RemoveComponent<T>(GameObject obj) where T : Component
        {
            int CompID = _compTypes.GetId<T>();
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                if (components.Remove(obj.Id)) return true;
            }
            return false;
        }

        public void ClearComponents(GameObject obj)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components.ElementAt(i).Value.Remove(obj.Id);
            }
        }

        public void ClearComponents(int id)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components.ElementAt(i).Value.Remove(id);
            }
        }

        public bool HasComponent<T>(GameObject obj) where T : Component
        {
            int CompID = _compTypes.GetId<T>();
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                return components.ContainsKey(obj.Id);
            }
            return false; 
        }

        public bool HasComponent(GameObject obj, Type type)
        {
            int CompID = _compTypes.GetTypeFor(type).Id;
            Dictionary<int, Component> components = null;
            if (_components.TryGetValue(CompID, out components))
            {
                return components.ContainsKey(CompID);
            }
            return false;
        }

        #endregion

        public void ObjectUpdate(float Delta, Game game)
        {
            if(_components.Count == 0) return;
            foreach (KeyValuePair<int, Dictionary<int, Component>> pair in _components)
            {
                int count;
                do
                {
                    count = pair.Value.Count;
                    if (pair.Value.Count != 0)
                    {
                        foreach (KeyValuePair<int, Component> component in pair.Value)
                        {
                            (component.Value).Update(Delta, game);
                            if (count != pair.Value.Count)
                                break;
                        }
                    }
                } while (count != pair.Value.Count);
            }
        }

        public void FixedObjectUpdate(float Delta, Game game)
        {
            if (_components.Count == 0) return;
            foreach (KeyValuePair<int, Dictionary<int, Component>> pair in _components)
            {
                int count;
                do
                {
                    count = pair.Value.Count;
                    if (pair.Value.Count != 0)
                    {
                        foreach (KeyValuePair<int, Component> component in pair.Value)
                        {
                            (component.Value).FixedUpdate(Delta, game);
                            if (count != pair.Value.Count)
                                break;
                        }
                    }
                } while (count != pair.Value.Count);
            }
        }

        #endregion
    }

    #endregion

    #region ComponentTypeManager

    public class ComponentTypeManager
    {
        #region Fields

        //current registered component types
        private Dictionary<Type, ComponentType> _componentTypes = new Dictionary<Type,ComponentType>();

        #endregion

        #region Constructors

        #endregion

        #region Functions

        /// <summary>
        /// Gets corresponding ComponentType for Component if already added,
        /// if not added adds new ComponentType
        /// </summary>
        /// <typeparam name="T"> Type of desired component</typeparam>
        /// <returns>the corresponding component type</returns>
        public ComponentType GetTypeFor<T>() where T : Component
        {
            ComponentType type = null;
            Type receivedType = typeof(T);
            if(!_componentTypes.TryGetValue(receivedType,out type))
            {
                type = new ComponentType();
                _componentTypes.Add(receivedType, type);
            }
            return type;
        }

        /// <summary>
        /// Gets corresponding ComponentType for Component if already added,
        /// if not added adds new ComponentType
        /// </summary>
        /// <typeparam name="T"> Type of desired component</typeparam>
        /// <returns>the corresponding component type</returns>
        public ComponentType GetTypeFor(Type component)
        {
            ComponentType type = null;
            if (!_componentTypes.TryGetValue(component, out type))
            {
                type = new ComponentType();
                _componentTypes.Add(component, type);
            }
            return type;
        }

        /// <summary>
        /// Gets corresponding ComponentType for Component and returns id if already added,
        /// if not added adds new ComponentType and returns id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int GetId<T>() where T : Component
        {
            return GetTypeFor<T>().Id;
        }

        #endregion

    }

    #endregion

    #region ComponentType

    public sealed class ComponentType
    {
        #region Fields

        //next available id
        static int _nextId = 0;
        
        private int _id;

        #endregion

        #region Constructors

        public ComponentType()
        {
            increment();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Increases nextId by 1 and sets the value for id
        /// </summary>
        private void increment()
        {
            _id = _nextId++;
        }

        //getter
        public int Id
        {
            get { return _id; }
        }

        #endregion

    }

    #endregion

    #region BaseGameObject

    public class GameObject
    {

        #region Fields

        private GameObjectManager _objectManager;
        private int _id;
        private string _name;
        private string _tag;

        #endregion

        #region Constructors

        public GameObject(int id, GameObjectManager objectManager)
        {
            _id = id;
            _objectManager = objectManager;
            _name = "undef";
            _tag = "undef";
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public Transform transform
        {
            get { return GetComponent<Transform>() as Transform; }
        }

        public Camera camera
        {
            get { return GetComponent<Camera>() as Camera; }
        }

        public Sprite sprite
        {
            get { return GetComponent<Sprite>() as Sprite; }
        }

        public Physic physic
        {
            get { return GetComponent<Physic>() as Physic; }
        }

        public Animation animation
        {
            get { return GetComponent<Animation>() as Animation; }
        }

        public Emitter emitter
        {
            get { return GetComponent<Emitter>() as Emitter; }
        }

        public Temporary temporary
        {
            get { return GetComponent<Temporary>() as Temporary; }
        }

        public Solid solid
        {
            get { return GetComponent<Solid>() as Solid; }
        }

        public UIText uitext
        {
            get { return GetComponent<UIText>() as UIText; }
        }

        public UIButton uibutton
        {
            get { return GetComponent<UIButton>() as UIButton; }
        }

        public UITexture uitexture
        {
            get { return GetComponent<UITexture>() as UITexture; }
        }

        public Audio audio
        {
            get { return GetComponent<Audio>() as Audio; }
        }

        public CharacterMover characterMover
        {
            get { return GetComponent<CharacterMover>() as CharacterMover; }
        }

        #endregion

        #region Functions

        // setter
        public bool AddComponent<T>(Component component) where T : Component
        {
            return _objectManager.AddComponent<T>(this, component);
        }

        public bool AddComponent(Component component)
        {
            return _objectManager.AddComponent(this, component);
        }

        // getter
        public Component GetComponent<T>() where T : Component
        {
            return _objectManager.GetComponent<T>(this);
        }

        public Component GetComponent(Type type)
        {
            return _objectManager.GetComponent(this, type);
        }

        public List<Component> GetComponents()
        {
            return _objectManager.GetComponents(this);
        }

        // checker
        public bool HasComponent<T>() where T : Component
        {
            return _objectManager.HasComponent<T>(this);
        }

        public bool HasComponent(Type type)
        {
            return _objectManager.HasComponent(this, type);
        }

        //deleter
        public bool RemoveComponent<T>() where T : Component
        {
            return _objectManager.RemoveComponent<T>(this);
        }

        public bool RemoveComponent(Type type)
        {
            return _objectManager.RemoveComponent(this, type);
        }

        public void ClearComponents()
        {
            _objectManager.ClearComponents(this);
        }

        public void DeleteObject()
        {
            _objectManager.DeleteObject(_id);
        }

        #endregion

    }

    #endregion

    #region Component

    public interface IComponent
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Methods

        #endregion
    }

    public class Component : IComponent
    {
        #region Fields

        protected GameObjectManager _objectManager;
        int _id;

        #endregion

        #region Constructor

        public Component(GameObjectManager objMan, int id)
        {
            _objectManager = objMan;
            _id = id;
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
        }

        public GameObject gameObject
        {
            get { return _objectManager.GetObject(_id); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// initialize method
        /// method called on component creation
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// update method
        /// code called every update
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void Update(float Delta, Game game)
        {

        }

        /// <summary>
        /// fixed update method
        /// code called every fixed timestep
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void FixedUpdate(float Delta, Game game)
        {

        }

        /// <summary>
        /// On Collison Method
        /// code called on collision
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void OnCollision(GameObject other)
        {

        }

        /// <summary>
        /// On Trigger Enter Method
        /// code called on Entering trigger
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void OnTriggerEnter(GameObject other)
        {

        }

        /// <summary>
        /// On Trigger Stay Method
        /// code called on staying in trigger
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void OnTriggerStay(GameObject other)
        {

        }

        /// <summary>
        /// On Trigger Enter Method
        /// code called on leaving trigger
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void OnTriggerLeave(GameObject other)
        {

        }

        /// <summary>
        /// OnDeleteMethod
        /// code called on gameobject deletion
        /// overwritten by specific component if needed
        /// </summary>
        public virtual void OnDelete()
        {

        }

        #endregion
    }

    #endregion

}