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
    #region WorldSpaceData

    public struct WorldSpaceData
    {
        public GameObject obj;
        public Rectangle world;
        public Texture2D texture;
    }

    #endregion

    #region CollisionData

    public struct CollisionData
    {
        public GameObject obj;
        public Texture2D texture;
        public Rectangle world;
        public Vector3 basePos;
        public List<WorldSpaceData> possible;
        public Rectangle intersect;
        public GameObject lastCol;
        public Vector3 offset;
        //public Vector3 delta2;
        public bool collision;

        /*public Vector3 getCollisionFreePixel1()
        {
            Vector3 pos = obj1.physic.velocity;
            float length = pos.Length();
            Vector2 factor = new Vector2(1f, 1f);
            float scaling = 0f;
            Vector3 p = new Vector3(point.X - world1.Left, point.Y - world1.Bottom, 0f);
            p += pos;
            for(int i = 0; i < length; i++)
            {
                p -= pos * (1f / length) * i;
                if(!(p.X >= texture1.Width || p.Y >= texture1.Height || p.X < 0 || p.Y < 0))
                {
                    if (Sprite.GetPixel(texture1, new Point((int)p.X, (int)p.Y)).A > 0)
                        scaling += p.Length();
                    if ((p - pos).Length() > length)
                        break;
                }
            }

            //turned around for other texture
            p = new Vector3(point.X - world2.Left, point.Y - world2.Bottom, 0f);
            p -= pos;
            for (int i = 0; i < length; i++)
            {
                p += pos * (1f / length) * i;
                if (!(p.X >= texture2.Width || p.Y >= texture2.Height || p.X < 0 || p.Y < 0))
                {
                    if (Sprite.GetPixel(texture2, new Point((int)p.X, (int)p.Y)).A > 0)
                        scaling += p.Length();
                    if ((p + pos).Length() > length)
                        break;
                }
            }

            return Vector3.Normalize(-pos) * scaling;
            //return new Vector3(point.X, point.Y, 0f) - new Vector3(world1.Left + p.X, world1.Bottom - p.Y, 0f);

            //return Vector3.Zero;
        }*/

        /*public Vector3 getCollisionFreePixel2()
        {
            Vector3 pos = obj2.physic.velocity;
            float length = pos.Length();
            Vector2 factor = new Vector2(1f, 1f);
            float scaling = 0f;
            Vector3 p = new Vector3(point.X - world2.Left, point.Y - world2.Bottom, 0f);
            p += pos;
            for (int i = 0; i < length; i++)
            {
                p -= pos * (1f / (length)) * i;
                if (!(p.X >= texture2.Width || p.Y >= texture2.Height || p.X < 0 || p.Y < 0))
                {
                    if (Sprite.GetPixel(texture2, new Point((int)p.X, (int)p.Y)).A > 0)
                        scaling += p.Length();
                    if ((p - pos).Length() > length)
                    {
                        break;
                    }
                }
            }

            //turned around for other texture
            p = new Vector3(point.X - world1.Left, point.Y - world1.Bottom, 0f);
            p -= pos;
            for (int i = 0; i < length; i++)
            {
                p += pos * (1f / (length)) * i;
                if (!(p.X >= texture1.Width || p.Y >= texture1.Height || p.X < 0 || p.Y < 0))
                {
                    if (Sprite.GetPixel(texture1, new Point((int)p.X, (int)p.Y)).A > 0)
                        scaling += p.Length();
                    if ((p + pos).Length() > length)
                    {
                        break;
                    }
                }
            }

            return Vector3.Normalize(-pos) * scaling;
        }*/

        public void doMovement(GameObjectManager objMan, float Delta)
        {
            //obj.physic.HasGroundContact = false;
            basePos = obj.transform.position;
            Vector3 offset = new Vector3(0f, 0f, 0f);
            //Step each axis separately, starting with the one with the largest absolute difference.
            //Integrate acceleration and velocity to compute the desired delta-position vector (how much to move in each axis)
            if (obj.physic.velocity.X * obj.physic.velocity.X > obj.physic.velocity.Y * obj.physic.velocity.Y)
            {
                HorizontalMovement(Delta);
                VerticalMovement(Delta);
            }
            else
            {
                VerticalMovement(Delta);
                HorizontalMovement(Delta);
            }
            //If, at the end of the movement, any pixel of the character is overlaping with any obstacle, undo the movement on this axis.
            //Regardless of result of last condition, proceed to do the same for the other axis.
            objMan.Quadtree.UpdateObject(obj, basePos);

            //do onCollison
            if (lastCol != null)
            {
                List<Component> comps = obj.GetComponents();
                foreach (Component e in comps)
                    e.OnCollision(lastCol);
                comps = lastCol.GetComponents();
                foreach (Component e in comps)
                    e.OnCollision(obj);
            }
            if (obj.physic.HasGroundContact)
            {
                obj.physic.velocity = new Vector3(0f , 0f, 0f);
            }
        }

        private void HorizontalMovement(float Delta)
        {
            world = obj.sprite.GetWorldBound();
            texture = obj.sprite.GetWorldTexture(flatRender.DeviceManager, flatRender.SpriteBatch, world);
            //Step each axis separately, starting with the one with the largest absolute difference.
            offset.X = obj.physic.velocity.X * Delta;
            //For the horizontal movement, offset the player AABB by 3 pixels to the top, so he can climb slopes.
            //only do this if most of the movement is actually horizontal
            offset.Y = 0f;
            //Scan ahead, by checking against all valid obstacles and the bitmask itself, to determine how many pixels 
            //it is able to move before hitting an obstacle. 
            for (int i = 0; i < possible.Count; i++)
            {
                if (Sprite.CheckCollision(this, possible[i]))
                {
                    offset.Y = 9.81f * 0.25f * Delta;
                    if (Sprite.CheckCollision(this, possible[i]))
                    {
                        lastCol = possible[i].obj;
                        offset.X -= (obj.physic.velocity.X * Delta) / 20;
                        if (obj.physic.velocity.X < 0)
                        {
                            if (offset.X >= 0)
                            {
                                offset.X = 0;
                                break;
                            }
                        }
                        else if (obj.physic.velocity.X >= 0)
                        {
                            if (offset.X <= 0)
                            {
                                offset.X = 0;
                                break;
                            }
                        }
                        i--;
                    }
                }
            }
            // Move to this new position.
            //If this was horizontal movement, move as many pixels up as necessary (which should be up to 3) to make up for slope.
            //if (!obj.physic.HasGroundContact)
               // offset.Y = 0f;
            obj.transform.position += offset;
        }

        private void VerticalMovement(float Delta)
        {
            world = obj.sprite.GetWorldBound();
            texture = obj.sprite.GetWorldTexture(flatRender.DeviceManager, flatRender.SpriteBatch, world);
            offset.X = 0;
            offset.Y = obj.physic.velocity.Y * Delta;
            for (int i = 0; i < possible.Count; i++)
            {
                if (Sprite.CheckCollision(this, possible[i]))
                {
                    lastCol = possible[i].obj;
                    offset.Y -= (obj.physic.velocity.Y * Delta) / 20;
                    if (offset.Y < 0f)
                        obj.physic.HasGroundContact = true;
                    if (obj.physic.velocity.Y < 0)
                    {
                        if (offset.Y >= 0)
                        {
                            offset.Y = 0;
                            break;
                        }
                    }
                    else if (obj.physic.velocity.Y >= 0)
                    {
                        if (offset.Y <= 0)
                        {
                            offset.Y = 0;
                            break;
                        }
                    }
                    i--;
                }
            }
            obj.transform.position += offset;
        }

    }
    

    #endregion 

    #region Bounding Volume

    public class BoundingVolume
    {
        #region fields

        public Vector3 _position;

        public float _radius;

        #endregion

        #region constructor

        public BoundingVolume(float radius, Vector3 pos)
        {
            _position = pos;
            _radius = radius;
        }

        #endregion

        #region methods

        public static bool Intersect(BoundingVolume vol1, BoundingVolume vol2)
        {
            return ((vol1._position - vol2._position).LengthSquared() <= (vol1._radius + vol2._radius) * (vol1._radius + vol2._radius) * 0.5f);
        }

        #endregion
    }

    #endregion

    #region quadtree

    public enum QuadTreeSection { LOWER_LEFT = 0, LOWER_RIGHT = 1, UPPER_LEFT = 2, UPPER_RIGHT = 3};

    public class QuadTree
    {
        #region Fields

        private QuadTreeNode[] _root = new QuadTreeNode[3];

        #endregion

        #region Constructor

        public QuadTree()
        {
            _root[0] = new QuadTreeNode(new Vector2(8192f, 4608f));
            _root[1] = new QuadTreeNode(new Vector2(8192f, 4608f));
            _root[2] = new QuadTreeNode(new Vector2(8192f, 4608f));
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Insert(GameObject obj)
        {
            _root[obj.transform.layer].Insert(obj, obj.sprite.GetBoundingVolume());
        }

        public void UpdateObject(GameObject obj, Vector3 position)
        {
            QuadTreeNode node = _root[obj.transform.layer].FindNodeWith(obj, position);
            if(node != null)
            {
                node.RemoveElement(obj);
                if(obj.sprite != null)
                    node.UpdateElement(obj, obj.sprite.GetBoundingVolume());
            }
            else
                _root[obj.transform.layer].Insert(obj, obj.sprite.GetBoundingVolume());
        }

        public void RemoveObject(GameObject obj)
        {
            QuadTreeNode node = _root[obj.transform.layer].FindNodeWith(obj, obj.transform.position);
            node.RemoveElement(obj);
        }

        public List<GameObject> GetRange(Rectangle rect, int layer)
        {
            List<GameObject> list = new List<GameObject>();
            if (_root[layer].Intersect(rect)) //exception : trying to get out of world range
            {
                _root[layer].GetRange(rect, list);
            }
            return list;
        }

        public List<GameObject> GetRange(Camera cam, int layer)
        {
            switch (layer)
            {
                case 0:
                    return GetRange(new Rectangle((int)(cam.gameObject.transform.position.X - (cam.vision.X * 0.5f * 1.5f)),
                    (int)(cam.gameObject.transform.position.Y - (cam.vision.Y * 0.5f * 1.5f)),
                    (int)cam.vision.X,
                    (int)cam.vision.Y), layer);
                case 1:
                    return GetRange(new Rectangle((int)(cam.gameObject.transform.position.X - (cam.vision.X * 0.5f)),
                    (int)(cam.gameObject.transform.position.Y - (cam.vision.Y * 0.5f)),
                    (int)cam.vision.X,
                    (int)cam.vision.Y), layer);
                case 2:
                    return GetRange(new Rectangle((int)(cam.gameObject.transform.position.X - (cam.vision.X * 0.5f)),
                    (int)(cam.gameObject.transform.position.Y - (cam.vision.Y * 0.5f)),
                    (int)cam.vision.X,
                    (int)cam.vision.Y), layer);
                default:
                    return null;
            }
            
        }

        public List<T> GetRange<T>(Rectangle rect, int layer) where T: Component
        {
            List<T> comps = new List<T>();
            List<GameObject> list = new List<GameObject>();
            if (_root[layer].Intersect(rect)) //exception : trying to get out of world range
            {
                _root[layer].GetRange(rect, list);
            }
            foreach(GameObject e in list)
            {
                if(e.GetComponent<T>() != null)
                    comps.Add(e.GetComponent<T>() as T);
            }
            return comps;
        }

        public CollisionData GetPotentialsContacts(GameObject obj)
        {
            return _root[obj.transform.layer].FindNodeWith(obj, obj.transform.position).GetPotentialContacts(new CollisionData(), obj);
        }

        #endregion
    }

    #endregion

    #region QuadTreeNode

    public class QuadTreeNode
    {
        #region Fields

        private Vector3 _position;
        private Rectangle _dimensions;

        float _minWidth = 64f;

        private QuadTreeNode _parent;
        private QuadTreeNode[] _children = new QuadTreeNode[4];
        private Dictionary<int, GameObject> _elements = new Dictionary<int, GameObject>();

        #endregion

        #region Constructor

        public QuadTreeNode(Vector2 dimensions)
        {
            _parent = null;
            _dimensions = new Rectangle(0 - (int)(dimensions.X * 0.5f), 0 - (int)(dimensions.Y * 0.5f), (int)dimensions.X, (int)dimensions.Y);
            _position = Vector3.Zero;
            _children[0] = null;
            _children[1] = null;
            _children[2] = null;
            _children[3] = null;
        }
        
        public QuadTreeNode(QuadTreeNode parent, Vector3 position)
        {
            _parent = parent;
            _position = position;
            _dimensions = new Rectangle((int)(position.X - (parent._dimensions.Width * 0.25f)), 
                (int)(position.Y - (parent._dimensions.Height * 0.25f)), 
                (int)(parent._dimensions.Width * 0.5f), 
                (int)(parent._dimensions.Height * 0.5f));
            _children[0] = null;
            _children[1] = null;
            _children[2] = null;
            _children[3] = null;
            
        }

        #endregion

        #region Properties

        public bool IsLeaf
        {
            get { return (_children[0] == null && _children[1] == null && _children[2] == null && _children[3] == null); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new node to the passed node
        /// </summary>
        /// <param name="node"> node the node should be added to </param>
        /// <param name="section"> QuadTreeSection of the new node </param>
        /// <returns> new node </returns>
        static QuadTreeNode AddNode(QuadTreeNode node, int section)
        {
            switch (section)
            {
                case (int)QuadTreeSection.LOWER_LEFT:
                    return new QuadTreeNode(node, node._position + new Vector3(-node._dimensions.Width * 0.25f, -node._dimensions.Height * 0.25f, 0f));
                case (int)QuadTreeSection.LOWER_RIGHT:
                    return new QuadTreeNode(node, node._position + new Vector3(node._dimensions.Width * 0.25f, -node._dimensions.Height * 0.25f, 0f));
                case (int)QuadTreeSection.UPPER_LEFT:
                    return new QuadTreeNode(node, node._position + new Vector3(-node._dimensions.Width * 0.25f, node._dimensions.Height * 0.25f, 0f));
                case (int)QuadTreeSection.UPPER_RIGHT:
                    return new QuadTreeNode(node, node._position + new Vector3(node._dimensions.Width * 0.25f, node._dimensions.Height * 0.25f, 0f));
                default:
                    return null;
            }
        }

        public void GetRange(Rectangle rect, List<GameObject> list)
        {
            if (IsLeaf)
            {
                if(_elements.Count != 0)
                    list.AddRange(_elements.Values.ToList());
            }
            else 
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_children[i].Intersect(rect))
                        if (_children[i].Envelops(rect))
                            _children[i].AddTillLeaf(list);
                        else
                            _children[i].GetRange(rect, list);
                }
                list.AddRange(_elements.Values.ToList());
            }
        }

        public void AddTillLeaf(List<GameObject> list)
        {
            if (IsLeaf)
            {
                if (_elements.Count != 0)
                    list.AddRange(_elements.Values.ToList());
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    _children[i].AddTillLeaf(list);
                }
                list.AddRange(_elements.Values.ToList());
            }
        }

        public bool Intersect(Rectangle rect)
        {
            return !(rect.Right <= _dimensions.Left || rect.Left >= _dimensions.Right || rect.Bottom <= _dimensions.Top || rect.Top >= _dimensions.Bottom);
        }

        public bool Envelops(Rectangle rect)
        {
            return (rect.Left <= _dimensions.Left && rect.Right >= _dimensions.Right && rect.Top <= _dimensions.Top && rect.Bottom >= _dimensions.Bottom);
        }

        public void Insert(GameObject obj, BoundingVolume vol)
        {
            if (IsLeaf)
            {
                if (_dimensions.Width == _minWidth)
                {
                    if(!_elements.ContainsKey(obj.Id))
                        _elements.Add(obj.Id, obj);
                    return;
                }
                else
                {
                    _children[0] = AddNode(this, 0);
                    _children[1] = AddNode(this, 1);
                    _children[2] = AddNode(this, 2);
                    _children[3] = AddNode(this, 3);
                }
            }
            if (_children[0].InSection(vol))
                _children[0].Insert(obj, vol);
            else if (_children[1].InSection(vol))
                _children[1].Insert(obj, vol);
            else if (_children[2].InSection(vol))
                _children[2].Insert(obj, vol);
            else if (_children[3].InSection(vol))
                _children[3].Insert(obj, vol);
            else
            {
                if(!_elements.ContainsKey(obj.Id))
                    _elements.Add(obj.Id, obj);
            }
        }

        public bool InSection(BoundingVolume vol)
        {
            return !(vol._position.X - vol._radius <= _dimensions.Left||
                vol._position.Y - vol._radius <= _dimensions.Top||
                vol._position.X + vol._radius >= _dimensions.Right ||
                vol._position.Y + vol._radius >= _dimensions.Bottom);
        }

        public bool InSection(Vector3 position)
        {
            return !(position.X <= _dimensions.Left ||
                position.Y <= _dimensions.Top ||
                position.X >= _dimensions.Right ||
                position.Y >= _dimensions.Bottom);
        }

        public QuadTreeNode FindNodeWith(GameObject obj, Vector3 position)
        {
            if (_elements.ContainsKey(obj.Id))
                return this;
            else if (IsLeaf)
            {
                return null;
            }
            QuadTreeNode buffer;
            foreach (QuadTreeNode node in _children)
            {
                if (node.InSection(position))
                {
                    buffer = node.FindNodeWith(obj, position);
                    if (buffer != null)
                        return buffer;
                }
            }
            return null;
        }

        public void RemoveElement(GameObject obj)
        {
            if (_elements.ContainsKey(obj.Id))
                _elements.Remove(obj.Id);
        }

        public void UpdateElement(GameObject obj, BoundingVolume vol)
        {
            if (!this.InSection(vol))
            {
                if (_parent != null)
                    _parent.UpdateElement(obj, vol);
                else
                    _elements.Add(obj.Id, obj);
            }
            else
                Insert(obj, vol);
        }

        public CollisionData GetPotentialContacts(CollisionData data, GameObject obj)
        {
            data.obj = obj;
            data.possible = new List<WorldSpaceData>();

            foreach (KeyValuePair<int, GameObject> pair in _elements)
            {
                if (data.obj.Id != pair.Key && pair.Value.physic != null)
                {
                    if (pair.Value.physic.IsRigid)
                    {
                        WorldSpaceData d = new WorldSpaceData();
                        d.obj = pair.Value;
                        data.possible.Add(d);
                    }
                }
            }
            if (!IsLeaf)
            {
                for (int i = 0; i < 4; i++)
                {
                    _children[i].GetPotentialContactsWithChild(data);
                }
            }
            if(_parent != null)
                _parent.GetPotentialContactsWithParent(data);
            return data;
        }

        public void GetPotentialContactsWithChild(CollisionData data)
        {
            foreach (KeyValuePair<int, GameObject> pair in _elements)
            {
                if (data.obj.Id != pair.Key && pair.Value.physic != null)
                {
                    if (pair.Value.physic.IsRigid)
                    {
                        WorldSpaceData d = new WorldSpaceData();
                        d.obj = pair.Value;
                        data.possible.Add(d);
                    }
                }
            }
            if (IsLeaf)
                return;
            for (int i = 0; i < 4; i++)
            {
                _children[i].GetPotentialContactsWithChild(data);
            }
        }

        public void GetPotentialContactsWithParent(CollisionData data)
        {
            foreach (KeyValuePair<int, GameObject> pair in _elements)
            {
                if (data.obj.Id != pair.Key && pair.Value.physic != null)
                {
                    if (pair.Value.physic.IsRigid)
                    {
                        WorldSpaceData d = new WorldSpaceData();
                        d.obj = pair.Value;
                        data.possible.Add(d);
                    }
                }
            }
            if (_parent != null)
                _parent.GetPotentialContactsWithParent(data);
        }

        #endregion
    }

    #endregion

    #region UIPosition
    public enum alignment
    {
        UpperLeft, 
        UpperCenter, 
        UpperRight, 
        CenterLeft, 
        Center, 
        CenterRight, 
        LowerLeft, 
        LowerCenter, 
        LowerRight
    }

    public class UIPosition
    {
        #region Field

        Rectangle _res;
        Vector2 _alignment;
        alignment _cur;

        #endregion

        #region Constructor

        public UIPosition(Rectangle res)
        {
            Dimensions = res;
            setAlignment(alignment.Center);
        }

        #endregion

        #region Properties

        public Vector2 UpperLeft
        {
            get { return new Vector2(0, 0); }
        }
        public Vector2 UpperCenter
        {
            get { return new Vector2(_res.Width * 0.5f, 0); }
        }
        public Vector2 UpperRight
        {
            get { return new Vector2(_res.Width, 0); }
        }
        public Vector2 CenterLeft
        {
            get { return new Vector2(0, _res.Height*0.5f); }
        }
        public Vector2 Center
        {
            get { return new Vector2(_res.Width * 0.5f, _res.Height*0.5f); }
        }
        public Vector2 CenterRight
        {
            get { return new Vector2(_res.Width, _res.Height * 0.5f); }
        }
        public Vector2 LowerLeft
        {
            get { return new Vector2(0, _res.Height); }
        }
        public Vector2 LowerCenter
        {
            get { return new Vector2(_res.Width * 0.5f, _res.Height); }
        }
        public Vector2 LowerRight
        {
            get { return new Vector2(_res.Width, _res.Height); }
        }

        public Vector2 Alignment
        {
            get { return _alignment; }
        }

        public Rectangle Dimensions
        {
            get { return _res; }
            set
            {
                value.X = 0;
                value.Y = 0;
                if (value.Width <= 0)
                    value.Width = 1;
                if (value.Height <= 0)
                    value.Height = 1;
                _res = value;
                setAlignment(_cur);
            }
        }

        #endregion

        #region Methods

        public void setAlignment(alignment align)
        {
            switch (align)
            {
                case alignment.UpperLeft:
                    _cur = align;
                    _alignment = new Vector2(0, 0);
                    break;
                case alignment.UpperCenter:
                    _cur = align;
                    _alignment = new Vector2(_res.Width * 0.5f, 0);
                    break;
                case alignment.UpperRight:
                    _cur = align;
                    _alignment = new Vector2(_res.Width, 0);
                    break;
                case alignment.CenterLeft:
                    _cur = align;
                    _alignment = new Vector2(0, _res.Height*0.5f);
                    break;
                case alignment.Center:
                    _cur = align;
                    _alignment = new Vector2(_res.Width * 0.5f, _res.Height*0.5f);
                    break;
                case alignment.CenterRight:
                    _cur = align;
                    _alignment = new Vector2(_res.Width, _res.Height*0.5f);
                    break;
                case alignment.LowerLeft:
                    _cur = align;
                    _alignment = new Vector2(0, _res.Height);
                    break;
                case alignment.LowerCenter:
                    _cur = align;
                    _alignment = new Vector2(_res.Width*0.5f, _res.Height);
                    break;
                case alignment.LowerRight:
                    _cur = align;
                    _alignment = new Vector2(_res.Width, _res.Height);
                    break;
            }

        }

        #endregion
    }

    #endregion

}