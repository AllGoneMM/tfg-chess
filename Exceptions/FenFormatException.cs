using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class FenFormatException : Exception
    {
        public FenFormatException() : base() { }
        public FenFormatException(string message) : base(message) { }
        public FenFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
