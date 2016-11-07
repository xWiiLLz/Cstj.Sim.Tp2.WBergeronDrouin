using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Screens;
using MonoGame.Tools.Services;

namespace MonoGame.Tools.Managers
{

    public sealed class ScreenManager : DrawableGameComponent
    {
        #region Membres privés
        private List<Screen> _screens;
        private bool _isDebugging;
        private DebugScreen _debugScreen;
        #endregion

        #region Proprietes
        public bool GameWon { get; set; }
        #endregion

        public ScreenManager(Game game, bool isDebuging = false)
            : base(game)
        {
            _isDebugging = isDebuging;
            _screens = new List<Screen>();
            ServicesHelper.AddService<ScreenManager>(this);

            if (_isDebugging)
            {
                _debugScreen = new DebugScreen(Game);
                _debugScreen.LoadContent();
            }
        }

        public void AddScreen(Screen screen)
        {
            screen.LoadContent();
            _screens.Add(screen);
        }


        public void AddScreen<T>() where T : Screen
        {
            Type typeT = typeof(T);
            T screen = (T)Activator.CreateInstance(typeT, this.Game);
            AddScreen(screen);
        }

        public void UnloadScreen(string screenName)
        {
            Screen screenToUnload = _screens.Where(s => s.Name == screenName).FirstOrDefault();
            if (screenToUnload == null)
            {
                throw new ObjectDisposedException(screenToUnload.Name, "Cet écran n'existe pas");
            }
            UnloadScreen(screenToUnload);

        }

        public void UnloadScreen(Screen screen)
        {
            screen.Unload();
        }

        public override void Update(GameTime gameTime)
        {

            //1. Supprimer les écrans et enlever le focus à toutes les écrans
            _screens.RemoveAll(s => s.State == ScreenState.Shutdown);
            _screens.ForEach(s => s.IsFocused = false);

            //2. Focus des Screen (juste un screen avec le focus) prendre le dernier de la liste
            var screenToFocusOn = _screens.LastOrDefault(s => s.HasGrabFocus);

            //Si une fenêtre doit prendre le focus
            if (screenToFocusOn != null)
            {
                //3. Handle Input de l'écran avec le focus
                screenToFocusOn.IsFocused = true;
            }

            //Juste les fenêtres en Active et Hidden seront prise en compte
            var screenToUpdateOrHandle = _screens.Where(s => s.State == ScreenState.Active || s.State == ScreenState.Hidden || s.State == ScreenState.Pause).ToList();

            //HandleInput et Update les screen nécessaire
            screenToUpdateOrHandle.ForEach(s =>
                {
                    if (Game.IsActive)
                    {
                        s.HandleInput();
                    }
                    if (s.State != ScreenState.Pause)
                        s.Update(gameTime);
                });

            if (_isDebugging)
                HandleDebugScreen(gameTime, screenToFocusOn);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                if (screen.State == ScreenState.Active || screen.State == ScreenState.Pause)
                {
                    screen.Draw(gameTime);
                }
            }

            if (_isDebugging && _debugScreen.State == ScreenState.Active)
                _debugScreen.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void HandleDebugScreen(GameTime gameTime, Screen focusedScreen)
        {
            StringBuilder builder = new StringBuilder("Focused screen: ");
            if (focusedScreen != null)
                _debugScreen.FocusedScreen = builder.Append(focusedScreen.Name).ToString();
            else
                _debugScreen.FocusedScreen = builder.ToString();

            builder.Clear();

            builder = new StringBuilder("Screens: ");

            foreach (Screen screen in _screens)
            {
                builder.Append(screen.Name).Append(", ");
            }

            _debugScreen.Screens = builder.ToString().Substring(0, builder.Length - 2);
            _debugScreen.HandleInput();
            _debugScreen.Update(gameTime);
        }
    }
}
