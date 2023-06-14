using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class InvalidTileArrayException : Exception
    {
        public InvalidTileArrayException() : base() { }
        public InvalidTileArrayException(string message) : base(message) { }
        public InvalidTileArrayException(string message, Exception inner) : base(message, inner) { }
    }
}
