using System;

namespace MyMovies.BLL.Exceptions
{
    public class InvalidDbOperationException : Exception
    {
        public InvalidDbOperationException()
        {
        }

        public InvalidDbOperationException(string message) : base(message)
        {
        }
    }
}