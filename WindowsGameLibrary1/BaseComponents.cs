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
    #region Components

    #region transform

    public class Transform : Component
    {

        #region Fields

        private Vector3 _position;
        private Vector3 _scale;
        private Vector3 _rotation;
        private int _layer;

        #endregion

        #region Constructor

        public Transform(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            _position = Vector3.Zero;
            _scale = Vector3.One;
            _rotation = Vector3.Zero;
            _layer = 1;
        }

        public Transform(GameObjectManager objMan, int id, Vector3 position, Vector3 scale, Vector3 rotation, int layer)
            : base(objMan, id)
        {
            _position = position;
            _scale = scale;
            _rotation = rotation;
            _layer = layer;
        }

        #endregion

        #region Properties

        public Vector3 position
        {
            get { return _position; }
            set
            {
                _objectManager.addVolumeToUpdate(gameObject);
                _position = value;
            }
        }

        public Vector3 scale
        {
            get { return _scale; }
            set
            {
                _objectManager.addVolumeToUpdate(gameObject);
                _scale = value;
            }
        }

        public Vector3 rotation
        {
            get { return _rotation; }
            set
            {
                _objectManager.addVolumeToUpdate(gameObject);
                _rotation = value;
            }
        }

        public int layer
        {
            get { return _layer; }
            set
            {
                if (value < 0) _layer = 0;
                else if (value > 2) _layer = 2;
                else _layer = value;
            }
        }

        #endregion

        #region functions

        public void setPosition(float x, float y, float z)
        {
            _objectManager.addVolumeToUpdate(gameObject);
            _position = new Vector3(x, y, z);
        }

        public void setScale(float x, float y, float z)
        {
            _objectManager.addVolumeToUpdate(gameObject);
            _scale = new Vector3(x, y, z);
        }

        public void setRotation(float x, float y, float z)
        {
            _objectManager.addVolumeToUpdate(gameObject);
            _rotation = new Vector3(x, y, z);
        }

        #endregion

    }

    #endregion

    #region Camera

    public class Camera : Component
    {
        #region Fields

        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private Vector2 _vision;
        private Vector2 _viewport;

        #endregion

        #region Constructor

        public Camera(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            _viewMatrix = Matrix.Identity;
            _projectionMatrix = Matrix.Identity;
        }

        #endregion

        #region Properties

        public Vector2 viewport
        {
            get { return _viewport; }
        }

        public Vector2 vision
        {
            get { return _vision; }
        }

        public Matrix viewMatrix
        {
            get { return _viewMatrix; }
        }

        public Matrix projectionMatrix
        {
            get { return _projectionMatrix; }
        }

        public Matrix viewProjectionMatrix
        {
            get { return _viewMatrix * _projectionMatrix; }
        }

        #endregion

        #region methods

        public void UpdateProjections(float x, float y)
        {
            float factor = y / x;
            _viewport = new Vector2(x, y);
            _vision = new Vector2(128f, 128f * factor);
        }

        public void UpdateViewMatrix()
        {
            _viewMatrix = Matrix.CreateScale(gameObject.transform.scale)
                * Matrix.CreateScale(new Vector3(1f, -1f, 1f)) //flip coodinate System
                * Matrix.CreateRotationZ(gameObject.transform.rotation.Z)
                * Matrix.CreateTranslation(new Vector3(-gameObject.transform.position.X, gameObject.transform.position.Y, 0.0f))
                * Matrix.CreateTranslation(new Vector3(_vision.X * 0.5f, _vision.Y * 0.5f, 0.0f));
        }

        public void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix.CreateScale(new Vector3(_viewport.X / _vision.X, _viewport.Y / _vision.Y, 1.0f));
        }

        public void ToPosition()
        {
            GameObject obj = _objectManager.GetObjectByName("Character");
            if (obj != null)
                gameObject.transform.position = obj.transform.position;
        }

        #endregion

    }

    #endregion

    #region sprite

    public class Sprite : Component
    {

        #region Fields

        private Texture2D _texture2D;
        private Color _color;
        private bool _useAlpha;
        private Rectangle _source;
        private float _UnitToPixelRatio;
        private SpriteEffects _spriteEffects = SpriteEffects.FlipVertically;

        #endregion

        #region Constructor

        public Sprite(GameObjectManager objMan, int id, Texture2D texture, int texelsPerUnit)
            : base(objMan, id)
        {
            _color = Color.White;
            _texture2D = texture;
            _useAlpha = true;
            SetUPRatio(texelsPerUnit);
            _source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        #endregion

        #region Properties

        // accessors
        public bool alpha
        {
            get { return _useAlpha; }
            set { _useAlpha = value; }
        }

        public Texture2D texture2D
        {
            get { return _texture2D; }
            set
            {
                _texture2D = value;
                _source = new Rectangle(0, 0, _texture2D.Width, _texture2D.Height);
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Rectangle source
        {
            get { return _source; }
            set
            {
                if (_texture2D != null)
                {
                    if (value.Width == 0
                        || value.Width + value.X > _texture2D.Width
                        || value.Height == 0
                        || value.Height + value.Y > _texture2D.Height)
                    {
                        _source = new Rectangle(0, 0, _texture2D.Width, _texture2D.Height);
                    }
                    else
                        _source = value;
                }
            }
        }

        public SpriteEffects SpriteEffects
        {
            set { _spriteEffects = value; }
        }

        #endregion

        #region Methods

        #region component specific

        /// <summary>
        /// Draws the Sprite using sprite batch
        /// </summary>
        /// <param name="spriteBatch"> Sprite Batch Duh!</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Transform transform = gameObject.transform;
            spriteBatch.Draw(_texture2D,
                new Vector2(transform.position.X, transform.position.Y),
                _source,
                _color,
                transform.rotation.Z,
                new Vector2(_source.Width * 0.5f, _source.Height * 0.5f),
                new Vector2(transform.scale.X * _UnitToPixelRatio, transform.scale.Y * _UnitToPixelRatio),
                _spriteEffects,
                transform.position.Z);
        }

        public Texture2D GetWorldTexture(GraphicsDeviceManager devMan, SpriteBatch spriteBatch)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Rectangle bound = GetWorldBound();

            // set new render target
            RenderTarget2D collisionRenderTarget = new RenderTarget2D(devMan.GraphicsDevice, (int)(bound.Width), (int)(bound.Height));
            spriteBatch.GraphicsDevice.SetRenderTarget(collisionRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            //render
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullClockwise, null, Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, bound.Height, 0f));

            spriteBatch.Draw(_texture2D,
                new Vector2(bound.Width * 0.5f, bound.Height * 0.5f),
                _source,
                Color.White,
                transform.rotation.Z,
                new Vector2(_source.Width * 0.5f, _source.Height * 0.5f),
                new Vector2(transform.scale.X * _UnitToPixelRatio, transform.scale.Y * _UnitToPixelRatio),
                _spriteEffects,
                0f);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            // return Render Target as Texture
            return collisionRenderTarget;
        }

        public Texture2D GetWorldTexture(GraphicsDeviceManager devMan, SpriteBatch spriteBatch, Rectangle worldBound)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Rectangle bound = worldBound;

            // set new render target
            RenderTarget2D collisionRenderTarget = new RenderTarget2D(devMan.GraphicsDevice, (int)(bound.Width), (int)(bound.Height));
            spriteBatch.GraphicsDevice.SetRenderTarget(collisionRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            //render
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullClockwise, null, Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, bound.Height, 0f));

            spriteBatch.Draw(_texture2D,
                new Vector2(bound.Width * 0.5f, bound.Height * 0.5f),
                _source,
                _color,
                transform.rotation.Z,
                new Vector2(_source.Width * 0.5f, _source.Height * 0.5f),
                new Vector2(transform.scale.X * _UnitToPixelRatio, transform.scale.Y * _UnitToPixelRatio),
                _spriteEffects,
                0f);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            // return Render Target as Texture
            return collisionRenderTarget;
        }

        public Rectangle GetWorldBound()
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Vector2[] bounds = new Vector2[4];

            bounds[0] = new Vector2(0f, source.Height);
            bounds[1] = new Vector2(source.Width, source.Height);
            bounds[2] = new Vector2(0f, 0f);
            bounds[3] = new Vector2(source.Width, 0f);

            Matrix rotation = Matrix.CreateRotationZ(transform.rotation.Z);
            Matrix scaled = Matrix.CreateScale(transform.scale * _UnitToPixelRatio);
            Matrix translateTo = Matrix.CreateTranslation(-source.Width * 0.5f, -source.Height * 0.5f, 0f);
            Matrix translateBack = Matrix.CreateTranslation(transform.position.X, transform.position.Y, 0f);
            Matrix transMatrix = translateTo * rotation * scaled * translateBack;

            bounds[0] = Vector2.Transform(bounds[0], transMatrix);
            bounds[1] = Vector2.Transform(bounds[1], transMatrix);
            bounds[2] = Vector2.Transform(bounds[2], transMatrix);
            bounds[3] = Vector2.Transform(bounds[3], transMatrix);

            Vector2 min = new Vector2(MathHelper.Min(MathHelper.Min(bounds[0].X, bounds[1].X), MathHelper.Min(bounds[2].X, bounds[3].X)),
                MathHelper.Min(MathHelper.Min(bounds[0].Y, bounds[1].Y), MathHelper.Min(bounds[2].Y, bounds[3].Y)));
            Vector2 max = new Vector2(MathHelper.Max(MathHelper.Max(bounds[0].X, bounds[1].X), MathHelper.Max(bounds[2].X, bounds[3].X)),
                MathHelper.Max(MathHelper.Max(bounds[0].Y, bounds[1].Y), MathHelper.Max(bounds[2].Y, bounds[3].Y)));

            Rectangle bound = new Rectangle((int)min.X, (int)min.Y, (int)max.X - (int)min.X, (int)max.Y - (int)min.Y);

            return bound;
        }

        public BoundingVolume GetBoundingVolume()
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Vector2[] bounds = new Vector2[4];

            bounds[0] = new Vector2(0f, source.Height);
            bounds[1] = new Vector2(source.Width, source.Height);

            Matrix rotation = Matrix.CreateRotationZ(transform.rotation.Z);
            Matrix scaled = Matrix.CreateScale(transform.scale * _UnitToPixelRatio);
            Matrix translateTo = Matrix.CreateTranslation(-source.Width * 0.5f, -source.Height * 0.5f, 0f);
            Matrix translateBack = Matrix.CreateTranslation(transform.position.X, transform.position.Y, 0f);
            Matrix transMatrix = translateTo * rotation * scaled;

            bounds[0] = Vector2.Transform(bounds[0], transMatrix);
            bounds[1] = Vector2.Transform(bounds[1], transMatrix);

            float radius;
            if (bounds[0].LengthSquared() >= bounds[1].LengthSquared())
                radius = bounds[0].Length();
            else
                radius = bounds[1].Length();

            return new BoundingVolume(radius, transform.position);
        }

        public static bool CheckCollision(CollisionData data, WorldSpaceData other)
        {
            if (other.world.Width == 0)
                other.world = other.obj.sprite.GetWorldBound();

            //get intersect rectangle
            int left = (int)MathHelper.Max(data.world.X + data.offset.X, other.world.X);
            int top = (int)MathHelper.Max(data.world.Y + data.offset.Y, other.world.Y);
            int right = (int)MathHelper.Min(data.world.Width + data.offset.X + data.world.X, other.world.Width + other.world.X);
            int bottom = (int)MathHelper.Min(data.world.Y + data.offset.Y + data.world.Height, other.world.Y + other.world.Height);

            if (right - left <= 0 || bottom - top <= 0)
                return false;

            data.intersect = new Rectangle(left, top, right - left, bottom - top);

            if(other.texture == null)
                other.texture = other.obj.sprite.GetWorldTexture(flatRender.DeviceManager, flatRender.SpriteBatch, other.world);

            //get color data
            Color[] color1 = new Color[data.texture.Width * data.texture.Height];
            data.texture.GetData(color1);
            Color[] color2 = new Color[other.texture.Width * other.texture.Height];
            other.texture.GetData(color2);

            for (int y = 0; y < data.intersect.Height; y++)
            {
                for (int x = 0; x < data.intersect.Width; x++)
                {
                    Vector2 coord1 = new Vector2(x + (int)((data.intersect.Left - (data.world.Left + data.offset.X))), y + (((int)(data.world.Bottom + data.offset.Y) - data.intersect.Bottom)));
                    Vector2 coord2 = new Vector2(x + ((data.intersect.Left - other.world.Left)), y + ((other.world.Bottom - data.intersect.Bottom)));
                    if (coord1.X < 0)
                        coord1.X = 0;
                    if (coord1.Y < 0)
                        coord1.Y = 0;
                    if (coord1.X >= data.texture.Width)
                        coord1.X = data.texture.Width -1;
                    if (coord1.Y >= data.texture.Height)
                        coord1.Y = data.texture.Height - 1;
                    int Pos1 = (int)(coord1.X + ((int)coord1.Y * data.texture.Width));
                    int Pos2 = (int)(coord2.X + ((int)coord2.Y * other.texture.Width));
                    if (color1[Pos1].A > 40
                        &&
                        color2[Pos2].A > 40)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public static Color GetPixel (Texture2D tex, Point pos)
        {
            Color[] color = new Color[1];
            if (pos.X >= tex.Width || pos.Y >= tex.Height || pos.X < 0 || pos.Y < 0)
            {
                return Color.Transparent;
            }
            tex.GetData<Color>(0, new Rectangle(pos.X, pos.Y, 1, 1), color, 0, 1);
            return color[0];
        }

        public void SetUPRatio(int pixels)
        {
            _objectManager.addVolumeToUpdate(gameObject);
            _UnitToPixelRatio = 1f / (float)pixels;
        }
        #endregion

        #region base overloaded

        public override void Initialize()
        {
            _objectManager.Quadtree.Insert(gameObject);
            base.Initialize();
        }

        #endregion

        #endregion

    }

    #endregion

    #region animation

    public class Animation : Component
    {
        #region Fields

        private Dictionary<int, Texture2D> _animations = new Dictionary<int, Texture2D>();
        private Dictionary<int, Rectangle> _dimensions = new Dictionary<int, Rectangle>();
        private Dictionary<int, int> _frames = new Dictionary<int, int>();

        private int _fps;
        private float _frametime;

        private int _curAnim;
        private int _curFrame;
        private float _elapsed;

        #endregion

        #region Constructor

        public Animation(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            _fps = 1;
            _frametime = 1f / (float)_fps;
            _curFrame = 1;
            _elapsed = 0f;
            _curAnim = 1;
        }

        #endregion

        #region Properties

        public int FPS
        {
            get { return _fps; }
            set
            {
                _fps = value;
                if (_fps <= 0)
                {
                    _frametime = 0f;
                }
                else
                    _frametime = 1f / (float)_fps;
            }
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            _elapsed += Delta;
            if (_elapsed >= _frametime && _frametime != 0f)
            {
                Sprite sprite = gameObject.sprite;

                int buffer;
                Rectangle animRect;

                if (_frames.TryGetValue(_curAnim, out buffer) && _dimensions.TryGetValue(_curAnim, out animRect))
                {
                    if (_curFrame != buffer)
                    {
                        Rectangle source = sprite.source;
                        source.X += animRect.Width;
                        sprite.source = source;
                        _curFrame++;
                    }
                    else
                    {
                        sprite.source = animRect;
                        _curFrame = 1;
                    }
                    _elapsed = 0f;
                }
            }

            base.Update(Delta, game);
        }

        /// <summary>
        /// Adds an animation to the object
        /// </summary>
        /// <param name="anim"> Texture2D spritesheet of animation </param>
        /// <param name="name"> name of the animation e.g. walk, idle, etc.</param>
        /// <param name="dimension">
        /// Rectangle that specifies the Width and Height of a frame
        /// the rectangles upper left corner is always forced to be 0
        /// /param>
        /// <returns>
        /// true if successfull, false if failed
        /// </returns>
        public bool AddAnimation(Texture2D anim, string name, Rectangle dimension)
        {
            if (anim == null && name == null)
                return false;
            else if (dimension.Width > anim.Width || dimension.Height > anim.Height)
                return false;
            int id = name.GetHashCode();
            _animations.Add(id, anim);
            _dimensions.Add(id, dimension);
            _frames.Add(id, anim.Width / dimension.Width);
            return true;
        }

        /// <summary>
        /// Sets another animation as sprite
        /// resets source rectangle of sprite and elapsed animation time
        /// </summary>
        /// <param name="name"> 
        /// name of animation as string 
        /// </param>
        /// <returns> 
        /// true if successfull, false if failed
        /// </returns>
        public bool SetAnimation(string name)
        {
            int id = name.GetHashCode();
            Texture2D anim;
            Rectangle source;

            if (_animations.TryGetValue(id, out anim) && _dimensions.TryGetValue(id, out source))
            {
                _objectManager.addVolumeToUpdate(gameObject);
                gameObject.sprite.texture2D = anim;
                gameObject.sprite.source = source;
                _curAnim = id;
                _curFrame = 1;
                _elapsed = 0f;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets another animation as sprite
        /// resets source rectangle of sprite and elapsed animation time
        /// </summary>
        /// <param name="id">
        /// hashcode of animation name string
        /// </param>
        /// <returns>
        /// true if successfull, false if failed
        /// </returns>
        public bool SetAnimation(int id)
        {
            Texture2D anim;
            Rectangle source;
            if (_animations.TryGetValue(id, out anim) && _dimensions.TryGetValue(id, out source))
            {
                _objectManager.addVolumeToUpdate(gameObject);
                gameObject.sprite.texture2D = anim;
                gameObject.sprite.source = source;
                _curAnim = id;
                _curFrame = 1;
                _elapsed = 0f;
                return true;
            }
            return false;
        }

        #endregion
    }

    #endregion

    #region emitter

    public class Emitter : Component
    {

        #region Fields
        //sprite
        private Texture2D _particle;
        private Color _color;
        private static Color _random = new Color(123, 123, 123, 0);
        private float _scale;
        //animation
        private bool _anim;
        private Rectangle _source;
        private int _frames;
        private int _fps;
        //physics
        private float _velocity;
        private Vector3 _direction;
        private Vector3 _angle;
        private bool _collides;
        private bool _gravity;
        //lifetime
        private float _lifetime;
        //emitter vars
        private float _pps;
        private float _elapsed;

        #endregion

        #region Constructors

        /// <summary>
        /// contructor for nonanimated particles without gravity or movement
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, float scale, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = Color.White;
            _pps = 1f / amount;
            _collides = false;
            _velocity = 0f;
            _direction = Vector3.Zero;
            Angle = 0f;
            _gravity = false;
            _anim = false;
        }

        /// <summary>
        /// contructor for colortinted nonanimated particles without gravity or movement
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, float scale, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _pps = 1f / amount;
            _collides = false;
            _velocity = 0f;
            Angle = 0f;
            _direction = Vector3.Zero;
            _gravity = false;
            _anim = false;
        }

        /// <summary>
        /// contructor for colortinted animated particles without gravity or movement
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="frames"> number of frames if 0 not animated </param>
        /// <param name="fps"> frames per second if 0 not animated </param>
        /// <param name="source"> source rectangle for frame dimensions </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, int frames, int fps, Rectangle source, float scale, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _frames = frames;
            _pps = 1f / amount;
            _collides = false;
            _velocity = 0f;
            _direction = Vector3.Zero;
            Angle = 0f;
            _gravity = false;
            _anim = true;
            _fps = fps;
            _source = source;
        }

        /// <summary>
        /// contructor for colortinted animated particles with random movement at a certain speed
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="frames"> number of frames if 0 not animated </param>
        /// <param name="fps"> frames per second if 0 not animated </param>
        /// <param name="source"> source rectangle for frame dimensions </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="velocity"> speed of particle </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, int frames, int fps, Rectangle source, float scale, float velocity, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _frames = frames;
            _pps = 1f / amount;
            _collides = false;
            _velocity = velocity;
            _direction = Vector3.Zero;
            Angle = 0f;
            _gravity = false;
            _anim = true;
            _fps = fps;
            _source = source;
        }

        /// <summary>
        /// contructor for colortinted animated particles with movement at specified speed in specified direction
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="frames"> number of frames if 0 not animated </param>
        /// <param name="fps"> frames per second if 0 not animated </param>
        /// <param name="source"> source rectangle for frame dimensions </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="velocity"> speed of particle </param>
        /// <param name="direction"> direction of movement if zero random direction</param>
        /// <param name="angle"> angle of exhaust </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, int frames, int fps, Rectangle source, float scale, float velocity, Vector3 direction, float angle, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _frames = frames;
            _pps = 1f / amount;
            _collides = false;
            _velocity = velocity;
            _direction = direction;
            Angle = angle;
            _gravity = false;
            _anim = true;
            _fps = fps;
            _source = source;
        }

        /// <summary>
        /// contructor for colortinted animated particles with movement at specified speed in specified direction with influence of gravity
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="frames"> number of frames if 0 not animated </param>
        /// <param name="fps"> frames per second if 0 not animated </param>
        /// <param name="source"> source rectangle for frame dimensions </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="velocity"> speed of particle </param>
        /// <param name="direction"> direction of movement if zero random direction</param>
        /// <param name="angle"> angle of exhaust </param>
        /// <param name="gravity"> if true uses gravity </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, int frames, int fps, Rectangle source, float scale, float velocity, Vector3 direction, float angle, bool gravity, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _frames = frames;
            _pps = 1f / amount;
            _collides = false;
            _velocity = velocity;
            _direction = direction;
            Angle = angle;
            _gravity = gravity;
            _anim = true;
            _fps = fps;
            _source = source;
        }

        /// <summary>
        /// contructor for colortinted animated particles with movement at specified speed in specified direction with collsion and gravity
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="particle"> Particle texture </param>
        /// <param name="color"> tint color </param>
        /// <param name="frames"> number of frames if 0 not animated </param>
        /// <param name="fps"> frames per second if 0 not animated </param>
        /// <param name="source"> source rectangle for frame dimensions </param>
        /// <param name="scale"> particle scale </param>
        /// <param name="velocity"> speed of particle </param>
        /// <param name="direction"> direction of movement if zero random direction</param>
        /// <param name="angle"> angle of exhaust </param>
        /// <param name="gravity"> if true uses gravity </param>
        /// <param name="collides"> if true collides </param>
        /// <param name="amount"> amount of particles per second </param>
        /// <param name="lifetime"> particle lifetime </param>
        public Emitter(GameObjectManager objMan, int id, Texture2D particle, Color color, int frames, int fps, Rectangle source, float scale, float velocity, Vector3 direction, float angle, bool gravity, bool collides, float amount, float lifetime)
            : base(objMan, id)
        {
            _lifetime = lifetime;
            _velocity = 0f;
            _scale = scale;
            _particle = particle;
            _source = particle.Bounds;
            _color = color;
            _frames = frames;
            _pps = 1f / amount;
            _collides = collides;
            _velocity = velocity;
            _direction = direction;
            Angle = angle;
            _gravity = true;
            _anim = true;
            _fps = fps;
            _source = source;
        }

        #endregion

        #region properties

        //sprite
        public Texture2D Particle
        {
            get { return _particle; }
            set
            {
                _source = value.Bounds;
                _particle = value;
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        //animation
        public bool isAnimated
        {
            get { return _anim; }
            set
            {
                if (_anim && !value)
                {
                    _anim = value;
                    _frames = 0;
                }
            }
        }

        public int Frames
        {
            get { return _frames; }
            set
            {
                if (value < 0)
                {
                    _frames = 0;
                    _anim = false;
                }
                else
                {
                    _frames = value;
                    _anim = true;
                }
            }
        }

        public Rectangle Source
        {
            get { return _source; }
            set
            {
                if (value.Width > _particle.Width
                || value.Height > _particle.Height
                || value.Width <= 0
                || value.Height <= 0)
                {
                    value = _particle.Bounds;
                }
                _source = value;
            }
        }

        public int FPS
        {
            get { return _fps; }
            set
            {
                if (value <= 0) value = 1;
                _fps = value;
            }
        }

        //lifetime
        public float Lifetime
        {
            get { return _lifetime; }
            set
            {
                if (value <= 0f) _lifetime = 0.1f;
                else _lifetime = value;
            }
        }

        //physics
        public float Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = Vector3.Normalize(value); }
        }

        public float Angle
        {
            get { return _angle.X; }
            set
            {
                if (value < 0f) value = 0f;
                else if (value >= 1f) value = 1f;
                _angle.X = value;
                _angle.Y = value;
                _angle.Z = value;
            }
        }

        public bool Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
        }

        public bool Collides
        {
            get { return _collides; }
            set { _collides = value; }
        }

        //emitter
        public int ParticlesPerSec
        {
            get { return (int)(_pps * 1000); }
            set
            {
                if (value < 0) _pps = 1f;
                else _pps = value * 0.001f;
            }
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            base.Update(Delta, game);
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            _elapsed += Delta;
            Random random = new Random();
            while (_elapsed >= _pps)
            {
                _elapsed -= _pps;
                GameObject obj = _objectManager.CreateObject();
                obj.AddComponent<Transform>(new Transform(_objectManager, obj.Id));
                obj.name = "particle";
                obj.tag = "particle";
                obj.transform.position = gameObject.transform.position;
                obj.transform.scale = new Vector3(_scale, _scale, 1f);
                obj.AddComponent<Sprite>(new Sprite(_objectManager, obj.Id, _particle, 1));
                if (_color == _random)
                    obj.sprite.Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
                else
                    obj.sprite.Color = _color;
                obj.sprite.source = _particle.Bounds;
                if (_anim)
                {
                    obj.AddComponent<Animation>(new Animation(_objectManager, obj.Id));
                    obj.animation.FPS = _fps;
                    Rectangle test = new Rectangle(0, 0, _source.Width, _source.Height);
                    obj.animation.AddAnimation(_particle, "standard", test);
                    obj.animation.SetAnimation("standard");
                }
                if (_collides || _gravity || _velocity != 0f)
                {
                    obj.AddComponent<Physic>(new Physic(_objectManager, obj.Id, _collides, false, 5, _gravity, Vector3.Zero));
                    if (_velocity != 0f)
                    {
                        Vector3 buffer = _direction;
                        if (buffer == Vector3.Zero)
                        {
                            float x = (float)random.NextDouble();
                            float y = (float)random.NextDouble();
                            if (random.NextDouble() > 0.5f) x *= -1f;
                            if (random.NextDouble() > 0.5f) y *= -1f;
                            buffer = new Vector3(x, y, 0f);
                        }
                        else
                        {
                            float factor = (float)random.NextDouble();
                            if (random.NextDouble() > 0.5f) factor *= -1f;
                            buffer = Vector3.Transform(buffer, Matrix.CreateRotationZ(_angle.X*factor));
                        }

                        obj.physic.velocity = Vector3.Normalize(buffer) * _velocity;
                    }
                }
                obj.AddComponent<Temporary>(new Temporary(_objectManager, obj.Id, _lifetime));
                obj.sprite.Initialize();
            }

            base.FixedUpdate(Delta, game);
        }

        #endregion

    }

    #endregion

    #region temporary

    public class Temporary : Component
    {
        #region Fields

        private float _lifetime;
        private float _elapsed;

        #endregion

        #region Constructor

        public Temporary(GameObjectManager objMan, int id, float Lifetime)
            : base(objMan, id)
        {
            _lifetime = Lifetime;
            _elapsed = 0f;
        }

        #endregion

        #region Properties

        public float Lifetime
        {
            get { return _lifetime; }
            set
            {
                if (value <= 0f) _lifetime = 0.1f;
                else _lifetime = value;
            }
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            base.Update(Delta, game);
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            _elapsed += Delta;
            if (_elapsed >= _lifetime)
            {
                gameObject.DeleteObject();
            }
        }

        #endregion
    }

    #endregion

    #region physics

    public class Physic : Component
    {

        #region Fields

        private Vector3 _force;
        private Vector3 _velocity;
        private bool _usesGrav;
        private static Vector3 _gravity = Vector3.Down * 9.81f;

        private bool _isRigid;
        private float _mass;
        private bool _activeCollide;
        private float _lastGroundContact;

        private Vector3 _constraint;

        #endregion

        #region Constructor

        /// <summary>
        /// Standard Physic Constructor
        /// defines physic component of a moving object
        /// </summary>
        /// <param name="objMan"> object managwer </param>
        /// <param name="id"> object id </param>
        public Physic(GameObjectManager objMan, int id)
            : base(objMan, id)
        {
            _force = Vector3.Zero;
            _velocity = Vector3.Zero;
            _isRigid = false;
            _mass = 0f;
            _constraint = Vector3.Zero;
            _usesGrav = false;
            _activeCollide = false;
        }

        /// <summary>
        /// Advanced Physic Constructor
        /// defines physic component of a moving rigidbody that can be influenced by gravity
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="rigidbody"> if true is rigidbody </param>
        /// <param name="activeCollide"> object has activeCollisioncheck </param>
        /// <param name="mass"> specifies object mass </param>
        /// <param name="useGravity"> if true uses gravity </param>
        /// <param name="velocity"> initial velocity </param>
        public Physic(GameObjectManager objMan, int id, bool rigidbody, bool activeCollide, float mass, bool useGravity, Vector3 velocity)
            : base(objMan, id)
        {
            _force = Vector3.Zero;
            _velocity = velocity;
            _isRigid = rigidbody;
            _mass = mass;
            _constraint = Vector3.Zero;
            _usesGrav = useGravity;
            _activeCollide = activeCollide;
        }

        /// <summary>
        /// Advanced Physic Constructor
        /// defines physic component of a moving rigidbody that can be influenced by gravity and that can be constrained along multiple movement axes
        /// </summary>
        /// <param name="objMan"> object manager </param>
        /// <param name="id"> object id </param>
        /// <param name="rigidbody"> if true is rigidbody </param>
        /// <param name="activeCollide"> object has activeCollisioncheck </param>
        /// <param name="mass"> specifies object mass </param>
        /// <param name="constraint"> constraint, if 0 not constrained for axis, if not 0 not constrained </param>
        /// <param name="useGravity"> if true influenced by gravity </param>
        /// <param name="velocity"> initial velocity </param>
        public Physic(GameObjectManager objMan, int id, bool rigidbody, bool activeCollide,float mass, Vector3 constraint, bool useGravity, Vector3 velocity)
            : base(objMan, id)
        {
            _force = Vector3.Zero;
            _velocity = velocity;
            _isRigid = rigidbody;
            _mass = mass;
            _constraint = constraint;
            _usesGrav = useGravity;
            _activeCollide = activeCollide;
        }

        #endregion

        #region Properties

        public bool IsRigid
        {
            get { return _isRigid; }
            set { _isRigid = value; }
        }

        public bool ActiveCol
        {
            get { return _activeCollide; }
        }

        public Vector3 velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public bool HasGroundContact
        {
            get { return _lastGroundContact < 0.75f; }
            set 
            { 
                if(value)
                    _lastGroundContact = 0f; 
                else
                    _lastGroundContact = 1f; 
            }
        }

        public Vector3 constraint
        {
            get { return _constraint; }
            set
            {
                if (value.X != 0.0f) _constraint.X = 1.0f;
                else _constraint.X = value.X;
                if (value.Y != 0.0f) _constraint.Y = 1.0f;
                else _constraint.Y = value.Y;
                if (value.Z != 0.0f) _constraint.Z = 1.0f;
                else _constraint.Z = value.Z;
            }
        }

        public bool UsesGravity
        {
            get { return _usesGrav; }
            set { _usesGrav = value; }
        }

        public float Mass
        {
            get { return _mass; }
            set
            {
                if (value < 0f) value = 0f;
                _mass = value;
            }
        }

        #endregion

        #region Methods

        public void setConstraint(bool x, bool y, bool z)
        {
            if (x) _constraint.X = 1.0f;
            else _constraint.X = 0.0f;
            if (y) _constraint.Y = 1.0f;
            else _constraint.Y = 0.0f;
            if (z) _constraint.Z = 1.0f;
            else _constraint.Z = 0.0f;
        }

        public void AddForce(Vector3 force)
        {
            _force += force;
        }

        public void AddForce(float x, float y, float z)
        {
            _force += (new Vector3(x, y, z));
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void SetVelocity(float x, float y, float z)
        {
            _velocity = new Vector3(x, y, z);
        }

        public override void Update(float Delta, Game game)
        {
            _lastGroundContact += Delta;
            if (_usesGrav)
                _force += _gravity;
            _velocity += _force * Delta;
            _force = Vector3.Zero;

            if (_velocity.LengthSquared() > 64f * 64f)
            {
                _velocity.Normalize();
                _velocity *= 64f;
            }
            base.Update(Delta, game);
        }

        public override void OnCollision(GameObject other)
        {
            if (_constraint.X != 1 && _constraint.Y != 1)
            {
                Vector3 force = Vector3.Zero;

                if (_mass < other.physic.Mass)
                {
                    force = other.physic.velocity;
                    if (_constraint.X != 0)
                        force.X = 0;
                    if (_constraint.Y != 0)
                        force.Y = 0;
                }

                _velocity += force;
            }

            base.OnCollision(other);
        }

        #endregion

    }

    #endregion

    #region CharacterMover

    public class CharacterMover : Component
    {
        #region Fields

        private int _state;
        private float _speed;
        private float _runSpeed;
        private float _jumpForce;

        #endregion

        #region Constructor

        public CharacterMover(GameObjectManager objMan, int id, float speed, float runSpeed, float jumpForce)
            : base(objMan, id)
        {
            _state = 0;
            _speed = speed;
            _runSpeed = runSpeed;
            _jumpForce = jumpForce;
        }

        #endregion

        #region Properties

        public void Jump(float Delta)
        {
            switch (_state)
            {
                //standing
                case 0:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = false;
                        gameObject.physic.HasGroundContact = false;
                        gameObject.physic.AddForce(Vector3.Up * _jumpForce);
                        gameObject.animation.SetAnimation("idle");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically;
                        _state = 3;
                    }
                    break;
                //walking right
                case 1:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = false;
                        gameObject.physic.HasGroundContact = false;
                        gameObject.physic.AddForce(Vector3.Up * _jumpForce);
                        gameObject.animation.SetAnimation("idle");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically;
                        _state = 3;
                    }
                    break;
                //walking left
                case 2:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = false;
                        gameObject.physic.HasGroundContact = false;
                        gameObject.physic.AddForce(Vector3.Up * _jumpForce);
                        gameObject.animation.SetAnimation("idle");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                        _state = 3;
                    }
                    break;
                default:
                    break;
            }
        }

        public void MoveRight(float Delta, bool run)
        {
            switch (_state)
            {
                //standing
                case 0:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if(run)
                            gameObject.physic.AddForce(Vector3.Right * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Right * _speed);
                        gameObject.animation.SetAnimation("walk");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically;
                        _state = 1;
                    }
                    break;
                //walking right
                case 1:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if (run)
                            gameObject.physic.AddForce(Vector3.Right * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Right * _speed);
                    }
                    break;
                //walking left
                case 2:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if (run)
                            gameObject.physic.AddForce(Vector3.Right * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Right * _speed);
                        gameObject.animation.SetAnimation("walk");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically;
                        _state = 1;
                    }
                    break;
                //falling
                case 3:
                    gameObject.physic.AddForce(Vector3.Right * _speed * 0.2f);
                    gameObject.animation.SetAnimation("idle");
                    _state = 0;
                    break;
                default:
                    break;
            }
        }

        public void MoveLeft(float Delta, bool run)
        {
            switch (_state)
            {
                //standing
                case 0:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if (run)
                            gameObject.physic.AddForce(Vector3.Left * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Left * _speed);
                        gameObject.animation.SetAnimation("walk");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                        _state = 2;
                    }
                    break;
                //walking right
                case 1:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if (run)
                            gameObject.physic.AddForce(Vector3.Left * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Left * _speed);
                        gameObject.animation.SetAnimation("walk");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                        _state = 2;
                    }
                    break;
                //walking left
                case 2:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.audio.Play = true;
                        if (run)
                            gameObject.physic.AddForce(Vector3.Left * _runSpeed);
                        else
                            gameObject.physic.AddForce(Vector3.Left * _speed);
                    }
                    break;
                //falling
                case 3:
                    gameObject.physic.AddForce(Vector3.Left * _speed * 0.2f);
                    gameObject.animation.SetAnimation("idle");
                    gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    _state = 0;
                    break;
                default:
                    break;
            }
        }

        public void Stop()
        {
            gameObject.audio.Play = false;
            switch (_state)
            {
                //walking right
                case 1:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.animation.SetAnimation("idle");
                        _state = 0;
                    }
                    break;
                //walking left
                case 2:
                    if (gameObject.physic.HasGroundContact)
                    {
                        gameObject.animation.SetAnimation("idle");
                        gameObject.sprite.SpriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                        _state = 0;
                    }
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods

        public override void FixedUpdate(float Delta, Game game)
        {
            if ((_state == 3 || _state == 4) && gameObject.physic.HasGroundContact)
                        _state = 1;
        }

        #endregion

    }

    #endregion

    #region characterController

    public class CharacterController : Component
    {

        #region Fields

        #endregion

        #region Constructor

        public CharacterController(GameObjectManager objMan, int id)
            : base(objMan, id)
        {

        }

        #endregion

        #region Properties


        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            if (InputHandler.KeyPressed(Keys.Left) || InputHandler.GStickDirection("left"))
                gameObject.characterMover.MoveLeft(Delta, InputHandler.KeyPressed(Keys.LeftShift) || InputHandler.GButtonPressed("RB"));
            if (InputHandler.KeyPressed(Keys.Right) || InputHandler.GStickDirection("right"))
                gameObject.characterMover.MoveRight(Delta, InputHandler.KeyPressed(Keys.LeftShift) || InputHandler.GButtonPressed("RB"));
            if(InputHandler.KeyDown(Keys.Space) || InputHandler.GButtonDown("A"))
                gameObject.characterMover.Jump(Delta);
            if (!InputHandler.KeyPressed(Keys.Left) && !InputHandler.KeyPressed(Keys.Right) && !InputHandler.GStickDirection("left") && !InputHandler.GStickDirection("right"))
                gameObject.characterMover.Stop();
        }

        #endregion

    }

    #endregion

    #region audio

    public class Audio : Component
    {

        #region Fields

        private bool _play;
        public bool _loop;
        public float _interval;
        public float _elapsed;
        private string _name;
        private AudioEmitter _emitter = new AudioEmitter();
        private AudioListener _listener = new AudioListener();
        private List<Cue> _played = new List<Cue>();

        #endregion

        #region Constructor

        public Audio(GameObjectManager objMan, int id, string name, bool loop, float loopInterval)
            : base(objMan, id)
        {
            _name = name;
            _loop = loop;
            _interval = loopInterval;
            _elapsed = 0f;
        }

        #endregion

        #region Properties

        public bool isPlaying
        {
            get 
            {
                foreach (Cue c in _played)
                {
                    if (c.IsPlaying)
                        return true;
                }
                return false;
            }
        }

        public bool Play
        {
            set 
            {
                _play = value;
                if (!value)
                    stopAll();
            }
        }

        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        public float Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        #endregion

        #region Methods

        private void getListener()
        {
            Camera camera = _objectManager.GetComponents<Camera>().FirstOrDefault() as Camera;
            if (camera != null)
                _listener.Position = camera.gameObject.transform.position;
        }

        private void getEmitter()
        {
            _emitter.Position = gameObject.transform.position;
        }

        private void stopAll()
        {
            foreach (Cue c in _played)
            {
                if (c.IsPlaying)
                {
                    c.Stop(AudioStopOptions.Immediate);
                }
                c.Dispose();
            }
            _played.Clear();
        }

        private void Clean()
        {
            for(int i = 0; i < _played.Count; i++)
            {
                if (!_played[i].IsPlaying)
                {
                    _played[i].Dispose();
                    _played.RemoveAt(i);
                    i--;
                }
            }
        }

        private void apply3D()
        {
            getListener();
            getEmitter();
            foreach (Cue c in _played)
            {
                if (c.IsPlaying)
                {
                    c.Apply3D(_listener, _emitter);
                }
            }
        }

        private void Instantiate()
        {
            Cue newCue = AudioHandler.GetCue(_name);
            newCue.Apply3D(_listener, _emitter);
            newCue.Play();
            _played.Add(newCue);
        }

        public override void Update(float Delta, Game game)
        {
            Clean();
            apply3D();
            if (_play)
            {
                if (_loop)
                {
                    _elapsed += Delta;
                    if (_elapsed >= _interval)
                    {
                        Instantiate();
                        _elapsed = 0f;
                    }
                }
                else
                {
                    Instantiate();
                    _play = false;
                }
            }
            base.Update(Delta, game);
        }

        #endregion

    }
    #endregion

    #region trigger

    public class Trigger : Component
    {
        #region Fields

        private Rectangle _range;
        private Dictionary<int, GameObject> _new = new Dictionary<int,GameObject>();
        protected Dictionary<int, GameObject> _enter = new Dictionary<int, GameObject>();
        protected Dictionary<int, GameObject> _stay = new Dictionary<int, GameObject>();
        protected Dictionary<int, GameObject> _leave = new Dictionary<int, GameObject>();

        #endregion

        #region Constructor

        public Trigger(GameObjectManager objMan, int id, Rectangle range)
            : base(objMan, id)
        {
            _range = range;
        }

        #endregion

        #region Properties

        public Rectangle Range
        {
            get 
            {
                return new Rectangle((int)gameObject.transform.position.X + _range.X, (int)gameObject.transform.position.Y + _range.Y, _range.Width, _range.Height); 
            }
            set
            {
                if (value.Width < 0)
                    value.Width = 1;
                if (value.Height < 0)
                    value.Height = 1;
                _range = value;
            }
        }

        public Dictionary<int, GameObject> InRange
        {
            get  {return _stay; }
        }

        #endregion

        #region Methods

        public override void FixedUpdate(float Delta, Game game)
        {
            _leave.Clear();

            foreach (var pair in _stay)
            {
                if (!_new.ContainsKey(pair.Key))
                {
                    _leave.Add(pair.Key, pair.Value);
                }
            }

            foreach (var pair in _leave)
            {
                _stay.Remove(pair.Key);
            }

            foreach (var pair in _new)
            {
                if (_enter.ContainsKey(pair.Key))
                {
                    _stay.Add(pair.Key, pair.Value);
                    _enter.Remove(pair.Key);
                }
                else if(!_stay.ContainsKey(pair.Key))
                {
                    _enter.Add(pair.Key, pair.Value);
                }
            }
            _new.Clear();

            foreach (var pair in _enter)
            {
                List<Component> comp = pair.Value.GetComponents();
                foreach (Component c in comp)
                    c.OnTriggerEnter(gameObject);
            }
            foreach (var pair in _stay)
            {
                List<Component> comp = pair.Value.GetComponents();
                foreach (Component c in comp)
                    c.OnTriggerStay(gameObject);
            }
            foreach (var pair in _leave)
            {
                List<Component> comp = pair.Value.GetComponents();
                foreach (Component c in comp)
                    c.OnTriggerLeave(gameObject);
            }

            base.FixedUpdate(Delta, game);
        }

        public void UpdateLists(List<GameObject> objects)
        {
            Rectangle range = Range;
            foreach (GameObject obj in objects)
            {
                if(inRectangle(obj.transform.position, range))
                _new.Add(obj.Id, obj);
            }
        }

        private bool inRectangle(Vector3 pos, Rectangle rect)
        {
            return !(pos.X < rect.Left ||
                pos.X > rect.Right ||
                pos.Y < rect.Top ||
                pos.Y > rect.Bottom);
        }

        public override void OnDelete()
        {
            foreach (var pair in _stay)
            {
                List<Component> comps = pair.Value.GetComponents();
                if (comps != null)
                {
                    foreach (Component comp in comps)
                        comp.OnTriggerLeave(gameObject);
                }
            }
            foreach (var pair in _leave)
            {
                List<Component> comps = pair.Value.GetComponents();
                if (comps != null)
                {
                    foreach (Component comp in comps)
                        comp.OnTriggerLeave(gameObject);
                }
            }
            base.OnDelete();
        }

        #endregion
    }

    #endregion

    #region UIGroup

    public class UIGroup : Component
    {

        #region Fields

        protected Dictionary<int, GameObject> _elements = new Dictionary<int,GameObject>();
        protected UIPosition _pos;

        #endregion

        #region Constructor

        public UIGroup(GameObjectManager objMan, int id, Rectangle range, alignment align, params GameObject[] elements)
            : base(objMan, id)
        {
            _pos = new UIPosition(range);
            _pos.setAlignment(align);
            if(elements != null)
                foreach(GameObject obj in elements)
                {
                    _elements.Add(obj.name.GetHashCode(), obj);
                }
        }

        #endregion

        #region Properties

        public UIPosition UIPosition
        {
            get { return _pos; }
        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var pair in _elements)
            {
                Transform transform = pair.Value.transform;
                if (pair.Value.solid != null)
                {
                    pair.Value.solid.Draw(spriteBatch, 
                        new Vector3(gameObject.transform.position.X - _pos.Alignment.X , 
                            gameObject.transform.position.Y - _pos.Alignment.Y, 
                            gameObject.transform.position.Z),
                            new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height),
                            gameObject.transform.rotation.Z);
                }
                if (pair.Value.uitext != null)
                {
                    pair.Value.uitext.Draw(spriteBatch,
                        new Vector3(gameObject.transform.position.X - _pos.Alignment.X, 
                            gameObject.transform.position.Y - _pos.Alignment.Y, 
                            gameObject.transform.position.Z),
                            new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height),
                            gameObject.transform.rotation.Z);
                }
                if (pair.Value.uitexture != null)
                {
                    pair.Value.uitexture.Draw(spriteBatch,
                        new Vector3(gameObject.transform.position.X - _pos.Alignment.X,
                            gameObject.transform.position.Y - _pos.Alignment.Y,
                            gameObject.transform.position.Z),
                            new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height),
                            gameObject.transform.rotation.Z);
                }
            }
        }

        protected GameObject findElementByName(string name)
        {
            foreach (var pair in _elements)
            {
                if (pair.Value.name.GetHashCode() == name.GetHashCode())
                    return pair.Value;
            }
            return null;
        }

        public override void OnDelete()
        {
            foreach (var pair in _elements)
                pair.Value.DeleteObject();
            base.OnDelete();
        }
        #endregion

    }
 
    #endregion

    #region UIButton

    public enum UIButtonState
    {
        inactive = 0,
        active,
        pressed
    }

    public class UIButton : Component
    {
        
        #region Fields

        private Color[] _buttonStates =  new Color[3];
        private UIButtonState _buttonState = UIButtonState.inactive;
        private UIButtonState _prevButtonState = UIButtonState.inactive;

        #endregion

        #region Constructor

        public UIButton(GameObjectManager objMan, int id, Color inactive, Color hover, Color pressed)
            : base(objMan, id)
        {
            _buttonStates[0] = inactive;
            _buttonStates[1] = hover;
            _buttonStates[2] = pressed;
            if (gameObject.uitext != null)
                gameObject.uitext.Color = _buttonStates[(int)_buttonState];
            else if (gameObject.solid != null)
                gameObject.solid.Color = _buttonStates[(int)_buttonState];
            else if (gameObject.uitexture != null)
                gameObject.uitexture.Color = _buttonStates[(int)_buttonState];
        }

        #endregion

        #region Properties

        public UIButtonState State
        {
            get { return _buttonState; }
            set
            {
                _buttonState = value; 
            }
        }

        public UIButtonState PrevState
        {
            get { return _prevButtonState; }
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            if (_buttonState != _prevButtonState)
            {
                if (gameObject.uitext != null)
                    gameObject.uitext.Color = _buttonStates[(int)_buttonState];
                else if (gameObject.solid != null)
                    gameObject.solid.Color = _buttonStates[(int)_buttonState];
                else if (gameObject.uitexture != null)
                    gameObject.uitexture.Color = _buttonStates[(int)_buttonState];
            }
            _prevButtonState = _buttonState;
            base.Update(Delta, game);
        }

        public bool PointIntersect(Vector3 pos, Vector2 dimensions, float rotation)
        {
            Vector3 mousePos = InputHandler.MousePosition;
            if (gameObject.uitext != null)
            {
                if (gameObject.uitext.PointIntersect(mousePos, pos, dimensions, rotation))
                    return true;
            }
            else if (gameObject.solid != null)
            {
                if (gameObject.solid.PointIntersect(mousePos, pos, dimensions, rotation))
                    return true;
            }
            else if (gameObject.uitexture != null)
            {
                if (gameObject.uitexture.PointIntersect(mousePos, pos, dimensions, rotation))
                    return true;
            }
            return false;
        }

        #endregion

    }

    #endregion

    #region UIText

    public class UIText : Component
    {
        #region Fields

        private SpriteFont _font;
        private UIPosition _pos;
        private float _scale;
        private string _text;
        private Color _color;
        private Color _target;
        private float _elapsed;
        private float _duration;
        private Point _spacing;

        #endregion

        #region Constructor

        public UIText(GameObjectManager objMan, int id, string text, SpriteFont font, Color color)
            : base(objMan, id)
        {
            _font = font;
            _pos = new UIPosition(new Rectangle(0, 0, 0, 0));
            _spacing = new Point(0, 0);
            Text = text;
            _pos.setAlignment(alignment.UpperLeft);
            _color = color;
            _scale = 1f;
        }

        public UIText(GameObjectManager objMan, int id, string text, SpriteFont font, Color color, int width, int height, alignment align)
            : base(objMan, id)
        {
            _font = font;
            _pos = new UIPosition(new Rectangle(0, 0, 0, 0));
            _spacing = new Point(width, height);
            Text = text;
            _pos.setAlignment(align);
            _color = color;
        }

        #endregion

        #region Properties

        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                //int length = _text.Count<char>();
                Vector2 buffer = _font.MeasureString(_text);
                _pos.Dimensions = new Rectangle(0, 0, (int)buffer.X, (int)buffer.Y);
                _scale = 1f;
                if (_spacing.X != 0)
                    SetSpacing(_spacing.X, _spacing.Y);
                _pos.setAlignment(alignment.CenterLeft);
            }
        }

        public UIPosition UIPosition
        {
            get { return _pos; }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == Color.Transparent)
                    value = Color.White;
                _color = value;
            }
        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            spriteBatch.DrawString(_font,
                _text,
                new Vector2((position.X + ((gameObject.transform.position.X)* (dimension.X * 0.01f))),
                    position.Y + ((gameObject.transform.position.Y)* (dimension.Y * 0.01f))),
                _color,
                gameObject.transform.rotation.Z + rotation,
                _pos.Alignment,
                new Vector2((gameObject.transform.scale.X / 28f) *_scale * ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    (gameObject.transform.scale.Y / 28f) * _scale * ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                SpriteEffects.None,
                 position.Z + (gameObject.transform.position.Z * 0.01f));
        }

        public void BlendTo(Color color, float time)
        {
            _target = color;
            _duration = time;
            _elapsed = 0f;
        }

        public void BlendTo(float transparency, float time)
        {
            _target = _color;
            _target.A = (Byte)((255 * 0.01f) * transparency);
            _duration = time;
            _elapsed = 0f;
        }

        public Texture2D GetWorldTexture(GraphicsDeviceManager devMan, SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Rectangle bound = GetWorldBound(position, dimension, rotation);

            // set new render target
            RenderTarget2D collisionRenderTarget = new RenderTarget2D(devMan.GraphicsDevice, bound.Width, bound.Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(collisionRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            //render
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullCounterClockwise);

            spriteBatch.DrawString(_font,
                _text,
                new Vector2(bound.Width * 0.5f, bound.Height * 0.5f),
                _color,
                transform.rotation.Z + rotation,
                new Vector2(_pos.Dimensions.Width * 0.5f, _pos.Dimensions.Height * 0.5f),
                new Vector2(gameObject.transform.scale.X * ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    gameObject.transform.scale.Y * ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                SpriteEffects.None,
                0f);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            // return Render Target as Texture
            return collisionRenderTarget;
        }

        public Rectangle GetWorldBound(Vector3 position, Vector2 dimension, float rotation)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Vector2[] bounds = new Vector2[4];

            bounds[0] = new Vector2(0f, _pos.Dimensions.Height);
            bounds[1] = new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height);
            bounds[2] = new Vector2(0f, 0f);
            bounds[3] = new Vector2(_pos.Dimensions.Width, 0f);

            Matrix rotated = Matrix.CreateRotationZ(transform.rotation.Z + rotation);
            Matrix scaled = Matrix.CreateScale(transform.scale.X/28 * _scale * ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                transform.scale.Y/28 * _scale * ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height),
                1f);

            Matrix translateTo = Matrix.CreateTranslation(-_pos.Dimensions.Width * 0.5f, -_pos.Dimensions.Height * 0.5f, 0f);
            Matrix translateBack = Matrix.CreateTranslation(position.X + (transform.position.X * (dimension.X * 0.01f)),
                position.Y + (transform.position.Y * (dimension.Y * 0.01f)),
                0f);
            Matrix transMatrix = translateTo * rotated * scaled * translateBack;

            bounds[0] = Vector2.Transform(bounds[0], transMatrix);
            bounds[1] = Vector2.Transform(bounds[1], transMatrix);
            bounds[2] = Vector2.Transform(bounds[2], transMatrix);
            bounds[3] = Vector2.Transform(bounds[3], transMatrix);

            Vector2 min = new Vector2(MathHelper.Min(MathHelper.Min(bounds[0].X, bounds[1].X), MathHelper.Min(bounds[2].X, bounds[3].X)),
                MathHelper.Min(MathHelper.Min(bounds[0].Y, bounds[1].Y), MathHelper.Min(bounds[2].Y, bounds[3].Y)));
            Vector2 max = new Vector2(MathHelper.Max(MathHelper.Max(bounds[0].X, bounds[1].X), MathHelper.Max(bounds[2].X, bounds[3].X)),
                MathHelper.Max(MathHelper.Max(bounds[0].Y, bounds[1].Y), MathHelper.Max(bounds[2].Y, bounds[3].Y)));

            Rectangle bound = new Rectangle((int)min.X, (int)min.Y, (int)max.X - (int)min.X, (int)max.Y - (int)min.Y);

            return bound;
        }

        public bool PointIntersect(Vector3 mouse, Vector3 position, Vector2 dimension, float rotation)
        {
            
            Point p = new Point((int)(mouse.X - position.X - (gameObject.transform.position.X * (dimension.X * 0.01f))),
                (int)(mouse.Y - position.Y - (gameObject.transform.position.Y * (dimension.Y * 0.01f))));
            if (GetWorldBound(position, dimension, rotation).Contains(new Point((int)mouse.X, (int)mouse.Y)))
            {
                return Sprite.GetPixel(GetWorldTexture(flatRender.DeviceManager,
                    flatRender.SpriteBatch,
                    position,
                    dimension,
                    rotation),
                    p).A > 0;
            }
            return false;
        }

        public void SetSpacing(int width, int height)
        {
            Vector2 dim = _font.MeasureString(_text);
            if (dim.X / width > dim.Y / height)
                _scale = width / dim.X;
            else
                _scale = height / dim.Y;
        }

        public override void Update(float Delta, Game game)
        {
            if (_elapsed < _duration)
            {
                _elapsed += Delta;
                _color = Color.Lerp(_color, _target, Delta / _duration);
            }
            base.Update(Delta, game);
        }

        public void RemoveLast()
        {
            if(_text.Length > 0)
                Text = _text.Remove(_text.Length - 1);
        }

        public void AddAtEnd(string toadd)
        {
            if(toadd != " ")
                Text += toadd;
        }

        #endregion

    }

    #endregion

    #region Solid

    public class Solid : Component
    {

        #region Fields

        private Color _color;
        private UIPosition _pos;
        private Texture2D _solid;
        private Color _target;
        private float _elapsed;
        private float _duration;

        #endregion

        #region Constructor

        public Solid(GameObjectManager objMan, int id, Rectangle res, Color color, alignment align)
            : base(objMan, id)
        {
            _color = color;
            _pos = new UIPosition(res);
            _pos.setAlignment(align);
            _elapsed = 0f;
            _duration = 0f;
            _target = Color.Black;
            createSolid(flatRender.DeviceManager.GraphicsDevice);
        }

        #endregion

        #region Properties

        public float Transparence
        {
            get { return _color.A/(255/100); }
            set
            {
                if (value < 0f) _color.A = 0;
                else if (value > 1f) _color.A = 255;
                else
                    _color.A = (Byte)(value*255);
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _solid = null;
            }
        }

        public UIPosition UIPosition
        {
            get { return _pos; }
        }

        #endregion

        #region Methods

        void createSolid(GraphicsDevice device)
        {
            _solid = new Texture2D(device, _pos.Dimensions.Width, _pos.Dimensions.Height);
            Color[] colors = new Color[_solid.Width * _solid.Height];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.White;
            _solid.SetData<Color>(colors);
        }

        public void Draw(SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            if (_solid == null)
                createSolid(spriteBatch.GraphicsDevice);

            Transform transform = gameObject.transform;
            spriteBatch.Draw(_solid,
                new Vector2((position.X + (gameObject.transform.position.X * (dimension.X * 0.01f))),
                    position.Y + (gameObject.transform.position.Y * (dimension.Y * 0.01f))),
                _pos.Dimensions,
                _color,
                gameObject.transform.rotation.Z + rotation,
                _pos.Alignment,
                new Vector2(gameObject.transform.scale.X * ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    gameObject.transform.scale.Y * ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                SpriteEffects.None,
                position.Z + (gameObject.transform.position.Z * 0.01f));
        }

        public void BlendTo(Color color, float time)
        {
            _target = color;
            _duration = time;
            _elapsed = 0f;
        }

        public void BlendTo(float transparency, float time)
        {
            _target = _color;
            _target.A = (Byte)((255 * 0.01f) * transparency);
            _duration = time;
            _elapsed = 0f;
        }

        public Texture2D GetWorldTexture(GraphicsDeviceManager devMan, SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            if (_solid == null)
                createSolid(spriteBatch.GraphicsDevice);
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Rectangle bound = GetWorldBound(position, dimension, rotation);

            // set new render target
            RenderTarget2D collisionRenderTarget = new RenderTarget2D(devMan.GraphicsDevice, bound.Width, bound.Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(collisionRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            //render
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullCounterClockwise);

            spriteBatch.Draw(_solid,
                new Vector2(bound.Width * 0.5f, bound.Height * 0.5f),
                _pos.Dimensions,
                _color,
                transform.rotation.Z + rotation,
                new Vector2(_pos.Dimensions.Width * 0.5f, _pos.Dimensions.Height * 0.5f),
                new Vector2(gameObject.transform.scale.X *2* ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    gameObject.transform.scale.Y *2* ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                SpriteEffects.None,
                0f);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            // return Render Target as Texture
            return collisionRenderTarget;
        }

        public Rectangle GetWorldBound(Vector3 position, Vector2 dimension, float rotation)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Vector2[] bounds = new Vector2[4];

            bounds[0] = new Vector2(0f, _pos.Dimensions.Height);
            bounds[1] = new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height);
            bounds[2] = new Vector2(0f, 0f);
            bounds[3] = new Vector2(_pos.Dimensions.Width, 0f);

            Matrix rotated = Matrix.CreateRotationZ(transform.rotation.Z + rotation);
            Matrix scaled = Matrix.CreateScale(transform.scale.X * 2* ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                transform.scale.Y * ((_pos.Dimensions.Height * 2*(dimension.Y * 0.01f)) / _pos.Dimensions.Height),
                1f);
            Matrix translateTo = Matrix.CreateTranslation(-_pos.Dimensions.Width * 0.5f, -_pos.Dimensions.Height * 0.5f, 0f);
            Matrix translateBack = Matrix.CreateTranslation(position.X + ((transform.position.X - _pos.Alignment.X) * (dimension.X * 0.01f)),
                position.Y + ((transform.position.Y - _pos.Alignment.Y)* (dimension.Y * 0.01f)),
                0f);
            Matrix transMatrix = translateTo * rotated * scaled * translateBack;

            bounds[0] = Vector2.Transform(bounds[0], transMatrix);
            bounds[1] = Vector2.Transform(bounds[1], transMatrix);
            bounds[2] = Vector2.Transform(bounds[2], transMatrix);
            bounds[3] = Vector2.Transform(bounds[3], transMatrix);

            Vector2 min = new Vector2(MathHelper.Min(MathHelper.Min(bounds[0].X, bounds[1].X), MathHelper.Min(bounds[2].X, bounds[3].X)),
                MathHelper.Min(MathHelper.Min(bounds[0].Y, bounds[1].Y), MathHelper.Min(bounds[2].Y, bounds[3].Y)));
            Vector2 max = new Vector2(MathHelper.Max(MathHelper.Max(bounds[0].X, bounds[1].X), MathHelper.Max(bounds[2].X, bounds[3].X)),
                MathHelper.Max(MathHelper.Max(bounds[0].Y, bounds[1].Y), MathHelper.Max(bounds[2].Y, bounds[3].Y)));

            Rectangle bound = new Rectangle((int)min.X, (int)min.Y, (int)max.X - (int)min.X, (int)max.Y - (int)min.Y);

            return bound;
        }

        public bool PointIntersect(Vector3 mouse, Vector3 position, Vector2 dimension, float rotation)
        {
            Point p = new Point((int)(mouse.X - position.X - ((gameObject.transform.position.X - _pos.Alignment.X) * (dimension.X * 0.01f))),
                (int)(mouse.Y - position.Y - ((gameObject.transform.position.Y - _pos.Alignment.Y) * (dimension.Y * 0.01f))));
            if (GetWorldBound(position, dimension, rotation).Contains(new Point((int)mouse.X, (int)mouse.Y)))
            {
                return Sprite.GetPixel(GetWorldTexture(flatRender.DeviceManager,
                    flatRender.SpriteBatch,
                    position,
                    dimension,
                    rotation),
                    p).A > 0;
            }
            return false;
        }

        public override void Update(float Delta, Game game)
        {
            if (_elapsed < _duration)
            {
                _elapsed += Delta;
                _color = Color.Lerp(_color, _target, Delta/_duration);
            }
            base.Update(Delta, game);
        }

        #endregion

    }

    #endregion

    #region UITexture

    public class UITexture : Component
    {

        #region Fields

        private Texture2D _texture2D;
        private Color _color;
        private float _UnitToPixelRatio;
        private UIPosition _pos;
        private SpriteEffects _spriteEffects = SpriteEffects.None;

        #endregion

        #region Constructor

        public UITexture(GameObjectManager objMan, int id, Texture2D texture, int texelsPerUnit, alignment align)
            : base(objMan, id)
        {
            _color = Color.White;
            _texture2D = texture;
            _pos = new UIPosition(_texture2D.Bounds);
            _pos.setAlignment(align);
            SetUPRatio(texelsPerUnit);
        }

        #endregion

        #region Properties

        // accessors
        public Texture2D texture2D
        {
            get { return _texture2D; }
            set { _texture2D = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public SpriteEffects SpriteEffects
        {
            set { _spriteEffects = value; }
        }

        public UIPosition UIPosition
        {
            get { return _pos; }
        }

        #endregion

        #region Methods

        #region component specific

        public void Draw(SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            spriteBatch.Draw(_texture2D,
                new Vector2((position.X + (gameObject.transform.position.X * (dimension.X * 0.01f))),
                    position.Y + (gameObject.transform.position.Y * (dimension.Y * 0.01f))),
                _pos.Dimensions,
                _color,
                gameObject.transform.rotation.Z + rotation,
                _pos.Alignment,
                new Vector2(gameObject.transform.scale.X * _UnitToPixelRatio * ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    gameObject.transform.scale.Y * _UnitToPixelRatio * ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                SpriteEffects.None,
                position.Z + (gameObject.transform.position.Z * 0.01f));
        }

        public Texture2D GetWorldTexture(GraphicsDeviceManager devMan, SpriteBatch spriteBatch, Vector3 position, Vector2 dimension, float rotation)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Rectangle bound = GetWorldBound(position, dimension, rotation);

            // set new render target
            RenderTarget2D collisionRenderTarget = new RenderTarget2D(devMan.GraphicsDevice, bound.Width, bound.Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(collisionRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            //render
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullCounterClockwise);

            spriteBatch.Draw(_texture2D,
                new Vector2(bound.Width * 0.5f, bound.Height * 0.5f),
                _pos.Dimensions,
                _color,
                transform.rotation.Z + rotation,
                new Vector2(_pos.Dimensions.Width * 0.5f, _pos.Dimensions.Height * 0.5f),
                new Vector2(gameObject.transform.scale.X * _UnitToPixelRatio *2* ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                    gameObject.transform.scale.Y * _UnitToPixelRatio * 2* ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height)),
                _spriteEffects,
                0f);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            // return Render Target as Texture
            return collisionRenderTarget;
        }

        public Rectangle GetWorldBound(Vector3 position, Vector2 dimension, float rotation)
        {
            // get object transformation
            Transform transform = gameObject.transform;
            //create Bound checker
            Vector2[] bounds = new Vector2[4];

            bounds[0] = new Vector2(0f, _pos.Dimensions.Height);
            bounds[1] = new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height);
            bounds[2] = new Vector2(0f, 0f);
            bounds[3] = new Vector2(_pos.Dimensions.Width, 0f);

            Matrix rotated = Matrix.CreateRotationZ(transform.rotation.Z + rotation);
            Matrix scaled = Matrix.CreateScale(transform.scale.X * _UnitToPixelRatio *2* ((_pos.Dimensions.Width * (dimension.X * 0.01f)) / _pos.Dimensions.Width),
                transform.scale.Y * _UnitToPixelRatio *2* ((_pos.Dimensions.Height * (dimension.Y * 0.01f)) / _pos.Dimensions.Height), 
                1f);
            Matrix translateTo = Matrix.CreateTranslation(-_pos.Dimensions.Width * 0.5f, -_pos.Dimensions.Height * 0.5f, 0f);
            Matrix translateBack = Matrix.CreateTranslation(position.X + ((transform.position.X - (_UnitToPixelRatio * _pos.Alignment.X)) * (dimension.X * 0.01f)),
                position.Y + ((transform.position.Y - (_UnitToPixelRatio * _pos.Alignment.Y)) * (dimension.Y * 0.01f)),
                0f);
            Matrix transMatrix = translateTo * rotated * scaled * translateBack;

            bounds[0] = Vector2.Transform(bounds[0], transMatrix);
            bounds[1] = Vector2.Transform(bounds[1], transMatrix);
            bounds[2] = Vector2.Transform(bounds[2], transMatrix);
            bounds[3] = Vector2.Transform(bounds[3], transMatrix);

            Vector2 min = new Vector2(MathHelper.Min(MathHelper.Min(bounds[0].X, bounds[1].X), MathHelper.Min(bounds[2].X, bounds[3].X)),
                MathHelper.Min(MathHelper.Min(bounds[0].Y, bounds[1].Y), MathHelper.Min(bounds[2].Y, bounds[3].Y)));
            Vector2 max = new Vector2(MathHelper.Max(MathHelper.Max(bounds[0].X, bounds[1].X), MathHelper.Max(bounds[2].X, bounds[3].X)),
                MathHelper.Max(MathHelper.Max(bounds[0].Y, bounds[1].Y), MathHelper.Max(bounds[2].Y, bounds[3].Y)));

            Rectangle bound = new Rectangle((int)min.X, (int)min.Y, (int)max.X - (int)min.X, (int)max.Y - (int)min.Y);

            return bound;
        }

        public bool PointIntersect(Vector3 mouse, Vector3 position, Vector2 dimension, float rotation)
        {
            Point p = new Point((int)(mouse.X - position.X - ((gameObject.transform.position.X - (_UnitToPixelRatio * _pos.Alignment.X)) * (dimension.X * 0.01f))),
                (int)(mouse.Y - position.Y - ((gameObject.transform.position.Y - (_UnitToPixelRatio * _pos.Alignment.Y)) * (dimension.Y * 0.01f))));
            if (GetWorldBound(position, dimension, rotation).Contains(new Point((int)mouse.X, (int)mouse.Y)))
            {
                return Sprite.GetPixel(GetWorldTexture(flatRender.DeviceManager,
                    flatRender.SpriteBatch,
                    position,
                    dimension,
                    rotation),
                    p).A > 0;
            }
            return false;
        }

        public void SetUPRatio(int pixels)
        {
            _UnitToPixelRatio = 1f / (float)pixels;
        }
        #endregion

        #region base overloaded

        #endregion

        #endregion
    }

    #endregion

    #region UIMenu

    public class UIMenu : UIGroup
    {
        #region Fields

        protected SortedList <int, UIButton> _sorted = new SortedList<int,UIButton>();
        private bool _focus = true;
        private List<int> _lostFocus = new List<int>();

        #endregion

        #region Constructors

        public UIMenu(GameObjectManager objMan, int id, Rectangle range, alignment align, params GameObject[] elements)
            : base(objMan, id, range, align, elements)
        {

        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        protected void UpdateElementGroups()
        {
            foreach(var p in _elements)
            {
                if(p.Value.uibutton != null)
                    _sorted.Add((int)p.Value.transform.position.Y, p.Value.uibutton);
            }
        }

        public void GrabFocus()
        {
            List<Component> menus = _objectManager.GetComponents<UIGroup>();
            foreach(Component comp in menus)
            {
                if (comp.GetType().IsSubclassOf(typeof(UIMenu)) && comp.Id != Id)
                {
                    if((comp as UIMenu).GetFocus())
                        _lostFocus.Add(comp.Id);
                }
            }
        }

        public void DiscardFocus()
        {
            foreach(int i in _lostFocus)
            {
                (_objectManager.GetComponent<UIGroup>(i) as UIMenu).RestoreFocus();
            }
        }

        public bool GetFocus()
        {
            if (_focus)
            {
                _focus = false;
                return true;
            }
            return false;
        }

        public void RestoreFocus()
        {
            _focus = true;
        }

        public override void FixedUpdate(float Delta, Game game)
        {
            if (_focus)
            {
                foreach (var p in _sorted)
                {
                    if (p.Value.PointIntersect(gameObject.transform.position - new Vector3(_pos.Alignment.X, _pos.Alignment.Y, 0),
                        new Vector2(_pos.Dimensions.Width, _pos.Dimensions.Height),
                        gameObject.transform.rotation.Z))
                    {
                        if (InputHandler.MButtonPressed("LMB") || InputHandler.GButtonPressed("A"))
                            p.Value.State = UIButtonState.pressed;
                        else
                            p.Value.State = UIButtonState.active;
                    }
                    else
                        p.Value.State = UIButtonState.inactive;
                }
            }
            base.Update(Delta, game);
        }

        public override void OnDelete()
        {
            DiscardFocus();
            base.OnDelete();
        }
        #endregion
    }

    #endregion

    #region Cursor

    public class Cursor : Component
    {
        #region Fields

        #endregion

        #region Constructor

        public Cursor(GameObjectManager objMan, int id) 
            : base(objMan, id)
        {
            
        }

        #endregion

        #region Methods

        public override void Update(float Delta, Game game)
        {
            gameObject.transform.position = new Vector3(InputHandler.MousePosition.X,
                InputHandler.MousePosition.Y,
                0f);
            base.Update(Delta, game);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gameObject.uitexture.Draw(spriteBatch, Vector3.Zero, new Vector2(100, 100), 0f);
        }

        #endregion
    }

    #endregion

    #region LightTexture

    public class LightTexture : Component
    {
        #region Fields

        private Color _color;
        private Texture2D _solid;
        private Color _target;
        private Color _source;
        private float _elapsed;
        private float _duration;

        #endregion

        #region Constructor

        public LightTexture(GameObjectManager objMan, int id, Rectangle res, Color color)
            : base(objMan, id)
        {
            _color = color;
            _elapsed = 0f;
            _duration = 0f;
            _target = Color.Black;
            createSolid(flatRender.DeviceManager);
        }

        #endregion

        #region Properties

        public float Transparence
        {
            get { return _color.A/(255/100); }
            set
            {
                if (value < 0f) _color.A = 0;
                else if (value > 1f) _color.A = 255;
                else
                    _color.A = (Byte)(value*255);
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
            }
        }

        #endregion

        #region Methods

        void createSolid(GraphicsDeviceManager device)
        {
            _solid = new Texture2D(device.GraphicsDevice, device.PreferredBackBufferWidth, device.PreferredBackBufferHeight);
            Color[] colors = new Color[_solid.Width * _solid.Height];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.White;
            _solid.SetData<Color>(colors);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_solid == null)
                createSolid(flatRender.DeviceManager);

            spriteBatch.Draw(_solid,
                _solid.Bounds,
                _color);
        }

        public void BlendTo(Color color, float time)
        {
            _target = color;
            _source = _color;
            _duration = time;
            _elapsed = 0f;
        }

        public void BlendTo(float transparency, float time)
        {
            _target = _color;
            _source = _color;
            _target.A = (Byte)((255 * 0.01f) * transparency);
            _duration = time;
            _elapsed = 0f;
        }

        public override void Update(float Delta, Game game)
        {
            if (_elapsed < _duration)
            {
                _elapsed += Delta;
                //_color = new Color(_color.ToVector4() - ((_color.ToVector4() - _target.ToVector4()) * Delta / _duration));
                _color = Color.Lerp(_source, _target, _elapsed / _duration);
            }
            base.Update(Delta, game);
        }

        #endregion
    }

    #endregion

    #endregion
}
