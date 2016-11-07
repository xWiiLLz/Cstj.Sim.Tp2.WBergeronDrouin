using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Tools.Services
{
    public class KeyboardService : GameComponent
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public KeyboardService(Game game)
            : base(game)
        {
            ServicesHelper.AddService<KeyboardService>(this);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key);
        }

        public bool IsAnyKeyPressed()
        {
            return _currentKeyboardState.GetPressedKeys().Length > 0;
        }


        #region Propriété pour les états
        public KeyboardState CurrentState
        {
            get
            {
                return _currentKeyboardState;
            }
        }

        public KeyboardState PreviousState
        {
            get
            {
                return _previousKeyboardState;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }
    }
}
