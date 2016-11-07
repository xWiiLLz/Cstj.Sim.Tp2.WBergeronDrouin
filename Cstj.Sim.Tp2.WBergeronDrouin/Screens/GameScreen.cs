using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cstj.Sim.Tp2.WBergeronDrouin.Sprites;
using MonoGame.Tools.Sprites;
using Microsoft.Xna.Framework.Audio;

namespace Cstj.Sim.Tp2.WBergeronDrouin.Screens
{
    public class GameScreen : Screen
    {
        private PlayerSprite player;
        private Texture2D background, pause, diamond, gem;
        private SoundEffect jumpSoundEffect, coinSoundEffect;
        private List<Texture2D> texEnnemies;
        private List<EnnemiSprite> ennemies;
        private List<Sprite> preciousStones;
        private SpriteFont font;
        private const float MILLISECONDS_PER_SPAWN = 1500;
        private float _millisecondsSinceLastSpawn = 0;
        private const float ENEMY_SCALE = 0.5f;
        public GameScreen(Game game) : base(game)
        {
            texEnnemies = new List<Texture2D>();
            ennemies = new List<EnnemiSprite>();
            preciousStones = new List<Sprite>();
        }

        public override void LoadContent()
        {
            background = LoadTexture(@"Sprites/Backgrounds/background");
            pause = LoadTexture(@"Sprites/Backgrounds/pause");
            Texture2D texPlayerIdle = LoadTexture(@"Sprites/Player/Idle");
            Texture2D texPlayerRun = LoadTexture(@"Sprites/Player/Run");
            Texture2D texPlayerJump = LoadTexture(@"Sprites/Player/Jump");
            diamond = LoadTexture(@"Sprites/diamond");
            gem = LoadTexture(@"Sprites/gem");
            player = new PlayerSprite(texPlayerIdle, 1, 1, new Vector2(200, 342), new Vector2(3, 3), 1, texPlayerRun, 1, 10, 10);
            player.SetJumpingAnimation(texPlayerJump, 1, 11, 11, 75);
            jumpSoundEffect = Load<SoundEffect>(@"Audio/jumping");
            coinSoundEffect = Load<SoundEffect>(@"Audio/coin");
            player.SetJumpSoundEffect(jumpSoundEffect);
            player.SetCoinSoundEffect(coinSoundEffect);
            font = Load<SpriteFont>(@"Fonts/Arial8");

            // Loading enemies textures
            for (int i = 0; i < 10; i++)
            {
                texEnnemies.Add(LoadTexture(String.Format(@"Sprites/Ennemies/{0}", i)));
            }

            //  Initial Spawn
            Spawn();

        }

        public override void HandleInput()
        {
            if (KeyboardService.IsKeyPressed(Keys.L) && IsFocused)
            {
                Unload();
            }
            if (KeyboardService.IsKeyPressed(Keys.Escape) && State != ScreenState.Pause)
            {
                this.State = ScreenState.Pause;
            }
            else if (KeyboardService.IsKeyPressed(Keys.Escape) && State == ScreenState.Pause)
            {
                this.State = ScreenState.Active;
            }
        }
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);

            //  Is the game finished?
            #region Look if game is finished
            if (player.Score <= -500)
            {
                this.ScreenManager.GameWon = false;
                this.ScreenManager.AddScreen<EndScreen>();
                Unload();
            }
            if(player.Score >=1000)
            {
                this.ScreenManager.GameWon = true;
                this.ScreenManager.AddScreen<EndScreen>();
                Unload();
            }
            #endregion

            //  Spawning gems and enemies every MILLISECONDS_PER_SPAWN
            _millisecondsSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if(_millisecondsSinceLastSpawn >= MILLISECONDS_PER_SPAWN)
            {
                Spawn();
                _millisecondsSinceLastSpawn -= MILLISECONDS_PER_SPAWN;
            }

            foreach (var enemy in ennemies.ToList())
            {
                //  Check if an enemy collides with the player
                if (enemy.HitBox.Intersects(player.HitBox))
                {
                    player.Collides(enemy);
                    //  The enemy needs to be destroyed
                    ennemies.Remove(enemy);
                    continue;
                }
                enemy.Update(gameTime, Game.Window.ClientBounds);
                if (enemy.IsDestroyable())
                {
                    ennemies.Remove(enemy);
                }
            }

            foreach (var stone in preciousStones.ToList())
            {
                //  Check if a stone is colliding with the player
                if(stone.HitBox.Intersects(player.HitBox))
                {
                    player.Collides(stone);
                    //  The stone needs to be removed
                    preciousStones.Remove(stone);

                    //  We continue the loop
                    continue;
                }

                //  If the stone is out of bounds, we delete it
                if(stone.HitBox.Y > Game.Window.ClientBounds.Height)
                {
                    preciousStones.Remove(stone);
                    continue;
                }
                stone.Update(gameTime, Game.Window.ClientBounds);
                
            }

        }

        private void Spawn()
        {
            Random rand = new Random();

            #region Stones
            //  Amount of stones spawned, random number betwenn 2 and 5
            int stones = rand.Next(2, 6);
            int stoneType = 0;
            for (int i = 0; i < stones; i++)
            {
                Sprite stone;

                //  One chance on 8 of being a diamond
                stoneType = rand.Next(1, 9);

                //  Random speed
                Vector2 stoneSpeed, stonePosition;
                if (stoneType == 1)
                {
                    //  Is a diamond
                    stonePosition = new Vector2(rand.Next(0, Game.Window.ClientBounds.Width + 1 - diamond.Width), 0);
                    stoneSpeed = new Vector2(0, rand.Next(3, 9));
                    stone = new DiamondSprite(diamond, stonePosition, stoneSpeed);
                }
                else
                {
                    stonePosition = new Vector2(rand.Next(0, Game.Window.ClientBounds.Width + 1 - (gem.Width/5)), 0);
                    stoneSpeed = new Vector2(0, rand.Next(1, 6));
                    stone = new GemSprite(gem, 1, 5, stonePosition, stoneSpeed);
                }

                //  Add newly created stone to the list
                preciousStones.Add(stone);
            }
            #endregion
            #region Enemies
            //  Amount of enemies spawned, random number between 0 and 7
            int numberOfEnemies = rand.Next(0, 8);

            for (int i = 0; i < numberOfEnemies; i++)
            {
                //  Random number for the texture of the enemy to spawn
                int textureIndex = rand.Next(0, texEnnemies.Count);

                //  Random position
                Vector2 enemyPosition = new Vector2(rand.Next(0, Game.Window.ClientBounds.Width - (int)(texEnnemies[textureIndex].Width * ENEMY_SCALE)), 0);

                //  Random speed
                Vector2 enemySpeed = new Vector2(0, rand.Next(2, 8));
                EnnemiSprite enemy = new EnnemiSprite(texEnnemies[textureIndex], 1, 1, enemyPosition, enemySpeed, 1, 35, ENEMY_SCALE);
                ennemies.Add(enemy);
            }
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            player.Draw(SpriteBatch);
            //  Drawing enemies
            foreach (var ennemi in ennemies)
            {
                ennemi.Draw(SpriteBatch);
            }

            //  Drawing stones
            foreach (var stone in preciousStones)
            {
                stone.Draw(SpriteBatch);
            }
            //  Affichage du score
            SpriteBatch.DrawString(font, String.Format("Score : {0}", player.Score), Vector2.Zero, Color.Black);
            if (State == ScreenState.Pause)
            {
                SpriteBatch.Draw(pause, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
                string text = "Appuyez sur Escape pour continuer la partie...";
                SpriteBatch.DrawString(font,text, new Vector2((Game.Window.ClientBounds.Width/2) - (font.MeasureString(text).X/2), 300), Color.Black,0f,Vector2.Zero,1f,SpriteEffects.None,0f);
            }
            SpriteBatch.End();
        }
    }
}