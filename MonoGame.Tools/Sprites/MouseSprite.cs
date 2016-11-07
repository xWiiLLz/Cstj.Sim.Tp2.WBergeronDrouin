using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Tools.Services;

namespace MonoGame.Tools.Sprites
{
    public class MouseSprite : AnimatedSprite
    {
        public MouseSprite(Texture2D texture, int rows, int columns)
            :base(texture,rows,columns)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            MouseService mouseService = ServicesHelper.GetService<MouseService>();
            _position = mouseService.CurrentPosition;

            base.Update(gameTime, clientBound);
        }
    }
}
