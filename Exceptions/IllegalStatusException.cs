using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class IllegalStatusException : Exception
    {
        public IllegalStatusException() : base() { }
        public IllegalStatusException(string message) : base(message) { }
        public IllegalStatusException(string message, Exception inner) : base(message, inner) { }
    }
}
