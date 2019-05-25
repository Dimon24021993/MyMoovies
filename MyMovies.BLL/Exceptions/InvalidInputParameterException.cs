using System;

namespace MyMovies.BLL.Exceptions
{
    public class InvalidInputParameterException : Exception
    {
        public InvalidInputParameterException()
        {
        }

        public InvalidInputParameterException(string message) : base(message)
        {
        }
    }
}