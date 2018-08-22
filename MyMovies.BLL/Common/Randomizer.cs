using System;
using System.Security.Cryptography;

namespace MyMovies.BLL.Common
{
    public static class Randomizer
    {
        public static int GetRandomNumber()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return int.Parse($"{random:D8}");
        }
    }
}