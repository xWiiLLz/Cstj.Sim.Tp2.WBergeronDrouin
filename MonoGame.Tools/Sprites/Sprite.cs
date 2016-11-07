using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Vector2 _speed;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            _position = Vector2.Zero;
            _speed = Vector2.Zero;
        }

        public Sprite(Texture2D texture, Vector2 position)
            : this(texture)
        {
            _position = position;
        }

        public Sprite(Texture2D texture, Vector2 position, Vector2 speed)
            :this(texture, position)
        {
            _speed = speed;
        }

        protected virtual void HandleOutOfBound(Rectangle clientBounds) { }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Update();

            HandleOutOfBound(clientBounds);
        }

        public virtual void Update()
        {
            _position += _speed;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public virtual Rectangle HitBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height); }
        }

    }
}
