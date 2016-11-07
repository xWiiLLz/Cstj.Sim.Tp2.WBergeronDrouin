using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Services;

namespace MonoGame.Tools.Screens
{
    public class DebugScreen : Screen
    {

        #region Membres privées
        private int fps;
        private int fpsCounter;
        private double fpsTimer;
        #endregion

        #region Information pour l'écran
        public string Screens { private get; set; }
        public string FocusedScreen { private get; set; }
        #endregion

        #region Position
        private Vector2 _leftCorner;
        #endregion

        #region Police
        private SpriteFont Arial8;
        #endregion

        private KeyboardService keyboardService;

        public DebugScreen(Game game)
            : base(game)
        {
            Name = "Debug";
            HasGrabFocus = false;
            State = ScreenState.Hidden;
            keyboardService = ServicesHelper.GetService<KeyboardService>();
        }

        public override void LoadContent()
        {
            //TODO: Remplacer par FontPool
            Arial8 = Game.Content.Load<SpriteFont>(@"Fonts\Arial8");
        }

        public override void HandleInput()
        {
            if (keyboardService.IsKeyPressed(Keys.F12))
            {
                if (State == ScreenState.Active)
                    State = ScreenState.Hidden;
                else if (State == ScreenState.Hidden)
                    State = ScreenState.Active;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //FPS
            if (gameTime.TotalGameTime.TotalMilliseconds > fpsTimer)
            {
                fps = fpsCounter;
                fpsTimer = gameTime.TotalGameTime.TotalMilliseconds + 1000;
                fpsCounter = 1;
            }
            else
            {
                fpsCounter += 1;
            }

            //Chaine des Screens et Focused String            
            int txtWidth = (int)Math.Max(Arial8.MeasureString(Screens).X, Arial8.MeasureString(FocusedScreen).X);
            int txtHeight = (int)Arial8.MeasureString(Screens).Y * 3;

            _leftCorner.X = Game.Window.ClientBounds.Width - (txtWidth + 15);
            _leftCorner.Y = Game.Window.ClientBounds.Height - (txtHeight + 5);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Arial8, fps.ToString(), _leftCorner, Color.White);
            SpriteBatch.DrawString(Arial8, Screens, new Vector2(_leftCorner.X, _leftCorner.Y + 12), Color.White);
            SpriteBatch.DrawString(Arial8, FocusedScreen, new Vector2(_leftCorner.X, _leftCorner.Y + 24), Color.White);
            SpriteBatch.End();
        }
    }
}
