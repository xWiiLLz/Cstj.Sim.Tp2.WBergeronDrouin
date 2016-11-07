using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cstj.Sim.Tp2.WBergeronDrouin.Screens
{
    public class EndScreen : Screen
    {
        private Texture2D background;

        public EndScreen(Game game) : base(game)
        {
        }

        public override void LoadContent()
        {
            if(ScreenManager.GameWon)
            {
                background = LoadTexture(@"Sprites/Backgrounds/victoire");
            }
            else
            {
                background = LoadTexture(@"Sprites/Backgrounds/perdu");
            }
        }

        public override void HandleInput()
        {
            if (KeyboardService.IsKeyDown(Keys.Space) && IsFocused)
            {
                ScreenManager.AddScreen<GameScreen>();
                Unload();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            SpriteBatch.End();
        }
    }
}
