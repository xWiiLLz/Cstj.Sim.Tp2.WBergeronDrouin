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
    public class GemSprite : CustomAnimatedSprite
    {
        private float _scale = 0.75f;
        public GemSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : base(texture, rows, columns, position, speed)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Texture, _position, currentAnimation.GetNextFrame().SourceRectangle, Color.White, 0f, Vector2.Zero, new Vector2(_scale), SpriteEffects.None, 0f);
        }

        public override Rectangle HitBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, (int)(this.currentAnimation.GetNextFrame().SourceRectangle.Width * _scale), (int)(this.currentAnimation.GetNextFrame().SourceRectangle.Height * _scale)); }
        }
    }
}
