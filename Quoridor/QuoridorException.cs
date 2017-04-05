using System;

namespace Quoridor
{
    public class QuoridorException : Exception
    {
        public QuoridorException(string message) : base(message)
        {
        }
    }
}