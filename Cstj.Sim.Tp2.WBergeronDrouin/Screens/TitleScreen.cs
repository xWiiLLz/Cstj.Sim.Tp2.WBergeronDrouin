using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cstj.Sim.Tp2.WBergeronDrouin.Screens
{
    public class TitleScreen : Screen
    {
        private Texture2D background;

        public TitleScreen(Game game) : base(game)
        {

        }

        public override void LoadContent()
        {
            background = LoadTexture(@"Sprites/Backgrounds/titre");
        }

        public override void HandleInput()
        {
            if (KeyboardService.IsAnyKeyPressed() && IsFocused)
            {
                ScreenManager.AddScreen<GameScreen>();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0,0,Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            SpriteBatch.End();
        }
    }
}
