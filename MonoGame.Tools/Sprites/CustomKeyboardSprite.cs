using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class CustomKeyboardSprite : CustomAnimatedSprite
    {
        public enum State { Standing, WalkingLeft, WalkingRight, Jumping };
        private Vector2 _initialSpeed, _initialPosition;
        private Animation walkingAnimation, jumpingAnimation;
        private SpriteEffects spriteEffect;
        public State CurrentState { get; set; }
        private float _acceleration;

        public CustomKeyboardSprite(Texture2D texture, int rows, int columns)
            : this(texture, rows, columns, Vector2.Zero, Vector2.Zero)
        {

        }
        public CustomKeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position)
            : this(texture, rows, columns, position, Vector2.Zero)
        {

        }

        public CustomKeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : this(texture, rows, columns, position, speed, rows * columns, texture, rows, columns, rows * columns, 35)
        {

        }

        public CustomKeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed, int totalFrame,
             Texture2D walkingTexture, int walkingTextureRows, int walkingTextureColumns, int walkingTotalFrames, int millisecondsPerFrame = 35)
            : base(texture, rows, columns, position, speed, totalFrame, millisecondsPerFrame)
        {
            _initialSpeed = speed;
            walkingAnimation = new Animation(walkingTexture, walkingTexture.Width, walkingTexture.Height, walkingTextureRows, walkingTextureColumns, walkingTotalFrames, millisecondsPerFrame);
            jumpingAnimation = new Animation(texture, texture.Width, texture.Height, rows, columns, totalFrame, millisecondsPerFrame);
            _initialPosition = position;
            spriteEffect = SpriteEffects.None;
            _acceleration = 0.15f;
        }

        public void SetJumpingAnimation(Texture2D texture, int rows, int columns, int totalFrames, int millisecondsPerFrame)
        {
            jumpingAnimation = new Animation(texture, texture.Width, texture.Height, rows, columns, totalFrames, millisecondsPerFrame);
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
                    direction.X -= 1;
                    CurrentState = State.WalkingLeft;
                    currentAnimation = walkingAnimation;
                    spriteEffect = SpriteEffects.None;
                }
                if (keyboardService.IsKeyDown(Keys.Right))
                {
                    direction.X += 1;
                    CurrentState = State.WalkingRight;
                    currentAnimation = walkingAnimation;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }

                _speed = _initialSpeed * direction;

                if (keyboardService.IsKeyDown(Keys.Space))
                {
                    CurrentState = State.Jumping;
                    currentAnimation = jumpingAnimation;
                    _speed.Y = -3f;
                    _initialSpeed.Y = _speed.Y;
                    direction.Y = 1;
                }
            }
            else
            {
                _speed.Y = _speed.Y + _acceleration;
                
                if (_speed.Y > _initialSpeed.Y * -1)
                {
                    CurrentState = State.Standing;
                    _position.Y = _initialPosition.Y;
                    jumpingAnimation.End();
                }
            }

            _position += _speed;
            currentAnimation.Update(gameTime);
            if(_position.X<0)
            {
                _position.X = 0;
            }
            if(_position.X + currentAnimation.GetNextFrame().SourceRectangle.Width > clientBound.Width)
            {
                _position.X = clientBound.Width - currentAnimation.GetNextFrame().SourceRectangle.Width;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle position = new Rectangle((int)_position.X, (int)_position.Y, currentAnimation.GetNextFrame().SourceRectangle.Width, currentAnimation.GetNextFrame().SourceRectangle.Height);
            spriteBatch.Draw(currentAnimation.Texture, position, currentAnimation.GetNextFrame().SourceRectangle, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);
        }
    }
}
