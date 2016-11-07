using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class AnimatedSprite : Sprite
    {
        protected readonly int _rows;
        protected readonly int _columns;

        protected int _millesecondsPerFrame = 35;
        protected int _timeSinceLastFrame = 0;
        protected int _currentFrame;

        protected readonly int _totalFrames;
        protected readonly int _frameWidth;
        protected readonly int _frameHeight;

        public enum States {Standing, MovingRight, MovingLeft };

        protected List<AnimationFrame> standingFrames, walkingLeftFrames, walkingRightFrames;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
            : this(texture, rows, columns, Vector2.Zero, Vector2.Zero)
        { }

        public AnimatedSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : this(texture, rows, columns, position, speed, rows * columns)
        { }

        public AnimatedSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed,
                                int totalFrames, int millisecondsPerFrame = 35)
            : base(texture, position, speed)
        {
            _rows = rows;
            _columns = columns;

            _totalFrames = totalFrames;
            _currentFrame = 0;

            _frameHeight = _texture.Height / _rows;
            _frameWidth = _texture.Width / _columns;

            _millesecondsPerFrame = millisecondsPerFrame;

        }


        public override void Update(GameTime gameTime, Rectangle clientBound)
        {

            _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (_timeSinceLastFrame > _millesecondsPerFrame)
            {
                _timeSinceLastFrame -= _millesecondsPerFrame;
                _currentFrame++;
                if (_currentFrame == _totalFrames)
                {
                    _currentFrame = 0;
                }

            }

            base.Update(gameTime, clientBound);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int rangee = _currentFrame / _columns;
            int colonne = _currentFrame % _columns;

            Rectangle source = new Rectangle(colonne * _frameWidth,
                                             rangee * _frameHeight,
                                             _frameWidth, _frameHeight);

            spriteBatch.Draw(_texture, _position, source, Color.White);

        }

        public override Rectangle HitBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _frameWidth, _frameHeight); }
        }
    }
}
