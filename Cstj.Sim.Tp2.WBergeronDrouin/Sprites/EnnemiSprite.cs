using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cstj.Sim.Tp2.WBergeronDrouin.Sprites
{
    public class EnnemiSprite : CustomAnimatedSprite
    {
        // Random variable is static so we don't get the same number with close cpu clock
        static Random random = new Random();
        private float _scale;
        private bool hasHitGround = false, isDestroyable = false;
        int millisecondsBeforeDestruction, millisecondsSinceHitGround = 0;
        public EnnemiSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed, int totalFrames, int millisecondsPerFrame = 35, float scale = 1f)
            : base(texture, rows, columns, position, speed, totalFrames, millisecondsPerFrame)
        {
            _scale = scale;
        }

        public override Rectangle HitBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, (int)(this._texture.Width * _scale), (int)(this._texture.Height * _scale)); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Texture, _position, currentAnimation.GetNextFrame().SourceRectangle, Color.White, 0f, Vector2.Zero, new Vector2(_scale), SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            if (_position.Y >= 358 && !hasHitGround)
            {
                millisecondsBeforeDestruction = random.Next(300, 1001);
                hasHitGround = true;
                _position.Y = 358;
                _speed = Vector2.Zero;
            }
            else if (hasHitGround)
            {
                millisecondsSinceHitGround += gameTime.ElapsedGameTime.Milliseconds;
                if (millisecondsSinceHitGround >= millisecondsBeforeDestruction)
                {
                    // Destruction
                    isDestroyable = true;
                }
            }
            base.Update(gameTime, clientBound);
        }
        public bool IsDestroyable()
        {
            return isDestroyable;
        }
    }
}
