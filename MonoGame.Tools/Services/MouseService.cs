using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Tools.Services
{
    public class MouseService : GameComponent
    {
        #region Membres Privés
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public MouseService(Game game)
            : base(game)
        {
            ServicesHelper.AddService<MouseService>(this);
        }
        #endregion

        #region Propriétés
        public Vector2 CurrentPosition
        {
            get
            {
                return new Vector2(_currentMouseState.X, _currentMouseState.Y);
            }
        }

        public Vector2 PreviousPosition
        {
            get
            {
                return new Vector2(_previousMouseState.X, _previousMouseState.Y);
            }
        }

        public MouseState CurrentState
        {
            get
            {
                return _currentMouseState;
            }
        }

        public MouseState PreviousState
        {
            get
            {
                return _previousMouseState;
            }
        }
        #endregion

        #region Méthodes
        public override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        public bool IsLeftButtonDown()
        {
            return _previousMouseState.LeftButton == ButtonState.Released
                    && _currentMouseState.LeftButton == ButtonState.Pressed;
        }
        #endregion
    }
}
