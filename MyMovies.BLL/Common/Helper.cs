using System;

namespace MyMovies.BLL.Common
{
    public static class Helper
    {
        public const string MatchMobilePhonePattern = @"^\+380\d{9}";

        public static T TryValue<T>(string val, T def = default(T))
        {
            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch (Exception)
            {
                return def;
            }
        }
    }
}