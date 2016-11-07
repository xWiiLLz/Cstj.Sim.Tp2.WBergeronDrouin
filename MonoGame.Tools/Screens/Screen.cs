using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Managers;
using MonoGame.Tools.Services;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Tools.Screens
{
    public enum ScreenState
    {
        Active,
        Shutdown,
        Pause,
        Hidden
    }
    public abstract class Screen
    {

        #region Membres privés
        protected Game Game { get; set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected ScreenManager ScreenManager { get { return ServicesHelper.GetService<ScreenManager>(); } }
        protected KeyboardService KeyboardService { get { return ServicesHelper.GetService<KeyboardService>(); } }

        private static Random _random = new Random();

        //Surement dans les spécialisation
        //Add KeyBoard
        //Add Mouse
        //Add Fonts/Audio
        #endregion

        #region Propriétes
        public string Name { get; set; }
        public ScreenState State { get; set; }
        public bool IsFocused { get; set; }
        public bool HasGrabFocus { get; set; }
        public Random Random { get { return _random; } }
        #endregion

        public Screen(Game game)
        {
            Game = game;
            State = ScreenState.Active;
            IsFocused = false;
            HasGrabFocus = true;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Name = this.GetType().Name;
        }

        public virtual void LoadContent() { }
        public virtual void HandleInput() { }
        public virtual void Update(GameTime gameTime) { }
        public abstract void Draw(GameTime gameTime);
        public virtual void Unload()
        {
            State = ScreenState.Shutdown;
        }

        protected Texture2D LoadTexture(string ressourceName)
        {
            return Game.Content.Load<Texture2D>(ressourceName);
        }

        protected Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(Game.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);

            Color[] colors = new Color[1 * 1];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(color.ToVector3());
            }

            texture.SetData(colors);
            return texture;
        }


        protected T Load<T>(string ressourcePath)
        {
            return Game.Content.Load<T>(ressourcePath);
        }

    }
}
