using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class NoLegalMovesException : Exception
    {
        public NoLegalMovesException()
        {
        }

        public NoLegalMovesException(string message) : base(message)
        {
        }

        public NoLegalMovesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
