using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LameChicken
{
    #region inputHandler
    // Game Component for Input Detection
    public static class InputHandler
    {
        #region Fields

        private static Vector3 _mousePos;
        private static float _mouseSensivity = 0.02f;
        private static float _padSensivity = 0.5f;

        private static KeyboardState _keyboardState;
        private static KeyboardState _prevKeyboardState;

        private static MouseState _mouseState;
        private static MouseState _prevMouseState;

        private static GamePadState _gamepadState;
        private static GamePadState _prevGamepadState;

        #endregion

        #region Constructor

        static InputHandler()
        {
            Initialize();
        }

        #endregion

        #region Properties

        //keyboard Properties
        public static KeyboardState KeyboardState
        {
            get { return _keyboardState; }
        }

        public static KeyboardState PrevKeyboardState
        {
            get { return _prevKeyboardState; }
        }

        //Mouse properties
        public static MouseState MouseState
        {
            get { return _mouseState; }
        }

        public static MouseState PrevMouseState
        {
            get { return _prevMouseState; }
        }

        public static Vector3 MousePosition
        {
            get { return _mousePos; }
        }
        //Gamepad properties
        public static GamePadState GamepadState
        {
            get { return _gamepadState; }
        }

        public static GamePadState PrevGamepadState
        {
            get { return _prevGamepadState; }
        }

        #endregion

        #region Methods

        #region System Methods

        public static void Initialize()
        {
            Mouse.SetPosition(268, 268);
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _gamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public static void Update(float Delta, Game game)
        {
            if (game.IsActive)
            {
                _prevKeyboardState = _keyboardState;
                _prevMouseState = _mouseState;
                _prevGamepadState = _gamepadState;

                _keyboardState = Keyboard.GetState();
                _mouseState = Mouse.GetState();
                _gamepadState = GamePad.GetState(PlayerIndex.One);
                UpdateMousePosition();
                updateGamepadPosition();
            }
        }

        public static void FixedUpdate(float Delta)
        {

        }

        #endregion

        #region Keyboard methods

        public static bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        //Key Up
        public static bool KeyUp(Keys key)
        {
            return _keyboardState.IsKeyUp(key) && _prevKeyboardState.IsKeyDown(key);
        }

        //Key Down
        public static bool KeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _prevKeyboardState.IsKeyUp(key);
        }

        #region TextEntry

        public static char GetTextEntry()
        {
            bool upperCase = _keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift);
            if (KeyDown(Keys.A))
            {
                if (upperCase)
                    return 'A';
                return 'a';
            }
            if (KeyDown(Keys.B))
            {
                if (upperCase)
                    return 'B';
                return 'b';
            }
            if (KeyDown(Keys.C))
            {
                if (upperCase)
                    return 'C';
                return 'c';
            }
            if (KeyDown(Keys.D))
            {
                if (upperCase)
                    return 'D';
                return 'd';
            }
            if (KeyDown(Keys.E))
            {
                if (upperCase)
                    return 'E';
                return 'E';
            }
            if (KeyDown(Keys.F))
            {
                if (upperCase)
                    return 'F';
                return 'f';
            }
            if (KeyDown(Keys.G))
            {
                if (upperCase)
                    return 'G';
                return 'g';
            }
            if (KeyDown(Keys.H))
            {
                if (upperCase)
                    return 'H';
                return 'h';
            }
            if (KeyDown(Keys.I))
            {
                if (upperCase)
                    return 'I';
                return 'i';
            }
            if (KeyDown(Keys.J))
            {
                if (upperCase)
                    return 'J';
                return 'j';
            }
            if (KeyDown(Keys.K))
            {
                if (upperCase)
                    return 'K';
                return 'k';
            }
            if (KeyDown(Keys.L))
            {
                if (upperCase)
                    return 'L';
                return 'l';
            }
            if (KeyDown(Keys.M))
            {
                if (upperCase)
                    return 'M';
                return 'm';
            }
            if (KeyDown(Keys.N))
            {
                if (upperCase)
                    return 'N';
                return 'n';
            }
            if (KeyDown(Keys.O))
            {
                if (upperCase)
                    return 'O';
                return 'o';
            }
            if (KeyDown(Keys.P))
            {
                if (upperCase)
                    return 'P';
                return 'p';
            }
            if (KeyDown(Keys.Q))
            {
                if (upperCase)
                    return 'Q';
                return 'q';
            }
            if (KeyDown(Keys.R))
            {
                if (upperCase)
                    return 'R';
                return 'r';
            }
            if (KeyDown(Keys.S))
            {
                if (upperCase)
                    return 'S';
                return 's';
            }
            if (KeyDown(Keys.T))
            {
                if (upperCase)
                    return 'T';
                return 't';
            }
            if (KeyDown(Keys.U))
            {
                if (upperCase)
                    return 'U';
                return 'u';
            }
            if (KeyDown(Keys.V))
            {
                if (upperCase)
                    return 'V';
                return 'v';
            }
            if (KeyDown(Keys.W))
            {
                if (upperCase)
                    return 'W';
                return 'w';
            }
            if (KeyDown(Keys.X))
            {
                if (upperCase)
                    return 'X';
                return 'x';
            }
            if (KeyDown(Keys.Y))
            {
                if (upperCase)
                    return 'Y';
                return 'y';
            }
            if (KeyDown(Keys.Z))
            {
                if (upperCase)
                    return 'Z';
                return 'z';
            }
            return ' ';
        }

        #endregion

        #endregion

        #region Mouse methods

        //Key Pressed
        public static bool MButtonPressed(string mb)
        {
            switch (mb)
            {
                case "LMB":
                    return _prevMouseState.LeftButton == ButtonState.Pressed;
                case "MMB":
                    return _prevMouseState.LeftButton == ButtonState.Pressed;
                case "RMB":
                    return _prevMouseState.LeftButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        //Key Up
        public static bool MButtonUp(string mb)
        {
            switch (mb)
            {
                case "LMB":
                    return _mouseState.LeftButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed;
                case "MMB":
                    return _mouseState.MiddleButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed;
                case "RMB":
                    return _mouseState.RightButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        //Key Down
        public static bool MButtonDown(string mb)
        {
            switch (mb)
            {
                case "LMB":
                    return _mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released;
                case "MMB":
                    return _mouseState.MiddleButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released;
                case "RMB":
                    return _mouseState.RightButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        private static void UpdateMousePosition()
        {
            _mousePos = new Vector3(_mousePos.X + ((_mouseState.X - 268) * _mouseSensivity),
                _mousePos.Y + ((_mouseState.Y - 268) * _mouseSensivity),
                _mouseState.ScrollWheelValue - _prevMouseState.ScrollWheelValue);

            if (_mousePos.X > 100)
                _mousePos.X = 100f;
            else if (_mousePos.X < 0)
                _mousePos.X = 0f;
            if (_mousePos.Y > 100)
                _mousePos.Y = 100f;
            else if (_mousePos.Y < 0)
                _mousePos.Y = 0f;
            Mouse.SetPosition(268, 268);
        }

        #endregion

        #region Gamepad Functions

        //Key Pressed
        public static bool GButtonPressed(string gb)
        {
            switch (gb)
            {
                case "A":
                    return _prevGamepadState.Buttons.A == ButtonState.Pressed;
                case "B":
                    return _prevGamepadState.Buttons.B == ButtonState.Pressed;
                case "X":
                    return _prevGamepadState.Buttons.X == ButtonState.Pressed;
                case "Y":
                    return _prevGamepadState.Buttons.Y == ButtonState.Pressed;
                case "LB":
                    return _prevGamepadState.Buttons.LeftShoulder == ButtonState.Pressed;
                case "RB":
                    return _prevGamepadState.Buttons.RightShoulder == ButtonState.Pressed;
                case "back":
                    return _prevGamepadState.Buttons.Back == ButtonState.Pressed;
                case "start":
                    return _prevGamepadState.Buttons.Start == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        //Key Up
        public static bool GButtonUp(string gb)
        {
            switch (gb)
            {
                case "A":
                    return _prevGamepadState.Buttons.A == ButtonState.Pressed && _gamepadState.Buttons.A == ButtonState.Released;
                case "B":
                    return _prevGamepadState.Buttons.B == ButtonState.Pressed && _gamepadState.Buttons.B == ButtonState.Released;
                case "X":
                    return _prevGamepadState.Buttons.X == ButtonState.Pressed && _gamepadState.Buttons.X == ButtonState.Released;
                case "Y":
                    return _prevGamepadState.Buttons.Y == ButtonState.Pressed && _gamepadState.Buttons.Y == ButtonState.Released;
                case "LB":
                    return _prevGamepadState.Buttons.LeftShoulder == ButtonState.Pressed && _gamepadState.Buttons.LeftShoulder == ButtonState.Released;
                case "RB":
                    return _prevGamepadState.Buttons.RightShoulder == ButtonState.Pressed && _gamepadState.Buttons.RightShoulder == ButtonState.Released;
                case "back":
                    return _prevGamepadState.Buttons.Back == ButtonState.Pressed && _gamepadState.Buttons.Back == ButtonState.Released;
                case "start":
                    return _prevGamepadState.Buttons.Start == ButtonState.Pressed && _gamepadState.Buttons.Start == ButtonState.Released;
                default:
                    return false;
            }
        }

        //Key Down
        public static bool GButtonDown(string gb)
        {
            switch (gb)
            {
                case "A":
                    return _prevGamepadState.Buttons.A == ButtonState.Released && _gamepadState.Buttons.A == ButtonState.Pressed;
                case "B":
                    return _prevGamepadState.Buttons.B == ButtonState.Released && _gamepadState.Buttons.B == ButtonState.Pressed;
                case "X":
                    return _prevGamepadState.Buttons.X == ButtonState.Released && _gamepadState.Buttons.X == ButtonState.Pressed;
                case "Y":
                    return _prevGamepadState.Buttons.Y == ButtonState.Released && _gamepadState.Buttons.Y == ButtonState.Pressed;
                case "LB":
                    return _prevGamepadState.Buttons.LeftShoulder == ButtonState.Released && _gamepadState.Buttons.LeftShoulder == ButtonState.Pressed;
                case "RB":
                    return _prevGamepadState.Buttons.RightShoulder == ButtonState.Released && _gamepadState.Buttons.RightShoulder == ButtonState.Pressed;
                case "back":
                    return _prevGamepadState.Buttons.Back == ButtonState.Released && _gamepadState.Buttons.Back == ButtonState.Pressed;
                case "start":
                    return _prevGamepadState.Buttons.Start == ButtonState.Released && _gamepadState.Buttons.Start == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool GStickDirection(string direction)
        {
            switch (direction)
            {
                case "left":
                    return (_gamepadState.ThumbSticks.Left.X <= -0.2f);
                case "right":
                    return (_gamepadState.ThumbSticks.Left.X >= 0.2f);
                case "up":
                    return (_gamepadState.ThumbSticks.Left.X >= 0.2f);
                case "down":
                    return (_gamepadState.ThumbSticks.Left.X <= -0.2f);
                default:
                    return false;
            }
        }

        private static void updateGamepadPosition()
        {
            _mousePos = _mousePos = new Vector3(_mousePos.X + (_gamepadState.ThumbSticks.Left.X * _padSensivity),
                _mousePos.Y + (_gamepadState.ThumbSticks.Left.Y * -1f * _padSensivity),
                _mouseState.ScrollWheelValue - _prevMouseState.ScrollWheelValue);

            if (_mousePos.X > 100)
                _mousePos.X = 100f;
            else if (_mousePos.X < 0)
                _mousePos.X = 0f;
            if (_mousePos.Y > 100)
                _mousePos.Y = 100f;
            else if (_mousePos.Y < 0)
                _mousePos.Y = 0f;
        }

        #endregion

        #endregion

    }

    #endregion

    #region DeviceMapper

    //still not implemented, needed for button mapping and multiple devices
    class DeviceMapper
    {
        #region Fields

        //private Dictionary<int, ButtonState> ButtonMapping;
        //private Dictionary<int, Keys> KeyMapping;

        #endregion

        #region Constructor

        public DeviceMapper()
        {
            Initialize();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        void Initialize()
        {
           
        }

        #endregion
    }

    #endregion
}
