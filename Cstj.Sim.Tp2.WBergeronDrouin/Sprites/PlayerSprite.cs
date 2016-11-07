using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Services;
using MonoGame.Tools.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cstj.Sim.Tp2.WBergeronDrouin.Sprites
{
    public class PlayerSprite : CustomAnimatedSprite
    {
        #region Properties
        private enum State { Standing, WalkingLeft, WalkingRight, Jumping };
        private State CurrentState { get; set; }

        public int Score { get; set; }

        private Vector2 _initialSpeed, _initialPosition;
        private Animation _walkingAnimation, _jumpingAnimation;
        private SoundEffect _jumpingSoundEffect, _coinSoundEffect;
        private SpriteEffects spriteEffect;
        private const float _ACCELERATION = 0.15f;
        #endregion

        #region Constructors
        public PlayerSprite(Texture2D texture, int rows, int columns)
            : this(texture, rows, columns, Vector2.Zero, Vector2.Zero)
        {

        }
        public PlayerSprite(Texture2D texture, int rows, int columns, Vector2 position)
            : this(texture, rows, columns, position, Vector2.Zero)
        {

        }

        public PlayerSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : this(texture, rows, columns, position, speed, rows * columns, texture, rows, columns, rows * columns, 35)
        {

        }

        public PlayerSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed, int totalFrame,
             Texture2D walkingTexture, int walkingTextureRows, int walkingTextureColumns, int walkingTotalFrames, int millisecondsPerFrame = 35)
            : base(texture, rows, columns, position, speed, totalFrame, millisecondsPerFrame)
        {
            _initialSpeed = speed;
            _walkingAnimation = new Animation(walkingTexture, walkingTexture.Width, walkingTexture.Height, walkingTextureRows, walkingTextureColumns, walkingTotalFrames, millisecondsPerFrame);
            _jumpingAnimation = new Animation(texture, texture.Width, texture.Height, rows, columns, totalFrame, millisecondsPerFrame);
            _initialPosition = position;
            spriteEffect = SpriteEffects.None;
            Score = 0;
        }
        #endregion

        /// <summary>
        /// Sets the jumping animation sprite
        /// </summary>
        /// <param name="texture">Texture of the jumping sprite</param>
        /// <param name="rows">Number of rows in the given texture</param>
        /// <param name="columns">Number of columns in the given texture</param>
        /// <param name="totalFrames">Number of total frames</param>
        /// <param name="millisecondsPerFrame">Time of animation for each frame</param>
        public void SetJumpingAnimation(Texture2D texture, int rows, int columns, int totalFrames, int millisecondsPerFrame)
        {
            _jumpingAnimation = new Animation(texture, texture.Width, texture.Height, rows, columns, totalFrames, millisecondsPerFrame);
        }

        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            Vector2 direction = Vector2.Zero;
            if (CurrentState != State.Jumping)
            {

                spriteEffect = SpriteEffects.None;
                currentAnimation = originalAnimation;
                CurrentState = State.Standing;
                KeyboardService keyboardService = ServicesHelper.GetService<KeyboardService>();

                if (keyboardService.IsKeyDown(Keys.Left))
                {
                    direction.X = -1;
                    CurrentState = State.WalkingLeft;
                    currentAnimation = _walkingAnimation;
                    currentAnimation.SetFrameDuration(50);
                    spriteEffect = SpriteEffects.None;
                    if (keyboardService.IsKeyDown(Keys.LeftShift))
                    {
                        direction.X *= 1.5f;
                        currentAnimation.SetFrameDuration(25);
                    }
                }
                if (keyboardService.IsKeyDown(Keys.Right))
                {
                    direction.X = 1;
                    CurrentState = State.WalkingRight;
                    currentAnimation = _walkingAnimation;
                    currentAnimation.SetFrameDuration(50);
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    if(keyboardService.IsKeyDown(Keys.LeftShift))
                    {
                        direction.X *= 1.5f;
                        currentAnimation.SetFrameDuration(25);
                    }
                }
                

                _speed = _initialSpeed * direction;

                if (keyboardService.IsKeyDown(Keys.Space))
                {
                    CurrentState = State.Jumping;
                    currentAnimation = _jumpingAnimation;
                    _speed.Y = -5f;
                    _initialSpeed.Y = _speed.Y;
                    direction.Y = 1;
                    PlayJumpSoundEffect();
                }
            }
            else
            {
                _speed.Y = _speed.Y + _ACCELERATION;

                if (_speed.Y > _initialSpeed.Y * -1)
                {
                    CurrentState = State.Standing;
                    _position.Y = _initialPosition.Y;
                    _jumpingAnimation.End();
                }
            }

            _position += _speed;
            currentAnimation.Update(gameTime);
            if (_position.X < 0)
            {
                _position.X = 0;
            }
            if (_position.X + currentAnimation.GetNextFrame().SourceRectangle.Width > clientBound.Width)
            {
                _position.X = clientBound.Width - currentAnimation.GetNextFrame().SourceRectangle.Width;
            }

        }
        /// <summary>
        /// Sets the coin sound effect;
        /// </summary>
        /// <param name="effect"></param>
        public void SetCoinSoundEffect(SoundEffect effect)
        {
            _coinSoundEffect = effect;
        }
        /// <summary>
        /// Plays the coin sound effect, if it's set
        /// </summary>
        private void PlayCoinSoundEffect()
        {
            if(_coinSoundEffect != null)
            {
                _coinSoundEffect.Play();
            }
        }

        /// <summary>
        /// Sets the jumping sound effect.
        /// </summary>
        /// <param name="effect">Source of the sound</param>
        public void SetJumpSoundEffect(SoundEffect effect)
        {
            _jumpingSoundEffect = effect;
        }
        /// <summary>
        /// Plays the jumping sound effect, if it's set.
        /// </summary>
        private void PlayJumpSoundEffect()
        {
            if(_jumpingSoundEffect != null)
            {
                _jumpingSoundEffect.Play();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle position = new Rectangle((int)_position.X, (int)_position.Y, currentAnimation.GetNextFrame().SourceRectangle.Width, currentAnimation.GetNextFrame().SourceRectangle.Height);
            spriteBatch.Draw(currentAnimation.Texture, position, currentAnimation.GetNextFrame().SourceRectangle, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);
        }
        public void Collides(Sprite sprite)
        {
            //  If the sprite is an enemy, we remove 100 points of the score
            if(sprite.GetType() == typeof(EnnemiSprite))
            {
                Score -= 100;
            }
            else if(sprite.GetType() == typeof(DiamondSprite))
            {
                Score += 50;
                PlayCoinSoundEffect();
            }
            else if(sprite.GetType() == typeof(GemSprite))
            {
                Score += 10;
                PlayCoinSoundEffect();
            }
        }
    }
}
