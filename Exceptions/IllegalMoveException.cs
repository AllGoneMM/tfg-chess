using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    internal class IllegalMoveException : Exception
    {
        public IllegalMoveException()
        {
        }

        public IllegalMoveException(string message) : base(message)
        {
        }

        public IllegalMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
