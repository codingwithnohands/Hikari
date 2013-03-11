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
    public class flatRender
    {
        #region Fields

        private static SpriteBatch _spriteBatch;
        private static GraphicsDeviceManager _graphics;

        private RenderTarget2D _foregroundRenderTarget;
        private RenderTarget2D _interactiveRenderTarget;
        private RenderTarget2D _backgroundRenderTarget;

        private RenderTarget2D _interfaceRenderTarget;

        private RenderTarget2D _postProcessTarget;

        private RenderTarget2D _finalRenderTarget;

        private List<DisplayMode> _supportedModes;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for 2D Renderer
        /// </summary>
        /// <param name="graphics"></param>
        public flatRender(GraphicsDeviceManager graphics)
        {
            if(_graphics == null)_graphics = graphics;
            if (_spriteBatch == null)_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            _foregroundRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _interactiveRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _backgroundRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _finalRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _postProcessTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _interfaceRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        #endregion

        #region Properties

        public static SpriteBatch SpriteBatch
        {
            get {return _spriteBatch; }
        }

        public static GraphicsDeviceManager DeviceManager
        {
            get { return _graphics; }
        }

        public static Rectangle Resolution
        {
            get { return new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); }
            set 
            { 
                _graphics.PreferredBackBufferHeight = value.Height;
                _graphics.PreferredBackBufferWidth = value.Width;
                _graphics.ApplyChanges();
            }
        }

        public static bool Fullscreen
        {
            get { return _graphics.IsFullScreen; }
            set
            {
                if (value && !_graphics.IsFullScreen)
                {
                    _graphics.ToggleFullScreen();
                    _graphics.ApplyChanges();
                }
                else if (!value && _graphics.IsFullScreen)
                {
                    _graphics.ToggleFullScreen();
                    _graphics.ApplyChanges();
                }
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Draw to Target method
        /// 
        /// Draws list of sprites on the passed rendertarget 
        /// with the transform given by the passed matrix
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sprites"></param>
        /// <param name="transforms"></param>
        private void DrawToTarget(RenderTarget2D target, List<GameObject> objects, Matrix transforms)
        {
            if (objects == null || target == null) return;
            _spriteBatch.GraphicsDevice.SetRenderTarget(target);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            if(target == _backgroundRenderTarget)
                _spriteBatch.GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullClockwise, null, transforms);
            foreach (GameObject obj in objects)
            {
                if(obj.sprite != null)
                    obj.sprite.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

        /// <summary>
        /// Combine layers method
        /// combines background interactive and foreground layers on one layer
        /// </summary>
        private void CombineLayers()
        {
            _spriteBatch.GraphicsDevice.SetRenderTarget(_finalRenderTarget);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_backgroundRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_interactiveRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_foregroundRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        /// <summary>
        /// render layers
        /// Renders all 3 layers after getting all sprites for each layer in seperate lists
        /// </summary>
        /// <param name="objMan"></param>
        private void RenderLayers(GameObjectManager objMan)
        {
            Camera main = objMan.GetComponents<Camera>().FirstOrDefault() as Camera;
            if (main != null)
            {
                main.UpdateProjections(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                main.UpdateViewMatrix();
                main.UpdateProjectionMatrix();

                List<GameObject> foregroundSprites = objMan.Quadtree.GetRange(main, 0);
                List<GameObject> interactiveSprites = objMan.Quadtree.GetRange(main, 1);
                List<GameObject> backgroundSprites = objMan.Quadtree.GetRange(main, 2);

                DrawToTarget(_foregroundRenderTarget, foregroundSprites, main.viewProjectionMatrix
                    * Matrix.CreateScale(1.5f)
                    * Matrix.CreateTranslation(new Vector3(-main.vision.X * 1.5f, -main.vision.Y * 1.5f, 0.0f)));
                DrawToTarget(_interactiveRenderTarget, interactiveSprites, main.viewProjectionMatrix);
                DrawToTarget(_backgroundRenderTarget, backgroundSprites, main.viewProjectionMatrix
                    * Matrix.CreateScale(0.5f)
                    * Matrix.CreateTranslation(new Vector3(main.vision.X * 1.5f, main.vision.Y * 1.5f, 0.0f)));
            }
        }

        private void PostProcessLayers(GameObjectManager objMan)
        {
                PostProcessLayer(objMan, 0, _backgroundRenderTarget);
                PostProcessLayer(objMan, 1, _interactiveRenderTarget);
                PostProcessLayer(objMan, 2, _foregroundRenderTarget);
        }

        private void PostProcessLayer(GameObjectManager objMan, int layer, RenderTarget2D target)
        {
            _spriteBatch.GraphicsDevice.SetRenderTarget(_postProcessTarget);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullNone, null);
            List<Component> comps = objMan.GetComponents<LightTexture>();
            _spriteBatch.Draw(target, target.Bounds, Color.White);
            if (comps != null)
            {
                foreach (Component comp in comps)
                {
                    if (comp.gameObject.transform.layer == layer)
                        (comp as LightTexture).Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();

            RenderTarget2D buffer = target;
            _spriteBatch.GraphicsDevice.SetRenderTarget(target);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullNone, null);

            _spriteBatch.Draw(_postProcessTarget, _postProcessTarget.Bounds, Color.White);

            _spriteBatch.End();

        }

        private void ApplyBloom()
        {
            
        }

        public void RenderUI(GameObjectManager objMan)
        {
            _spriteBatch.GraphicsDevice.SetRenderTarget(_interfaceRenderTarget);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            List<Component> comps = objMan.GetComponents<UIGroup>();
            if (comps != null)
            {
                if (comps.Count != 0)
                {
                    Matrix UIMatrix = Matrix.CreateScale(_graphics.PreferredBackBufferWidth * 0.01f, _graphics.PreferredBackBufferHeight * 0.01f, 1f);
                    _spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.AnisotropicClamp,
                        null,
                        RasterizerState.CullCounterClockwise,
                        null,
                        UIMatrix);

                    foreach (Component c in comps)
                        (c as UIGroup).Draw(_spriteBatch);

                    List<Component> components = objMan.GetComponents<Cursor>();
                    if(components != null)
                    {
                        Cursor cur = components.FirstOrDefault() as Cursor;
                        if (cur != null)
                            cur.Draw(_spriteBatch);
                    }

                    _spriteBatch.End();
                }
            }
        }

        void createFinal()
        {
            _spriteBatch.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_finalRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_interfaceRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        /// <summary>
        /// renders Scene including PostProcessing effects
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="objMan"></param>
        public void RenderScene(GameObjectManager objMan)
        {
            RenderLayers(objMan);
            PostProcessLayers(objMan);
            CombineLayers();
            //PostProcess Combined
            RenderUI(objMan);
            //PostProcess UI
            createFinal();
        }

        void UpdateResolutions()
        {
            DisplayModeCollection supModes = _graphics.GraphicsDevice.Adapter.SupportedDisplayModes;
            _supportedModes = supModes.ToList<DisplayMode>();
        }

        void ChangeResolution()
        {
            
        }

        #endregion

    }

    public class threeDRender
    {
    }
}
