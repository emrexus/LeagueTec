﻿using System;
namespace Adept_AIO.SDK.Junk
{
    class GetRandom
    {
        private static Random _random;

        public static void Init()
        {
            _random = new Random();
        }

        public static int Next(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
