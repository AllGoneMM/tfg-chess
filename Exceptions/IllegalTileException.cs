using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class IllegalTileException : Exception
    {
        public IllegalTileException() : base() { }
        public IllegalTileException(string message) : base(message) { }
        public IllegalTileException(string message, Exception inner) : base(message, inner) { }
    }
}
