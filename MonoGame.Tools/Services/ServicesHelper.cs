using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame.Tools.Services
{
    public static class ServicesHelper
    {
        private static Game _game;

        public static Random RandomGenerator { get; private set; }

        static ServicesHelper()
        {
            RandomGenerator = new Random();
        }

        public static Game Game
        {
            set { _game = value; }
        }

        public static void AddService<T>(T service) where T : class
        {
            _game.Services.AddService(typeof(T), service);
        }

        public static T GetService<T>() where T : class
        {
            return (T)_game.Services.GetService(typeof(T));
        }
    }
}
