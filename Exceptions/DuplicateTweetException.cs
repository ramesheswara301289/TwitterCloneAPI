// DuplicateTweetException.cs
using System;

namespace TwitterCloneAPI.Exceptions
{
    public class DuplicateTweetException : Exception
    {
        public DuplicateTweetException(string message) : base(message)
        {
        }
    }
}
