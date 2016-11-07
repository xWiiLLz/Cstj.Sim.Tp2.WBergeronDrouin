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
    public class KeyboardSprite : AnimatedSprite
    {
        private Vector2 _initialSpeed;
        public KeyboardSprite(Texture2D texture, int rows, int columns)
            :this(texture,rows,columns,Vector2.Zero,Vector2.Zero)
        {

        }
        public KeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position)
            : this(texture, rows, columns, position, Vector2.Zero, rows * columns)
        {

        }

        public KeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : this(texture, rows, columns, position, speed,rows*columns)
        {

        }

        public KeyboardSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed, int totalFrame, int millisecondsPerFrame = 35)
            :base(texture, rows, columns, position, speed,totalFrame,millisecondsPerFrame)
        {
            _initialSpeed = speed;
        }

        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            Vector2 direction = Vector2.Zero;
            KeyboardService keyboardService = ServicesHelper.GetService<KeyboardService>();
            
            if(keyboardService.IsKeyDown(Keys.Left))
            {
                direction.X -= 1;
            }
            if(keyboardService.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }
            if(keyboardService.IsKeyDown(Keys.Up))
            {
                direction.Y -= 1;
            }
            if(keyboardService.IsKeyDown(Keys.Down))
            {
                direction.Y += 1;
            }

            _speed = _initialSpeed * direction;
            base.Update(gameTime, clientBound);
        }
    }
}
