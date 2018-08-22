using System;

namespace MyMovies.BLL.Exceptions
{
    public class ImpossibleDeleteException : Exception
    {
        public ImpossibleDeleteException()
        {
        }

        public ImpossibleDeleteException(string message) : base(message)
        {
        }
    }
}