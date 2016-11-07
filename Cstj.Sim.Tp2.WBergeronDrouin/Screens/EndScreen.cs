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
        private Texture2D _background;
        private bool _gameIsWon;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">Game variable</param>
        /// <param name="won">Did the player win the game?</param>
        public EndScreen(Game game, bool won) : base(game)
        {
            _gameIsWon = won;
        }

        public override void LoadContent()
        {
            if(_gameIsWon)
            {
                _background = LoadTexture(@"Sprites/Backgrounds/victoire");
            }
            else
            {
                _background = LoadTexture(@"Sprites/Backgrounds/perdu");
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
            SpriteBatch.Draw(_background, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            SpriteBatch.End();
        }
    }
}
