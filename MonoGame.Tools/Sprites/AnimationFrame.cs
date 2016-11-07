using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class AnimationFrame
    {

        public Rectangle SourceRectangle { get; set; }
        /// <summary>
        /// Duration for the frame to stay, in milliseconds
        /// </summary>
        public int Duration { get; set; }

        public AnimationFrame(Rectangle rectangle, int duration)
        {
            SourceRectangle = rectangle;
            Duration = duration;
        }
    }
}
